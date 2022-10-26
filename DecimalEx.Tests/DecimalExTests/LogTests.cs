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
            new[] { 10000000000000000000000000000m, 28m, 0m },
            new[] { 1000000000000000000000000000.0m, 27m, 0m },
            new[] { 100000000000000000000000000.00m, 26m, 0m },
            new[] { 10000000000000000000000000.000m, 25m, 0m },
            new[] { 1000000000000000000000000.0000m, 24m, 0m },
            new[] { 100000000000000000000000.00000m, 23m, 0m },
            new[] { 10000000000000000000000.000000m, 22m, 0m },
            new[] { 1000000000000000000000.0000000m, 21m, 0m },
            new[] { 100000000000000000000.00000000m, 20m, 0m },
            new[] { 10000000000000000000.000000000m, 19m, 0m },
            new[] { 1000000000000000000.0000000000m, 18m, 0m },
            new[] { 100000000000000000.00000000000m, 17m, 0m },
            new[] { 10000000000000000.000000000000m, 16m, 0m },
            new[] { 1000000000000000.0000000000000m, 15m, 0m },
            new[] { 100000000000000.00000000000000m, 14m, 0m },
            new[] { 10000000000000.000000000000000m, 13m, 0m },
            new[] { 1000000000000.0000000000000000m, 12m, 0m },
            new[] { 100000000000.00000000000000000m, 11m, 0m },
            new[] { 10000000000.000000000000000000m, 10m, 0m },
            new[] { 1000000000.0000000000000000000m, 9m, 0m },
            new[] { 100000000.00000000000000000000m, 8m, 0m },
            new[] { 10000000.000000000000000000000m, 7m, 0m },
            new[] { 1000000.0000000000000000000000m, 6m, 0m },
            new[] { 100000.00000000000000000000000m, 5m, 0m },
            new[] { 10000.000000000000000000000000m, 4m, 0m },
            new[] { 1000.0000000000000000000000000m, 3m, 0m },
            new[] { 100.00000000000000000000000000m, 2m, 0m },
            new[] { 10.000000000000000000000000000m, 1m, 0m },
            new[] { 1.0000000000000000000000000000m, 0m, 0m },
            new[] { 0.1m, -1m, 0m },
            new[] { 0.01m, -2m, 0m },
            new[] { 0.001m, -3m, 0m },
            new[] { 0.0001m, -4m, 0m },
            new[] { 0.00001m, -5m, 0m },
            new[] { 0.000001m, -6m, 0m },
            new[] { 0.0000001m, -7m, 0m },
            new[] { 0.00000001m, -8m, 0m },
            new[] { 0.000000001m, -9m, 0m },
            new[] { 0.0000000001m, -10m, 0m },
            new[] { 0.00000000001m, -11m, 0m },
            new[] { 0.000000000001m, -12m, 0m },
            new[] { 0.0000000000001m, -13m, 0m },
            new[] { 0.00000000000001m, -14m, 0m },
            new[] { 0.000000000000001m, -15m, 0m },
            new[] { 0.0000000000000001m, -16m, 0m },
            new[] { 0.00000000000000001m, -17m, 0m },
            new[] { 0.000000000000000001m, -18m, 0m },
            new[] { 0.0000000000000000001m, -19m, 0m },
            new[] { 0.00000000000000000001m, -20m, 0m },
            new[] { 0.000000000000000000001m, -21m, 0m },
            new[] { 0.0000000000000000000001m, -22m, 0m },
            new[] { 0.00000000000000000000001m, -23m, 0m },
            new[] { 0.000000000000000000000001m, -24m, 0m },
            new[] { 0.0000000000000000000000001m, -25m, 0m },
            new[] { 0.00000000000000000000000001m, -26m, 0m },
            new[] { 0.000000000000000000000000001m, -27m, 0m },
            new[] { 0.0000000000000000000000000001m, -28m, 0m },
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
        
        [Test]
        public void LnThrowsIfArgZero()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => DecimalEx.Ln(0));
        }

        [Test]
        public void LnThrowsIfArgNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => DecimalEx.Ln(-1));
        }

        [Test]
        public void LnTest()
        {
            decimal m;

            m = 1;
            Assert.AreEqual(0, DecimalEx.Ln(m));

            m = 2;
            Assert.AreEqual(Math.Log((double)m), DecimalEx.Ln(m));

            m = 10;
            Assert.AreEqual(Math.Log((double)m), DecimalEx.Ln(m));

            m = DecimalEx.E;
            Assert.AreEqual(1, DecimalEx.Ln(m));

            m = decimal.MaxValue;
            Assert.AreEqual(Math.Log((double)m), DecimalEx.Ln(m));

            m = DecimalEx.SmallestNonZeroDec;
            Assert.AreEqual(Math.Log((double)m), DecimalEx.Ln(m));

            m = 1.23456789m;
            Assert.AreEqual(Math.Log((double)m), DecimalEx.Ln(m));

            m = 9.87654321m;
            Assert.AreEqual(Math.Log((double)m), DecimalEx.Ln(m));

            m = 123456789m;
            Assert.AreEqual(Math.Log((double)m), DecimalEx.Ln(m));

            m = 9876543210m;
            Assert.AreEqual(Math.Log((double)m), DecimalEx.Ln(m));

            m = 0.00000000000000000123456789m;
            Assert.AreEqual(Math.Log((double)m), DecimalEx.Ln(m));

            m = 0.00000000000000000987654321m;
            Assert.AreEqual(Math.Log((double)m), DecimalEx.Ln(m));
        }

        [Test]
        public void Log1Base0Returns0()
        {
            Assert.AreEqual(0, DecimalEx.Log(1, 0));
        }

        [Test]
        public void LogThrowsIfBase1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => DecimalEx.Log(1.234m, 1));
        }
    }
}
