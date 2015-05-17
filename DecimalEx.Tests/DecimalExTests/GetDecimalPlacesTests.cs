using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

namespace DecimalEx.Tests.DecimalExTests
{
    class GetDecimalPlacesTests
    {
        public static IEnumerable TestCases
        {
            get
            {
                // Test "Normal" Values
                yield return new TestCaseData(55.9016994374m).Returns(10);
                yield return new TestCaseData(-55.9016994374m).Returns(10);
                yield return new TestCaseData(42m).Returns(0);
                yield return new TestCaseData(0m).Returns(0);

                // Test Values With Trailing Zeros
                yield return new TestCaseData(0.0100m).Returns(2);
                yield return new TestCaseData(100.0000m).Returns(0);
                yield return new TestCaseData(0.0000m).Returns(0);
                yield return new TestCaseData(decimal.Negate(0.0000m)).Returns(0);
            }
        }

        [TestCaseSource("TestCases")]
        public int ExcludeTrailingZeros(decimal dec)
        {
            return DecimalEx.GetDecimalPlaces(dec, false);
        }

        [Test]
        public void CountTrailingZeros()
        {
            var x = DecimalEx.SmallestNonZeroDec;
            for (int i = 28; i >= 0; i--)
            {
                Assert.That(DecimalEx.GetDecimalPlaces(x, false), Is.EqualTo(i));
                x *= 10;
            }

            x = -DecimalEx.SmallestNonZeroDec;
            for (int i = 28; i >= 0; i--)
            {
                Assert.That(DecimalEx.GetDecimalPlaces(x, false), Is.EqualTo(i));
                x *= 10;
            }
        }
    }
}
