using System;
using System.Collections;
using System.Diagnostics;
using NUnit.Framework;

namespace MathExtensions.Tests.MathExtTests
{

    public class ACosTests
    {
        public const decimal Tolerance = 1m;

        public static decimal[][] SpecialCases =
        {
            new[] { -1m, MathExt.Pi, 0m },
            new[] { 0m, MathExt.PiHalf, 0m },
            new[] { 1m, 0m, 0m },
        };

        [TestCaseSource("SpecialCases")]
        public void TestSpecialCases(decimal d, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(MathExt.ACos(d), Is.EqualTo(expected).Within(tolerance));
        }

        public static decimal[][] TestCases =
        {
            new[] { -1m + MathExt.SmallestNonZeroDec, 3.1415926535897790963270196523m, 1m },
            new[] { -0.00272843m, 1.5735247601801302970459854516m, 18m },
            new[] { 0.00063728m, 1.5701590467517606365472099600m, 14m },
            new[] { 0.5m, 1.0471975511965977461542144611m, 1m },
            new[] { 0.63728m, 0.8798328036755322132581581473m, 5m },
            new[] { 1m - MathExt.SmallestNonZeroDec, 0.0000000000000141421356237310m, 0m },
        };

        [TestCaseSource("TestCases")]
        public void Test(decimal d, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(MathExt.ACos(d), Is.EqualTo(expected).Within(tolerance));
        }
    }

}