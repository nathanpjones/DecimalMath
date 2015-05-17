using System;
using System.Linq;
using NUnit.Framework;

namespace DecimalEx.Tests
{
    public class MatrixTests
    {
        [Test]
        public void TestGetIdentity()
        {
            var testMatrix = Matrix.GetIdentityMatrix(3);

            Assert.That(testMatrix,
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
                                       { 2250, 2316, 2382 },
                                   }));

            testMatrix3 = Matrix.Multiply(testMatrix2, testMatrix1);

            Assert.That(testMatrix3,
                        Is.EqualTo(new[,]
                                   {
                                       { 1076, 1172, 1268 },
                                       { 1175, 1280, 1385 },
                                       { 1274, 1388, 1502 },
                                   }));
        }

        [Test]
        public void TestMultiplyDifferentSize()
        {
            var testMatrix1 = new decimal[,]
                              {
                                  { 1, 0, -2 }, 
                                  { 0, 3, -1 }
                              };
            var testMatrix2 = new decimal[,]
                              {
                                  { 0, 3 }, 
                                  { -2, -1 },
                                  { 0, 4 }
                              };

            var testMatrix3 = Matrix.Multiply(testMatrix1, testMatrix2);
            Assert.That(testMatrix3, Is.EqualTo(new[,]
                                                {
                                                    { 0, -5 }, 
                                                    { -6, -7 }
                                                }));

            testMatrix3 = Matrix.Multiply(testMatrix2, testMatrix1);
            Assert.That(testMatrix3,
                        Is.EqualTo(new[,]
                                   {
                                       { 0, 9, -3 },
                                       { -2, -3, 5 },
                                       { 0, 12, -4 }
                                   }));
        }

        [Test]
        public void TestMultiplyWithColumnMatrix()
        {
            var testMatrix = new decimal[,]
                             {
                                 { 1, 2, 3 },
                                 { 11, 12, 13 },
                                 { 21, 22, 23 },
                             };
            var colMatrix = new decimal[,]
                            {
                                { 67 },
                                { 45 },
                                { 33 },
                            };

            var transformed = Matrix.Multiply(testMatrix, colMatrix);

            Assert.That(transformed,
                        Is.EqualTo(new decimal[,]
                                   {
                                       { 256 },
                                       { 1706 },
                                       { 3156 }
                                   }));
        }

        [Test]
        public void TestToColumn()
        {
            var values = new decimal[] { 1, 2, 3 };

            var columnMatrix = Matrix.ToColumn(values);


            Assert.That(columnMatrix,
                        Is.EqualTo(new decimal[,]
                                   {
                                       { 1 },
                                       { 2 },
                                       { 3 }
                                   }));
        }

        [Test]
        public void TestToRow()
        {
            var values = new decimal[] { 1, 2, 3 };

            var rowMatrix = Matrix.ToRow(values);


            Assert.That(rowMatrix,
                        Is.EqualTo(new decimal[,]
                                   {
                                       { 1, 2, 3 },
                                   }));
        }

        [Test]
        public void TestToArray()
        {
            var rowMatrix = new decimal[,]
                                   {
                                       { 1, 2, 3 },
                                   };
            var values = Matrix.RowOrColumnToArray(rowMatrix);
            Assert.That(values, Is.EqualTo(new decimal[] { 1, 2, 3 }));

            var columnMatrix = new decimal[,]
                               {
                                   { 1 },
                                   { 2 },
                                   { 3 }
                               };
            values = Matrix.RowOrColumnToArray(columnMatrix);
            Assert.That(values, Is.EqualTo(new decimal[] { 1, 2, 3 }));
        }
    }
}
