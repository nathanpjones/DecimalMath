using System;
using System.Collections;
using System.Diagnostics;
using NUnit.Framework;

namespace MathExtensions.Tests.MathExtTests
{

    public class NormalizeTests
    {
        public const decimal Tolerance = 10m;

        // Tolerances below are dictated by magnitude of loss of precision
        public static decimal[][] TestCases =
        {
                new[] {0m, 0m, 0m},
                new[] {-MathExt.Pi, MathExt.Pi, 0m},
                new[] {-10001 * MathExt.Pi, MathExt.Pi, 10000m},
                new[] {-MathExt.TwoPi, 0m, 0m},
                new[] {-2*MathExt.TwoPi, MathExt.TwoPi, 2m},
                new[] {5 * MathExt.Pi, MathExt.Pi, 10m},
                new[] {4 * MathExt.Pi + MathExt.PiHalf, MathExt.PiHalf, 10m},
                new[] {522277854577893m, 4.70162077398454137949m, 1000000m},
                new[] {-522277854577893m, 1.5815645331950450974m, 1000000m},
        };

        [TestCaseSource("TestCases")]
        public void Test(decimal d, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(MathExt.NormalizeAngle(d), Is.EqualTo(expected).Within(tolerance));
        }

        // Tolerances below are dictated by magnitude of loss of precision
        public static decimal[][] TestCasesDeg =
        {
                new[] {0m, 0m, 0m},
                new[] {-180m, 180m, 0m},
                new[] {-10001 * 180m, 180m, 0m},
                new[] {-360m, 0m, 0m},
                new[] {-2*360m - 100m * MathExt.SmallestNonZeroDec, 360m, 1m},
                new[] {5 * 180m, 180m, 0m},
                new[] {4 * 180m + 90m, 90m, 0m},
                new[] {522277854577893m, 333m, 0m},
                new[] {-522277854577893m, 27m, 0m},
        };

        [TestCaseSource("TestCasesDeg")]
        public void TestDeg(decimal d, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(MathExt.NormalizeAngleDeg(d), Is.EqualTo(expected).Within(tolerance));
        }
    }

}