using System;

namespace DecimalMath
{
    /// <summary>
    /// A base class to support implementation of a square matrix to be used for affine transforms.
    /// </summary>
    /// <typeparam name="TSelf">Reference to the type inheriting this base class.</typeparam>
    public abstract class TransformationMatrixBase<TSelf> where TSelf : TransformationMatrixBase<TSelf>, new()
    {
        /// <summary>
        /// The width / height of the square matrix.
        /// </summary>
        public readonly int Size;
        /// <summary>
        /// The raw matrix as a two-dimensional array. Stored as [row, column].
        /// </summary>
        protected decimal[,] M;

        /// <summary>
        /// Constructs a new matrix with height and width of <paramref name="size"/>.
        /// </summary>
        /// <param name="size">Size of matrix.</param>
        protected TransformationMatrixBase(int size)
        {
            Size = size;

            // initialize to identity matrix
            M = Matrix.GetIdentityMatrix(Size);
        }

        /// <summary>
        /// Constructs a new matrix with height and width of <paramref name="size"/>.
        /// </summary>
        /// <param name="size">Size of matrix.</param>
        /// <param name="values">Values to use to initialize the matrix. These values are not used
        /// directly, but are rather copied. Addressed as [row, column].</param>
        protected TransformationMatrixBase(int size, decimal[,] values)
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

        /// <summary>
        /// Sets a row using the given values as if they were a row matrix.
        /// </summary>
        /// <param name="row">The row to set.</param>
        /// <param name="values">Values to set the row. Treated as a row matrix.</param>
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
        public TSelf Multiply<TOther>(TransformationMatrixBase<TOther> other) where TOther : TransformationMatrixBase<TOther>, new()
        {
            var m = new TSelf();

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
        public TSelf InPlaceTransform<TTrans>(ref TTrans element) where TTrans : ITransformable<TSelf, TTrans>
        {
            element = element.Transform((TSelf)this);
            return (TSelf)this;
        }

        /// <summary> Transforms objects and returns the results by reference. </summary>
        public TSelf InPlaceTransform<TTrans1, TTrans2>(ref TTrans1 element1, ref TTrans2 element2)
             where TTrans1 : ITransformable<TSelf, TTrans1>
             where TTrans2 : ITransformable<TSelf, TTrans2>
        {
            element1 = Transform(element1);
            element2 = Transform(element2);
            return (TSelf)this;
        }

        /// <summary> Transforms objects and returns the results by reference. </summary>
        public TSelf InPlaceTransform<TTrans1, TTrans2, TTrans3>(ref TTrans1 element1, ref TTrans2 element2, ref TTrans3 element3)
             where TTrans1 : ITransformable<TSelf, TTrans1>
             where TTrans2 : ITransformable<TSelf, TTrans2>
             where TTrans3 : ITransformable<TSelf, TTrans3>
        {
            element1 = Transform(element1);
            element2 = Transform(element2);
            element3 = Transform(element3);
            return (TSelf)this;
        }

        /// <summary> Transforms objects and returns the results by reference. </summary>
        public TSelf InPlaceTransform<TTrans1, TTrans2, TTrans3, TTrans4>(ref TTrans1 element1, ref TTrans2 element2, ref TTrans3 element3, ref TTrans4 element4)
             where TTrans1 : ITransformable<TSelf, TTrans1>
             where TTrans2 : ITransformable<TSelf, TTrans2>
             where TTrans3 : ITransformable<TSelf, TTrans3>
             where TTrans4 : ITransformable<TSelf, TTrans4>
        {
            element1 = Transform(element1);
            element2 = Transform(element2);
            element3 = Transform(element3);
            element4 = Transform(element4);
            return (TSelf)this;
        }

        /// <summary> Transforms objects and returns the results by reference. </summary>
        public TSelf InPlaceTransform<TTrans1, TTrans2, TTrans3, TTrans4, TTrans5>(ref TTrans1 element1, ref TTrans2 element2, ref TTrans3 element3, ref TTrans4 element4, ref TTrans5 element5)
             where TTrans1 : ITransformable<TSelf, TTrans1>
             where TTrans2 : ITransformable<TSelf, TTrans2>
             where TTrans3 : ITransformable<TSelf, TTrans3>
             where TTrans4 : ITransformable<TSelf, TTrans4>
             where TTrans5 : ITransformable<TSelf, TTrans5>
        {
            element1 = Transform(element1);
            element2 = Transform(element2);
            element3 = Transform(element3);
            element4 = Transform(element4);
            element5 = Transform(element5);
            return (TSelf)this;
        }

        /// <summary>
        /// Transforms an object that supports transformations by this matrix type.
        /// </summary>
        public TTransformable Transform<TTransformable>(ITransformable<TSelf, TTransformable> obj)
        {
            return obj.Transform((TSelf)this);
        }

        #endregion

        /// <summary>
        /// Creates a deep copy of this matrix.
        /// </summary>
        public TSelf Copy()
        {
            var m = new TSelf();

            if (m.Size != Size)
                throw new Exception("Parameterless constructor for " + typeof(TSelf).Name + " did not properly set size of matrix!");

            Array.Copy(M, m.M, M.Length);

            return m;
        }
    }

}
