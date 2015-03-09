using NUnit.Framework;

namespace MathExtensions.Tests.MathExtTests
{

    public class LogTests
    {
        public const decimal Tolerance = 10m * MathExt.SmallestNonZeroDec;

        public static decimal[][] TestCases =
        {
            new[] { 15000m, 9.6158054800843471180499789342018m, Tolerance },
            new[] { .15m, -1.89711998488588130203997833922m, Tolerance },
            new[] { 15m, 2.7080502011022100659960045701487m, Tolerance },
            new[] { 1m, 0m, 0m },
        };

        [TestCaseSource("TestCases")]
        public void Test(decimal value, decimal expected, decimal tolerance)
        {
            Assert.That(MathExt.Log(value), Is.EqualTo(expected).Within(tolerance));
        }
    }

}