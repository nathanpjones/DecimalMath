using DecimalMath;
using NUnit.Framework;

namespace DecimalExTests.DecimalExTests
{
    /// <summary>
    /// Tests for <see cref="DecimalEx.Remainder"/>.
    /// </summary>
    public class RemainderTests
    {
        public static decimal[][] TestCases =
        {
            new[] { decimal.MaxValue, 1m + DecimalEx.SmallestNonZeroDec, 0.0771837485735662406456049673m, 0m },
            new[] { decimal.MaxValue, 1.5m, 0m, 0m },
            new[] { 12m, 2.5m, 2m, 0m },
            new[] { 12m, 4m, 0m, 0m },
            new[] { 1700000000000000000000000000m, DecimalEx.TwoPi, 4.5962074945229569987647604598m, 0m },
            new[] { -1700000000000000000000000000m, DecimalEx.TwoPi, -4.5962074945229569987647604598m, 0m },
            new[] { -DecimalEx.TwoPi * 2m, DecimalEx.TwoPi, -6.2831853071795864769252867664m, 0m },
            new[] { -DecimalEx.TwoPi * 2m, -DecimalEx.TwoPi, -6.2831853071795864769252867664m, 0m },
            new[] { DecimalEx.TwoPi * 2m, DecimalEx.TwoPi, 6.2831853071795864769252867664m, 0m },
            new[] { DecimalEx.TwoPi * 2m, -DecimalEx.TwoPi, 6.2831853071795864769252867664m, 0m },
        };

        [TestCaseSource("TestCases")]
        public void Test(decimal d1, decimal d2, decimal expected, decimal tolerance)
        {
            Assert.That(DecimalEx.Remainder(d1, d2), Is.EqualTo(expected).Within(tolerance));
        }
    }
}