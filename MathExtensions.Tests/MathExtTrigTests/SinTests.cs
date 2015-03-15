using System;
using System.Collections;
using System.Diagnostics;
using NUnit.Framework;

namespace MathExtensions.Tests.MathExtTests
{

    public class SinTests
    {
        public const decimal Tolerance = 10m;

        public static decimal[][] TestCases =
        {
                new[] {0m, 0m, 0m},
                new[] {MathExt.Pi, 0m, 0m},
                new[] {MathExt.TwoPi, 0m, 0m},
                new[] {MathExt.PiHalf, 1m, 0m},
                new[] {MathExt.Pi + MathExt.PiHalf, -1m, 0m},
                new[] {12m, -0.5365729180004349716653742282424m, Tolerance},
                new[] {2.667m, 0.45697615904786257495867623434033m, Tolerance},
                new[] {-12m, 0.5365729180004349716653742282424m, Tolerance},
                new[] {3.1415926535897932384626433832m, 0m, Tolerance},
        };

        [TestCaseSource("TestCases")]
        public void Test(decimal d, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(MathExt.Sin(d), Is.EqualTo(expected).Within(tolerance));
        }
    }

}