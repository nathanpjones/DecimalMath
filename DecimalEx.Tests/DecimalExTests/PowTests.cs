using System;
using System.Diagnostics;
using DecimalMath;
using NUnit.Framework;

namespace DecimalExTests.DecimalExTests
{

    public class PowTests
    {
        public const decimal FractionalTolerance = 5m;

        public static decimal[][] TestCases =
        {
            new[] {1m, 0m, 1m, 0m},
            new[] {2m, 8m, 256m, 0m},
            new[] {3m, 5m, 243m, 0m},
            new[] {5m, 40m, 9094947017729282379150390625m, 0m},
            new[] {99m, 1m, 99m, 0m},
                
            // Fractional powers are going to be off a bit
            new[] {5m, .5m, 2.2360679774997896964091736687m, FractionalTolerance},
            new[] {5m, 2.5m, 55.901699437494742410229341718m, FractionalTolerance},
            new[] {5m, 10.5m, 21836601.342771383753995836609m, FractionalTolerance},
            new[] {5m, 15.5m, 68239379196.160574231236989402m, FractionalTolerance},
            new[] {5m, 20.5m, 213248059988001.79447261559188m, FractionalTolerance},
            new[] {5m, 40.5m, 20336919783401660392056998432m, FractionalTolerance},
        };

        public static decimal[][] SqrTestCases =
        {
            new[] {1m, 1m},
            new[] {8m, 64m},
            new[] {3m, 9m},
        };

        [TestCaseSource("TestCases")]
        public void Test(decimal x, decimal y, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Debug.WriteLine("Pow({0}, {1}) = {2} within {3} (is {4})", x, y, DecimalEx.Pow(x, y), tolerance, DecimalEx.Pow(x, y) - expected);
            Assert.That(DecimalEx.Pow(x, y), Is.EqualTo(expected).Within(tolerance));
        }

        [Test]
        public void NegativeExponentOfZero()
        {
            Assert.Throws<OverflowException>(() => DecimalEx.Pow(0, -1));
        }

        [TestCaseSource("SqrTestCases")]
        public void SqrSameAsPower2(decimal x, decimal expected)
        {
            Assert.That(DecimalEx.Pow(x, 2), Is.EqualTo(DecimalEx.Sqr(x)));
            Assert.That(DecimalEx.Pow(x, 2), Is.EqualTo(expected));
        }
    }
}