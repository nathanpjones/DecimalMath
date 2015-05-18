using DecimalMath;
using NUnit.Framework;

namespace DecimalExTests.DecimalExTrigTests
{

    public class ATan2Tests
    {
        public const decimal Tolerance = 5m;

        public static decimal[][] SpecialCases =
        {
            new[] { 0m, 0m, 0m, 0m },
            new[] { 1m, 0m, DecimalEx.PiHalf, 0m },
            new[] { -1m, 0m, -DecimalEx.PiHalf, 0m },
            new[] { 0m, 1m, 0, 0m },
            new[] { 0m, -1m, DecimalEx.Pi, 0m },
        };

        [TestCaseSource("SpecialCases")]
        public void TestSpecialCases(decimal y, decimal x, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(DecimalEx.ATan2(y, x), Is.EqualTo(expected).Within(tolerance));
        }

        public static decimal[][] TestCases =
        {
            new[] { 1m, 1m, DecimalEx.PiQuarter, Tolerance },
            new[] { 1m, -1m, DecimalEx.PiHalf + DecimalEx.PiQuarter, Tolerance },
            new[] { -1m, -1m, -(DecimalEx.PiHalf + DecimalEx.PiQuarter), Tolerance },
            new[] { -1m, 1m, -DecimalEx.PiQuarter, Tolerance },

            new[] { 2m, .5m, 1.3258176636680324650592392104285m, Tolerance },
            new[] { 2m, -.5m, 1.815774989921760773403404172851m, Tolerance },
            new[] { -2m, -.5m, -1.815774989921760773403404172851m, Tolerance },
            new[] { -2m, .5m, -1.3258176636680324650592392104285m, Tolerance },
        };

        [TestCaseSource("TestCases")]
        public void Test(decimal y, decimal x, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(DecimalEx.ATan2(y, x), Is.EqualTo(expected).Within(tolerance));
        }
    }

}