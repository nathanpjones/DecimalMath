using System;
using System.Collections;
using System.Diagnostics;
using NUnit.Framework;

namespace DecimalEx.Tests.DecimalExTrigTests
{

    public class ATanTests
    {
        public const decimal Tolerance = 5m;

        public static decimal[][] SpecialCases =
        {
            new[] { -1m, -DecimalEx.PiQuarter, 0m },
            new[] { 0m, 0m, 0m },
            new[] { 1m, DecimalEx.PiQuarter, 0m },
        };

        [TestCaseSource("SpecialCases")]
        public void TestSpecialCases(decimal d, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(DecimalEx.ATan(d), Is.EqualTo(expected).Within(tolerance));
        }

        public static decimal[][] TestCases =
        {
            new[] { 0.5m, 0.46364760900080611621425623146121m, Tolerance },
            new[] { 0.625m, 0.55859931534356243597150821640166m, Tolerance },
            new[] { 1.5m, 0.98279372324732906798571061101467m, Tolerance },
            new[] { 15877m, 1.5707333426040118384856405100905m, Tolerance },
            new[] { -37m, -1.5437758776076318304431463582812m, Tolerance },
            new[] { 0.0000000000000007777777777777m, .0000000000000007777777777777m, Tolerance },
            new[] { 1700000000000000000000000000m, 1.5707963267948966192313216910515m, Tolerance },
            new[] { 39614081257132168796771975168m, 1.5707963267948966192313216916145m, Tolerance },
            new[] { 79228162514264337593543950335m,   1.5707963267948966192313216916271m, Tolerance },
        };

        [TestCaseSource("TestCases")]
        public void Test(decimal d, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(DecimalEx.ATan(d), Is.EqualTo(expected).Within(tolerance));
        }
    }

}