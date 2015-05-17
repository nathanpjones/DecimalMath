using System;
using System.Collections;
using NUnit.Framework;

namespace DecimalEx.Tests.DecimalExTests
{

    public class AGMeanTests
    {
        public const decimal Tolerance = 1m;

        public static decimal[][] TestCases =
        {
                new[] {24m, 6m, 13.4581714817256154207668131569m, Tolerance},
                new[] {3m, 6m, 4.3703730931407206075592971498m, Tolerance},
                new[] {-3m, -6m, -4.3703730931407206075592971498m, Tolerance},
                new[] {256636754m, 372843828m, 312029591.54689409232001318882m, Tolerance},
                new[] {DecimalEx.SmallestNonZeroDec, 372843828m, 6842214.3831477590161m, Tolerance}, // full result would be 6842214.3831477590161357941832 but for loss of precision
        };

        [TestCaseSource("TestCases")]
        public void Test(decimal x, decimal y, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(DecimalEx.AGMean(x, y), Is.EqualTo(expected).Within(tolerance));
        }

        public static decimal[][] SpecialCases =
        {
                new[] {0m, 6m, 0m, 0m},
                new[] {6m, 0m, 0m, 0m},
        };

        [TestCaseSource("SpecialCases")]
        public void TestSpecialCases(decimal x, decimal y, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(DecimalEx.AGMean(x, y), Is.EqualTo(expected).Within(tolerance));
        }

        [Test]
        public void RejectMixedSign()
        {
            Assert.Throws<Exception>(() => DecimalEx.AGMean(-3, 6));
            Assert.Throws<Exception>(() => DecimalEx.AGMean(3, -6));
        }
    }

}