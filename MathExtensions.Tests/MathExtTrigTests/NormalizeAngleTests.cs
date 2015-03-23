using System;
using System.Collections;
using System.Diagnostics;
using NUnit.Framework;

namespace MathExtensions.Tests.MathExtTests
{

    public class NormalizeTests
    {
        public static decimal[][] TestCases =
        {
                new[] {0m, 0m},
                new[] {-MathExt.Pi, MathExt.Pi},
                new[] {-31419.068128551522177864896476m, 3.1415926535897932384626437666m}, // equivalent to -1001π
                new[] {-MathExt.TwoPi, 0m},
                new[] {-2*MathExt.TwoPi, 2*MathExt.SmallestNonZeroDec},
                new[] {15.707963267948966192313216916m, 3.1415926535897932384626433828m},    // equivalent to 5π
                new[] {4 * MathExt.Pi + MathExt.PiHalf, 1.5707963267948966192313216918m},
                new[] {522277854577893m, 4.7016207739845413794891781334m},
                new[] {-522277854577893m, 1.5815645331950450974361086332m},
        };

        [TestCaseSource("TestCases")]
        public void Test(decimal d, decimal expected)
        {
            Assert.That(MathExt.NormalizeAngle(d), Is.EqualTo(expected));
        }

        public static decimal[][] TestCasesDeg =
        {
                new[] {0m, 0m},
                new[] {-180m, 180m},
                new[] {-10001 * 180m, 180m},
                new[] {-360m, 0m},
                new[] {-2*360m - 100m * MathExt.SmallestNonZeroDec, 360m - 100m * MathExt.SmallestNonZeroDec},
                new[] {5 * 180m, 180m},
                new[] {4 * 180m + 90m, 90m},
                new[] {522277854577893m, 333m},
                new[] {-522277854577893m, 27m},
        };

        [TestCaseSource("TestCasesDeg")]
        public void TestDeg(decimal d, decimal expected)
        {
            Assert.That(MathExt.NormalizeAngleDeg(d), Is.EqualTo(expected));
        }
    }

}