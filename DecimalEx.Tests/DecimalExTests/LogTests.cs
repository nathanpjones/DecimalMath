using System;
using DecimalMath;
using NUnit.Framework;

namespace DecimalExTests.DecimalExTests
{

    public class LogTests
    {
        public const decimal Tolerance = 5m;

        #region Log Tests

        public static decimal[][] LogNaturalTestCases =
        {
            new[] { 15000m, 9.6158054800843471180499789342018m, Tolerance },
            new[] { .15m, -1.89711998488588130203997833922m, Tolerance },
            new[] { 15m, 2.7080502011022100659960045701487m, Tolerance },
            new[] { 1m, 0m, 0m },
        };

        [TestCaseSource(nameof(LogNaturalTestCases))]
        public void LogNaturalTest(decimal value, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(DecimalEx.Log(value), Is.EqualTo(expected).Within(tolerance));
        }

        [Test]
        public void LogNaturalRejectNegativeValue()
        {
            Assert.Throws<ArgumentException>(() => DecimalEx.Log(-1));
        }
        [Test]
        public void LogNaturalRejectZeroValue()
        {
            Assert.Throws<OverflowException>(() => DecimalEx.Log(0));
        }

        #endregion

        #region Log With Base Tests

        public static decimal[][] LogBaseTestCases =
        {
            new[] { 15000m, 10m, 4.1760912590556812420812890085306m, Tolerance * 2 },
            new[] { .15m, 938m, -0.27720474871548463595021495017423m, Tolerance * 2 },
            new[] { 15m, 2m, 3.9068905956085185293240583734372m, Tolerance * 2 },
            new[] { 1m, 100m, 0m, 0m },
            new[] { 1m, 1m, 0m, 0m },
        };

        [TestCaseSource(nameof(LogBaseTestCases))]
        public void LogBaseTest(decimal value, decimal newBase, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(DecimalEx.Log(value, newBase), Is.EqualTo(expected).Within(tolerance));
        }

        [Test]
        public void LogBaseRejectBaseOf1()
        {
            Assert.Throws<InvalidOperationException>(() => DecimalEx.Log(1234, 1));
        }
        [Test]
        public void LogBaseRejectNegativeValue()
        {
            Assert.Throws<ArgumentException>(() => DecimalEx.Log(-1, 10));
            Assert.Throws<ArgumentException>(() => DecimalEx.Log(10, -1));
        }
        [Test]
        public void LogBaseRejectZeroValue()
        {
            Assert.Throws<OverflowException>(() => DecimalEx.Log(0, 10));
            Assert.Throws<OverflowException>(() => DecimalEx.Log(10, 0));
        }

        #endregion

        #region Log 10 Tests

        public static decimal[][] Log10TestCases =
        {
            new[] { 15000m, 4.1760912590556812420812890085306m, Tolerance },
            new[] { .15m, -0.82390874094431875791871099146938m, Tolerance },
            new[] { 15m, 1.1760912590556812420812890085306m, Tolerance },
            new[] { 1m, 0m, 0m },
        };

        [TestCaseSource(nameof(Log10TestCases))]
        public void Log10Test(decimal value, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(DecimalEx.Log10(value), Is.EqualTo(expected).Within(tolerance));
        }

        [Test]
        public void Log10RejectNegativeValue()
        {
            Assert.Throws<ArgumentException>(() => DecimalEx.Log(-1));
        }
        [Test]
        public void Log10RejectZeroValue()
        {
            Assert.Throws<OverflowException>(() => DecimalEx.Log(0));
        }

        #endregion

        #region Log 2 Tests

        public static decimal[][] Log2TestCases =
        {
            new[] { 15000m, 13.872674880270605572935016661905m, Tolerance },
            new[] { .15m, -2.7369655941662061664165804855416m, Tolerance },
            new[] { 15m, 3.9068905956085185293240583734372m, Tolerance },
            new[] { 1m, 0m, 0m },
        };

        [TestCaseSource(nameof(Log2TestCases))]
        public void Log2Test(decimal value, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(DecimalEx.Log2(value), Is.EqualTo(expected).Within(tolerance));
        }

        [Test]
        public void Log2RejectNegativeValue()
        {
            Assert.Throws<ArgumentException>(() => DecimalEx.Log(-1));
        }
        [Test]
        public void Log2RejectZeroValue()
        {
            Assert.Throws<OverflowException>(() => DecimalEx.Log(0));
        }

        #endregion
    }

}