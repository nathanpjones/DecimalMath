using DecimalMath;
using NUnit.Framework;

namespace DecimalExTests.DecimalExTrigTests
{

    public class SinTests
    {
        public const decimal Tolerance = 10m;

        public static decimal[][] TestCases =
        {
                new[] {0m, 0m, 0m},
                new[] {DecimalEx.Pi, 0m, 0m},
                new[] {DecimalEx.TwoPi, 0m, 0m},
                new[] {DecimalEx.PiHalf, 1m, 0m},
                new[] {DecimalEx.Pi + DecimalEx.PiHalf, -1m, 0m},
                new[] {12m, -0.5365729180004349716653742282424m, Tolerance},
                new[] {2.667m, 0.45697615904786257495867623434033m, Tolerance},
                new[] {-12m, 0.5365729180004349716653742282424m, Tolerance},
                new[] {3.1415926535897932384626433832m, 0m, Tolerance},
        };

        [TestCaseSource("TestCases")]
        public void Test(decimal d, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(DecimalEx.Sin(d), Is.EqualTo(expected).Within(tolerance));
        }
    }

}