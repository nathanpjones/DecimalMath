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
            { }
            public Matrix2D(decimal[,] values): base(3, values)
            { }

            public decimal[,] GetM()
            {
                return M;
            }

            public void SetElement(int row, int col, decimal value)
            {
                M[row, col] = value;
            }

            public override object Transform(object obj)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void TestConstructor()
        {
            var testMatrix = new Matrix2D();

            Assert.That(testMatrix.GetM(),
                        Is.EqualTo(new[,]
                                   {
                                       { 1, 0, 0 },
                                       { 0, 1, 0 }, 
                                       { 0, 0, 1 }
                                   }));
        }

        [Test]
        public void TestMultiplySameSize()
        {
            var testMatrix1 = new decimal[,]
                                           {
                                               { 1, 2, 3 },
                                               { 11, 12, 13 },
                                               { 21, 22, 23 }
                                           };

            var testMatrix2 = new decimal[,]
                                           {
                                               { 31, 32, 33 },
                                               { 34, 35, 36 },
                                               { 37, 38, 39 }
                                           };

            var testMatrix3 = Matrix.Multiply(testMatrix1, testMatrix2);

            Assert.That(testMatrix3,
                        Is.EqualTo(new[,]
                                   {
                                       { 210, 216, 222 },
                                       { 1230, 1266, 1302 },
                                       { 2250, 2316, 2382 }
                                   }));
        }

        [Test]
        public void TestMultiplyDifferentSize()
        {
            var testMatrix1 = new decimal[,] { { 1, 0, -2 }, { 0, 3, -1 } };
            var testMatrix2 = new decimal[,] { { 0, 3 }, { -2, -1 }, { 0, 4 } };

            var testMatrix3 = Matrix.Multiply(testMatrix1, testMatrix2);
            Assert.That(testMatrix3, Is.EqualTo(new[,] { { 0, -5 }, { -6, -7 } }));

            testMatrix3 = Matrix.Multiply(testMatrix2, testMatrix1);
            Assert.That(testMatrix3, Is.EqualTo(new[,] { { 0, 9, -3 }, { -2, -3, 5 }, { 0, 12, -4 } }));
        }

        [Test]
        public void TestMultiplyWithColumnMatrix()
        {
            var testMatrix = new Matrix2D();
            testMatrix.SetRow(0, new decimal[] { 1, 2, 3 });
            testMatrix.SetRow(1, new decimal[] { 11, 12, 13 });
            testMatrix.SetRow(2, new decimal[] { 21, 22, 23 });

            var transformed = testMatrix.Transform(new decimal[] { 67, 45, 33 });

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
