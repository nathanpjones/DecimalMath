using System;
using System.Collections;
using System.Diagnostics;
using NUnit.Framework;

namespace MathExtensions.Tests.MathExtTests
{

    public class CosTests
    {
        public const decimal Tolerance = 5m;

        public static decimal[][] SpecialCases =
        {
            new[] { 0m, 1m, 0m },
            new[] { MathExt.TwoPi, 1m, 0m },
            new[] { MathExt.Pi, -1m, 0m },
            new[] { MathExt.PiHalf, 0m, 0m },
            new[] { MathExt.Pi + MathExt.PiHalf, 0m, 0m },
        };

        [TestCaseSource("SpecialCases")]
        public void TestSpecialCases(decimal x, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(MathExt.Cos(x), Is.EqualTo(expected).Within(tolerance));
        }

        public static decimal[][] TestCases =
        {
            //new[] { 0.5m, 0.87758256189037271611628158260383m, Tolerance },
            //new[] { 0.625m, 0.81096311950521790218953480394108m, Tolerance },
            //new[] { 1.5m, 0.07073720166770291008818985143427m, Tolerance },
            //new[] { 15877m, 0.82006528143390372583008772810964m, Tolerance },
            new[] { -37m, 0.76541405194534335649108129290251m, Tolerance },
            //new[] { 0.0000000000000007777777777777m, 1m, Tolerance },
            //new[] { 1700000000000000000000000000m, 0.17364817766693034885171662676931m, Tolerance },
            //new[] { 39614081257132168796771975168m, -0.3746065934159120354149637745012m, Tolerance },
            //new[] { 79228162514264337593543950335m, -0.70710678118654752440084436210485m, Tolerance },
        };

        [TestCaseSource("TestCases")]
        public void Test(decimal x, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(MathExt.Cos(x), Is.EqualTo(expected).Within(tolerance));
        }
    }

}