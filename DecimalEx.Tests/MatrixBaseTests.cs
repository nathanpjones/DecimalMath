using System;
using System.Linq;
using NUnit.Framework;

namespace DecimalEx.Tests
{
    public class MatrixBaseTests
    {
        private class MatrixTestClass : MatrixBase<MatrixTestClass>
        {
            public MatrixTestClass():base(3)
            { }
            public MatrixTestClass(decimal[,] values): base(3, values)
            { }

            public decimal[,] GetM()
            {
                return M;
            }

            public override object Transform(object obj)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void TestConstructor()
        {
            var testMatrix = new MatrixTestClass();

            // Make sure we've initialized to the identity matrix
            Assert.That(testMatrix.GetM(),
                        Is.EqualTo(new[,]
                                   {
                                       { 1, 0, 0 },
                                       { 0, 1, 0 }, 
                                       { 0, 0, 1 }
                                   }));
        }

        [Test]
        public void TestConstructorWithValues()
        {
            var testMatrix = new MatrixTestClass(new decimal[,]
                                          {
                                              { 11, 12, 13 },
                                              { 21, 22, 23 },
                                              { 31, 32, 33 }
                                          });

            Assert.That(testMatrix.GetM(),
                        Is.EqualTo(new[,]
                                   {
                                       { 11, 12, 13 },
                                       { 21, 22, 23 },
                                       { 31, 32, 33 }
                                   }));
        }

        [Test]
        public void TestSetRow()
        {
            var testMatrix = new MatrixTestClass();

            testMatrix.SetRow(1, new decimal[] { 4, 5, 6 });

            Assert.That(testMatrix.GetM(),
                        Is.EqualTo(new[,]
                                   {
                                       { 1, 0, 0 },
                                       { 4, 5, 6 }, 
                                       { 0, 0, 1 }
                                   }));
        }

        [Test]
        public void TestMultiply()
        {
            var testMatrix1 = new MatrixTestClass(new decimal[,]
                                           {
                                               { 1, 2, 3 },
                                               { 11, 12, 13 },
                                               { 21, 22, 23 }
                                           });

            var testMatrix2 = new MatrixTestClass(new decimal[,]
                                           {
                                               { 31, 32, 33 },
                                               { 34, 35, 36 },
                                               { 37, 38, 39 }
                                           });

            var testMatrix3 = testMatrix1.Multiply(testMatrix2);

            Assert.That(testMatrix3.GetM(),
                        Is.EqualTo(new[,]
                                   {
                                       { 210, 216, 222 },
                                       { 1230, 1266, 1302 },
                                       { 2250, 2316, 2382 }
                                   }));
        }

        [Test]
        public void TestTransform()
        {
            var testMatrix = new MatrixTestClass(new decimal[,]
                                           {
                                               { 1, 2, 3 },
                                               { 11, 12, 13 },
                                               { 21, 22, 23 }
                                           });

            var transformed = testMatrix.Transform(new decimal[] { 67, 45, 33 });

            Assert.That(transformed, Is.EqualTo(new[] { 256, 1706, 3156 }));
        }

        [Test]
        public void TestCopy()
        {
            var testMatrix = new MatrixTestClass(new decimal[,]
                                           {
                                               { 1, 2, 3 },
                                               { 11, 12, 13 },
                                               { 21, 22, 23 }
                                           });

            var copyMatrix = testMatrix.Copy();

            Assert.That(!ReferenceEquals(testMatrix, copyMatrix), "Copy matrix is a shallow copy!");

            Assert.That(copyMatrix.GetM(),
                        Is.EqualTo(new[,]
                                   {
                                       { 1, 2, 3 },
                                       { 11, 12, 13 },
                                       { 21, 22, 23 }
                                   }));
        }
    }
}
