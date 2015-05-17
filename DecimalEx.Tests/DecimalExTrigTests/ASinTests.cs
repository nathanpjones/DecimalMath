using System;
using System.Collections;
using System.Diagnostics;
using NUnit.Framework;

namespace DecimalEx.Tests.DecimalExTrigTests
{

    public class ASinTests
    {
        public static decimal[][] SpecialCases =
        {
            new[] { -1m, -DecimalEx.PiHalf, 0m },
            new[] { 0m, 0m, 0m },
            new[] { 1m, DecimalEx.PiHalf, 0m },
        };

        [TestCaseSource("SpecialCases")]
        public void TestSpecialCases(decimal d, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(DecimalEx.ASin(d), Is.EqualTo(expected).Within(tolerance));
        }

        public static decimal[][] TestCases =
        {
            new[] { -1m + DecimalEx.SmallestNonZeroDec, -1.5707963267948824770956979607m, 11m },
            new[] { -0.00272843m, -0.0027284333852336778146637600m, 1m },
            new[] { 0.00063728m, 0.0006372800431359826841117316m, 2m },
            new[] { 0.5m, 0.5235987755982988730771072305m, 1m },
            new[] { 0.63728m, 0.6909635231193644059731635443m, 3m },
            new[] { 1m - DecimalEx.SmallestNonZeroDec, 1.5707963267948824770956979607m, 11m },
        };

        [TestCaseSource("TestCases")]
        public void Test(decimal d, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(DecimalEx.ASin(d), Is.EqualTo(expected).Within(tolerance));
        }
    }

}