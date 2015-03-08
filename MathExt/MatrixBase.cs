using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathExtensions
{
    public abstract class MatrixBase
    {

        private readonly Type _t; // TODO: Refactor to generic?
        protected readonly int _size;

        protected decimal[,] _m;

        protected MatrixBase(int size)
        {
            _t = this.GetType();
            _size = size;

            Reset();
        }

        #region "        Matrix Operations"


        public void Reset()
        {
            _m = _GetIdentityMatrix();

        }

        /// <summary>
        /// Gets the direct value of the matrix at the given row and column.
        /// </summary>
        /// <param name="row">The row at which to get the value.</param>
        /// <param name="column">The column at which to get the value.</param>
        public decimal this[int row, int column]
        {
            get { return _m[row, column]; }
        }

        /// <summary>
        /// Gets the identity matrix for this size matrix.
        /// </summary>
        protected decimal[,] _GetIdentityMatrix()
        {

            decimal[,] m = new decimal[_size, _size];

            // For an identity matrix, the diagonal (top left to bot right)
            // numbers are 1 and everything else is 0.
            for (int i = 0; i <= _size - 1; i++)
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

            m._m = _Multiply(other._m, this._m);

            return m;

        }
        protected static decimal[,] _Multiply(decimal[,] m1, decimal[,] m2)
        {

            int size = m1.GetLength(0);


            if (m1.GetLength(1) != size || m2.GetLength(0) != size || m2.GetLength(1) != size)
            {
                throw new Exception(string.Format("Can't multiply a {0}x{1} matrix with a {2}x{3} matrix!", m1.GetLength(0), m1.GetLength(1), m2.GetLength(0), m2.GetLength(1)));

            }

            decimal[,] m = new decimal[size, size];
            int r = 0;
            int c = 0;
            int i = 0;


            for (r = 0; r <= size - 1; r++)
            {

                for (c = 0; c <= size - 1; c++)
                {
                    m[r, c] = 0;

                    for (i = 0; i <= size - 1; i++)
                    {
                        m[r, c] += m1[r, i] * m2[i, c];
                    }

                }

            }

            return m;

        }
        protected static decimal[] _Multiply(decimal[] m1, decimal[,] m2)
        {

            int size = m1.GetLength(0);


            if (m2.GetLength(0) != size || m2.GetLength(1) != size)
            {
                throw new Exception(string.Format("Can't multiply a {0}x1 matrix with a {1}x{2} matrix!", m1.GetLength(0), m2.GetLength(0), m2.GetLength(1)));

            }

            decimal[] m = new decimal[size];
            int r = 0;
            int i = 0;


            for (r = 0; r <= size - 1; r++)
            {
                m[r] = 0;

                for (i = 0; i <= size - 1; i++)
                {
                    m[r] += m1[i] * m2[r, i];
                }

            }

            return m;

        }

        #endregion

        #region "        Applying To Objects    "

        /// <summary> Transforms a object and returns the result ByRef. </summary>
        /// <param name="element">The element to transform.</param>
        public void InPlaceTransform(ref object element)
        {
            element = Transform(element);
        }
        /// <summary> Transforms objects and returns the results ByRef. </summary>
        /// <param name="element1">A element to transform.</param>
        /// <param name="element2">A element to transform.</param>
        public void InPlaceTransform(ref object element1, ref object element2)
        {
            element1 = Transform(element1);
            element2 = Transform(element2);
        }
        /// <summary> Transforms objects and returns the results ByRef. </summary>
        /// <param name="element1">A element to transform.</param>
        /// <param name="element2">A element to transform.</param>
        /// <param name="element3">A element to transform.</param>
        public void InPlaceTransform(ref object element1, ref object element2, ref object element3)
        {
            element1 = Transform(element1);
            element2 = Transform(element2);
            element3 = Transform(element3);
        }
        /// <summary> Transforms objects and returns the results ByRef. </summary>
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
        /// <summary> Transforms objects and returns the results ByRef. </summary>
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
        /// strongly typed overloads).
        /// </summary>
        /// <param name="obj">The element to transform.</param>
        /// <remarks>
        /// NOTE: If you add a public Transform routine, make sure its type is added to
        /// this generic Transform routine.
        /// </remarks>
        public abstract object Transform(object obj);

        #endregion

        public object Clone()
        {

            var m = (MatrixBase)Activator.CreateInstance(_t);

            if (m._size != _size)
                throw new Exception("Parameterless constructor for " + _t.Name + " did not properly set size of matrix!");

            Array.Copy(this._m, m._m, this._m.Length);

            return m;

        }

    }

}
