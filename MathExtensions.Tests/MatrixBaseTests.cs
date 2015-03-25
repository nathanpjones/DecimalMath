using System;
using System.Linq;
using NUnit.Framework;

namespace MathExtensions.Tests
{
    public class MatrixBaseTests
    {
        private class Matrix2D : MatrixBase<Matrix2D>
        {
            public Matrix2D():base(3)
            {
                
            }

            public decimal[,] GetM()
            {
                return M;
            }

            public void SetElement(int row, int col, decimal value)
            {
                M[row, col] = value;
            }
            public void SetRow(int row, decimal[] values)
            {
                for (var col = 0; col < Size; col++)
                {
                    M[row, col] = values[col];                    
                }
            }

            public decimal[] Multiply(decimal[] m1)
            {
                return Multiply(m1, M);
            }

            public override object Transform(object obj)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void TestConstrucor()
        {
            var testMatrix = new Matrix2D();
            Assert.That(testMatrix.GetM(), Is.EqualTo(new[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } }));
        }

        [Test]
        public void TestReset()
        {
            var testMatrix = new Matrix2D();

            testMatrix.SetRow(0, new decimal[] { 1, 2, 3 });
            testMatrix.SetRow(1, new decimal[] { 11, 12, 13 });
            testMatrix.SetRow(2, new decimal[] { 21, 22, 23 });
            Assert.That(testMatrix.GetM(), Is.EqualTo(new[,] { { 1, 2, 3 }, { 11, 12, 13 }, { 21, 22, 23 } }));

            testMatrix.Reset();
            Assert.That(testMatrix.GetM(), Is.EqualTo(new[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } }));
        }

        [Test]
        public void TestMultiplySameSize()
        {
            var testMatrix1 = new Matrix2D();
            testMatrix1.SetRow(0, new decimal[] { 1, 2, 3 });
            testMatrix1.SetRow(1, new decimal[] { 11, 12, 13 });
            testMatrix1.SetRow(2, new decimal[] { 21, 22, 23 });

            var testMatrix2 = new Matrix2D();
            testMatrix2.SetRow(0, new decimal[] { 31, 32, 33 });
            testMatrix2.SetRow(1, new decimal[] { 34, 35, 36 });
            testMatrix2.SetRow(2, new decimal[] { 37, 38, 39 });

            var testMatrix3 = testMatrix1.Multiply(testMatrix2);

            Assert.That(testMatrix3.GetM(), Is.EqualTo(new[,] { { 210, 216, 222 }, { 1230, 1266, 1302 }, { 2250, 2316, 2382 } }));
        }

        [Test]
        public void TestMultiplyWithColumnMatrix()
        {
            var testMatrix = new Matrix2D();
            testMatrix.SetRow(0, new decimal[] { 1, 2, 3 });
            testMatrix.SetRow(1, new decimal[] { 11, 12, 13 });
            testMatrix.SetRow(2, new decimal[] { 21, 22, 23 });

            var transformed = testMatrix.Multiply(new decimal[] { 67, 45, 33 });

            Assert.That(transformed, Is.EqualTo(new[] { 256, 1706, 3156 }));
        }

        [Test]
        public void TestCopy()
        {
            var testMatrix = new Matrix2D();

            testMatrix.SetRow(0, new decimal[] { 1, 2, 3 });
            testMatrix.SetRow(1, new decimal[] { 11, 12, 13 });
            testMatrix.SetRow(2, new decimal[] { 21, 22, 23 });

            var copyMatrix = testMatrix.Copy();

            Assert.That(!ReferenceEquals(testMatrix, copyMatrix), "Copy matrix is a shallow copy!");

            Assert.That(copyMatrix.GetM(), Is.EqualTo(new[,] { { 1, 2, 3 }, { 11, 12, 13 }, { 21, 22, 23 } }));
        }
    }
}
