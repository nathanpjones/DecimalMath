using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathExtensions
{
    public abstract class MatrixBase
    {

        private readonly Type _t;
        protected readonly int Size;
        protected decimal[,] M;

        /// <summary>
        /// Constructs a new matrix with height and width of <paramref name="size"/>.
        /// </summary>
        /// <param name="size">Size of matrix.</param>
        protected MatrixBase(int size)
        {
            _t = GetType();
            Size = size;

            Reset();    // initialize to identity matrix
        }

        #region  Matrix Operations

        /// <summary>
        /// Resets matrix to the identity matrix.
        /// </summary>
        public void Reset()
        {
            M = GetIdentityMatrix();
        }

        /// <summary>
        /// Gets the direct value of the matrix at the given row and column.
        /// </summary>
        /// <param name="row">The row at which to get the value.</param>
        /// <param name="column">The column at which to get the value.</param>
        public decimal this[int row, int column]
        {
            get { return M[row, column]; }
        }

        /// <summary>
        /// Gets the identity matrix for this size matrix.
        /// </summary>
        protected decimal[,] GetIdentityMatrix()
        {
            var m = new decimal[Size, Size];

            // For an identity matrix, the diagonal (top left to bot right)
            // numbers are 1 and everything else is 0.
            for (var i = 0; i <= Size - 1; i++)
            {
                m[i, i] = 1;
            }

            return m;
        }

        /// <summary>
        /// Multiplies this matrix by another matrix (other x this) and returns a third matrix.
        /// Equivalent to applying all transformations from the other matrix AFTER this matrix.
        /// </summary>
        /// <param name="other">The other matrix to multiply by.</param>
        public MatrixBase Multiply(MatrixBase other)
        {
            var m = (MatrixBase)Activator.CreateInstance(_t);

            m.M = Multiply(other.M, M);

            return m;
        }

        /// <summary>
        /// Multiply two square matrices of the same size.
        /// </summary>
        /// <param name="m1">A square matrix.</param>
        /// <param name="m2">A square matrix.</param>
        protected static decimal[,] Multiply(decimal[,] m1, decimal[,] m2)
        {
            var size = m1.GetLength(0);

            // Verify that matrices are square and of the same size
            if (m1.GetLength(1) != size || m2.GetLength(0) != size || m2.GetLength(1) != size)
            {
                throw new Exception(string.Format("Can't multiply a {0}x{1} matrix with a {2}x{3} matrix!",
                                                  m1.GetLength(0), m1.GetLength(1),
                                                  m2.GetLength(0), m2.GetLength(1)));

            }

            var m = new decimal[size, size];

            // Select destination row
            for (var r = 0; r < size; r++)
            {
                // Select destination column
                for (var c = 0; c < size; c++)
                {
                    m[r, c] = 0;

                    for (var i = 0; i < size; i++)
                    {
                        m[r, c] += m1[r, i] * m2[i, c];
                    }
                }
            }

            return m;
        }

        /// <summary>
        /// Multiplies a column matrix by a square matrix of the same height.
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        protected static decimal[] Multiply(decimal[] m1, decimal[,] m2)
        {

            var size = m1.GetLength(0);

            // Check that the second matrix is square and of the appropriate size
            if (m2.GetLength(0) != size || m2.GetLength(1) != size)
            {
                throw new Exception(string.Format("Can't multiply a {0}x1 matrix with a {1}x{2} matrix!", 
                                                  m1.GetLength(0), 
                                                  m2.GetLength(0), m2.GetLength(1)));

            }

            var m = new decimal[size];

            // Select row
            for (var r = 0; r < size; r++)
            {
                m[r] = 0;

                for (var i = 0; i <= size - 1; i++)
                {
                    m[r] += m1[i] * m2[r, i];
                }

            }

            return m;
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
        public MatrixBase Copy()
        {
            var m = (MatrixBase)Activator.CreateInstance(_t);

            if (m.Size != Size)
                throw new Exception("Parameterless constructor for " + _t.Name + " did not properly set size of matrix!");

            Array.Copy(M, m.M, M.Length);

            return m;
        }
    }

}
