using System;

namespace DecimalMath
{
    /// <summary>
    /// Matrix operations against a matrix stored as a two-dimensional Decimal array where values
    /// are addressed as [row, column].
    /// </summary>
    public static class Matrix
    {
        /// <summary>
        /// Gets the identity matrix for this size matrix.
        /// </summary>
        public static decimal[,] GetIdentityMatrix(int size)
        {
            var m = new decimal[size, size];

            // For an identity matrix, the diagonal (top left to bot right)
            // numbers are 1 and everything else is 0.
            for (var i = 0; i < size; i++)
            {
                m[i, i] = 1;
            }

            return m;
        }

        /// <summary>
        /// Multiply two matrices.
        /// </summary>
        /// <param name="m1">A matrix.</param>
        /// <param name="m2">A matrix.</param>
        public static decimal[,] Multiply(decimal[,] m1, decimal[,] m2)
        {
            // Verify that matrices are compatible
            var columns1 = m1.GetLength(1);
            var rows2 = m2.GetLength(0);
            if (columns1 != rows2)
            {
                throw new Exception(string.Format("Can't multiply a {0}x{1} matrix with a {2}x{3} matrix!",
                                                  m1.GetLength(0), m1.GetLength(1),
                                                  m2.GetLength(0), m2.GetLength(1)));
            }

            var prodRows = m1.GetLength(0); // rows from m1
            var prodCols = m2.GetLength(1); // columns from m2
            var m = new decimal[prodRows, prodCols];

            // Select destination row
            for (var r = 0; r < prodRows; r++)
            {
                // Select destination column
                for (var c = 0; c < prodCols; c++)
                {
                    m[r, c] = 0;

                    for (var i = 0; i < columns1; i++)
                    {
                        m[r, c] += m1[r, i] * m2[i, c];
                    }
                }
            }

            return m;
        }
        
        /// <summary>
        /// Converts a one dimensional array to rows in a two dimensional column matrix.
        /// </summary>
        public static decimal[,] ToColumn(decimal[] m)
        {
            var rows = m.Length;
            var colMatrix = new decimal[rows, 1];
            for (var row = 0; row < rows; row++)
            {
                colMatrix[row, 0] = m[row];
            }
            return colMatrix;
        }
        
        /// <summary>
        /// Converts a one dimensional array to rows in a two dimensional row matrix.
        /// </summary>
        public static decimal[,] ToRow(decimal[] m)
        {
            var columns = m.Length;
            var rowMatrix = new decimal[1, columns];
            for (var col = 0; col < columns; col++)
            {
                rowMatrix[0, col] = m[col];
            }
            return rowMatrix;
        }

        /// <summary>
        /// Converts a column or row matrix into a simple array.
        /// </summary>
        public static decimal[] RowOrColumnToArray(decimal[,] m)
        {
            if (m.GetLength(0) == 1)
            {
                // Convert row to array
                var columns = m.GetLength(1);
                var ret = new decimal[columns];
                for (var col = 0; col < columns; col++)
                {
                    ret[col] = m[0, col];
                }
                return ret;
            }
            else if (m.GetLength(1) == 1)
            {
                // Convert column to array
                var rows = m.GetLength(0);
                var ret = new decimal[rows];
                for (var row = 0; row < rows; row++)
                {
                    ret[row] = m[row, 0];
                }
                return ret;
            }
            else
            {
                throw new ArgumentException("Matrix is not a single column or row.", "m");
            }
        }        
    }
}
