using System;
using System.Collections;
using System.Diagnostics;
using NUnit.Framework;

namespace DecimalEx.Tests.DecimalExTrigTests
{

    public class CosTests
    {
        public const decimal Tolerance = 10m;

        public static decimal[][] SpecialCases =
        {
            new[] { 0m, 1m, 0m },
            new[] { DecimalEx.TwoPi, 1m, 0m },
            new[] { DecimalEx.Pi, -1m, 0m },
            new[] { DecimalEx.PiHalf, 0m, 0m },
            new[] { DecimalEx.Pi + DecimalEx.PiHalf, 0m, 0m },
        };

        [TestCaseSource("SpecialCases")]
        public void TestSpecialCases(decimal x, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(DecimalEx.Cos(x), Is.EqualTo(expected).Within(tolerance));
        }

        // I generated the expected value using x % TwoPi
        public static decimal[][] TestCases =
        {
            new[] { 0.5m, 0.87758256189037271611628158260383m, Tolerance },
            new[] { 0.625m, 0.81096311950521790218953480394108m, Tolerance },
            new[] { 1.5m, 0.07073720166770291008818985143427m, Tolerance },
            new[] { 15877m, 0.820065281433903725830087668850260m, 3 * Tolerance },
            new[] { -4538.5m, -0.4523618664556990608284425320411137m, Tolerance },
            new[] { -37m, 0.7654140519453433564910812927706m, 2 * Tolerance },
            new[] { 0.0000000000000007777777777777m, 1m, Tolerance },
            new[] { 1700000000000000000000000000m, -0.115920289925442305587019103419483m, Tolerance },
            new[] { 39614081257132168796771975168m, 0.28745966621903537726698955261768m, Tolerance },
            new[] { 79228162514264337593543950335m, -0.00766925169057786160588762018743m, Tolerance },
        };

        [TestCaseSource("TestCases")]
        public void Test(decimal x, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(DecimalEx.Cos(x), Is.EqualTo(expected).Within(tolerance));
        }
    }

}