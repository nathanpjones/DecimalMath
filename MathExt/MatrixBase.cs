using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExtensions
{

    public abstract class MatrixBase<T> where T : MatrixBase<T>, new()
    {
        public readonly int Size;
        protected decimal[,] M;

        /// <summary>
        /// Constructs a new matrix with height and width of <paramref name="size"/>.
        /// </summary>
        /// <param name="size">Size of matrix.</param>
        protected MatrixBase(int size)
        {
            Size = size;

            // initialize to identity matrix
            M = Matrix.GetIdentityMatrix(Size);
        }

        /// <summary>
        /// Constructs a new matrix with height and width of <paramref name="size"/>.
        /// </summary>
        /// <param name="size">Size of matrix.</param>
        protected MatrixBase(int size, decimal[,] values)
        {
            Size = size;

            if (values.GetLength(0) != size || values.GetLength(1) != size)
                throw new ArgumentException("Matrix initialization values are not of the correct size!", "values");

            M = new decimal[size, size];
            Array.Copy(values, M, values.Length);
        }

        #region  Matrix Operations

        /// <summary>
        /// Gets the direct value of the matrix at the given row and column.
        /// </summary>
        /// <param name="row">The row at which to get the value.</param>
        /// <param name="column">The column at which to get the value.</param>
        public decimal this[int row, int column]
        {
            get { return M[row, column]; }
            set { M[row, column] = value; }
        }

        public void SetRow(int row, decimal[] values)
        {
            if (values.Length != Size) 
                throw new ArgumentException("Number of values does not match number of columns in a row.", "values");

            for (var col = 0; col < Size; col++)
            {
                M[row, col] = values[col];
            }
        }

        /// <summary>
        /// Multiplies this matrix by another matrix (this x other) and returns a third matrix.
        /// </summary>
        /// <param name="other">The other matrix to multiply by.</param>
        public T Multiply<TOther>(MatrixBase<TOther> other) where TOther: MatrixBase<TOther>, new()
        {
            var m = new T();

            m.M = Matrix.Multiply(M, other.M);

            return m;
        }

        /// <summary>
        /// Applies the transform to a column matrix.
        /// </summary>
        /// <param name="columnMatrix">Column matrix with length equal to <see cref="Size"/>.</param>
        public decimal[] Transform(decimal[] columnMatrix)
        {
            if (columnMatrix.Length != Size)
                throw new ArgumentException(string.Format("Length of column matrix should be {0} but is {1}.", Size, columnMatrix.Length), "columnMatrix");
            
            var twoDArray = Matrix.ToColumn(columnMatrix);
            var matrixResult = Matrix.Multiply(M, twoDArray);
            var arrayResult = Matrix.RowOrColumnToArray(matrixResult);

            return arrayResult;
        }

        #endregion

        #region  Applying To Objects

        /// <summary> Transforms an object and returns the result by reference. </summary>
        /// <param name="element">The element to transform.</param>
        public void InPlaceTransform(ref object element)
        {
            element = Transform(element);
        }

        /// <summary> Transforms objects and returns the results by reference. </summary>
        /// <param name="element1">A element to transform.</param>
        /// <param name="element2">A element to transform.</param>
        public void InPlaceTransform(ref object element1, ref object element2)
        {
            element1 = Transform(element1);
            element2 = Transform(element2);
        }
        /// <summary> Transforms objects and returns the results by reference. </summary>
        /// <param name="element1">A element to transform.</param>
        /// <param name="element2">A element to transform.</param>
        /// <param name="element3">A element to transform.</param>
        public void InPlaceTransform(ref object element1, ref object element2, ref object element3)
        {
            element1 = Transform(element1);
            element2 = Transform(element2);
            element3 = Transform(element3);
        }
        /// <summary> Transforms objects and returns the results by reference. </summary>
        /// <param name="element1">A element to transform.</param>
        /// <param name="element2">A element to transform.</param>
        /// <param name="element3">A element to transform.</param>
        /// <param name="element4">A element to transform.</param>
        public void InPlaceTransform(ref object element1, ref object element2, ref object element3, ref object element4)
        {
            element1 = Transform(element1);
            element2 = Transform(element2);
            element3 = Transform(element3);
            element4 = Transform(element4);
        }
        /// <summary> Transforms objects and returns the results by reference. </summary>
        /// <param name="element1">A element to transform.</param>
        /// <param name="element2">A element to transform.</param>
        /// <param name="element3">A element to transform.</param>
        /// <param name="element4">A element to transform.</param>
        /// <param name="element5">A element to transform.</param>
        public void InPlaceTransform(ref object element1, ref object element2, ref object element3, ref object element4, ref object element5)
        {
            element1 = Transform(element1);
            element2 = Transform(element2);
            element3 = Transform(element3);
            element4 = Transform(element4);
            element5 = Transform(element5);
        }

        /// <summary>
        /// Transforms a geometric element. Must be a supported element (see
        /// strongly typed overloads in inheriting classes).
        /// </summary>
        /// <param name="obj">The element to transform.</param>
        /// <remarks>
        /// NOTE: If you add a public Transform routine, make sure its type is added to
        /// this generic Transform routine.
        /// </remarks>
        public abstract object Transform(object obj);

        #endregion

        /// <summary>
        /// Creates a deep copy of this matrix.
        /// </summary>
        public T Copy()
        {
            var m = new T();

            if (m.Size != Size)
                throw new Exception("Parameterless constructor for " + typeof(T).Name + " did not properly set size of matrix!");

            Array.Copy(M, m.M, M.Length);

            return m;
        }
    }

}
