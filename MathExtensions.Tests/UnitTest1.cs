using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathExtensions.Tests
{
    [TestFixture]
    public class UnitTest1
    {
        // Decimals aren't real CLR types that we can pass to TestCases
        // http://stackoverflow.com/a/25116395/856595

        #region Sqrt Tests

        public static IEnumerable SqrtTestCases
        {
            get
            {
                yield return new TestCaseData(-1m).SetName("ErrorOnNegativeValue").Throws(typeof(ArgumentException));
                yield return new TestCaseData(MathExt.SmallestNonZeroDec).Returns(0m);
                yield return new TestCaseData(0m).Returns(0m);
                yield return new TestCaseData(1m).Returns(1m);
                yield return new TestCaseData(9m).Returns(3m);
                yield return new TestCaseData(1777m).Returns(42.15447781671598408129937593716m);
                yield return new TestCaseData(982451653m).Returns(31344.084816756095550750189105763m);
                yield return new TestCaseData(decimal.MaxValue).Returns(281474976710655.99999999999999822m);
            }
        }
        [TestCaseSource("SqrtTestCases")]
        public decimal SqrtTest(decimal s)
        {
            return MathExt.Sqrt(s);
        }

        /// <summary>
        /// This test is just to exercise the algorithm so ignore for automatic tests.
        /// </summary>
        [Test]
        [Ignore]
        public void SqrtTestRange()
        {
            var reset = new AutoResetEvent(false);
            var step = 1m;
            var i = 0m;
            while (true)
            {
                Task.Factory.StartNew(() =>
                                      {
                                          var result = MathExt.Sqrt(i);
                                          reset.Set();
                                      });
                Assert.IsTrue(reset.WaitOne(30000));

                step *= 1.01m;
                try { i += step; }
                catch (OverflowException)
                {
                    if (i == Decimal.MaxValue) break;
                    i = decimal.MaxValue;
                }
            }
        }

        #endregion

        #region Pow Tests

        public static IEnumerable PowTestCases
        {
            get
            {
                yield return new TestCaseData(1m, 0m).Returns(1m);
                yield return new TestCaseData(99m, 1m).Returns(99m);
                yield return new TestCaseData(2m, 8m).Returns(256m);
                yield return new TestCaseData(5m, 40m).Returns(9094947017729282379150390625m);
                yield return new TestCaseData(5m, .5m).Returns(2.2360679774997896964091736687m + 2 * MathExt.SmallestNonZeroDec);
            }
        }

        [TestCaseSource("PowTestCases")]
        public decimal PowTest(decimal x, decimal y)
        {
            return MathExt.Pow(x, y);
        }

        #endregion

        #region Floor Tests

        public static IEnumerable FloorTestCases
        {
            get
            {
                yield return new TestCaseData(1.999m, 0).Returns(1m);
                yield return new TestCaseData(2.2360679774997896964091736687m, 3).Returns(2.236m);
                yield return new TestCaseData(2.2360679774997896964091736687m, 27).Returns(2.236067977499789696409173668m);
                yield return new TestCaseData(-2.2360679774997896964091736687m, 27).Returns(-2.236067977499789696409173669m);
                yield return new TestCaseData(2.2360679774997896964091736687m, 28).Returns(2.2360679774997896964091736687m);
                yield return new TestCaseData(-2.2360679774997896964091736687m, 28).Returns(-2.2360679774997896964091736687m);
            }
        }

        [TestCaseSource("FloorTestCases")]
        public decimal FloorTest(decimal value, int places)
        {
            return MathExt.Floor(value, places);
        }

        #endregion

        #region Log Tests

        public const decimal LogTolerance = 10m * MathExt.SmallestNonZeroDec;

        public static decimal[][] LogTestValues =
        {
            new[] { 15000m, 9.6158054800843471180499789342018m, LogTolerance },
            new[] { .15m, -1.89711998488588130203997833922m, LogTolerance },
            new[] { 15m, 2.7080502011022100659960045701487m, LogTolerance },
            new[] { 1m, 0m, 0m},
        };
        
        [TestCaseSource("LogTestValues")]
        public void LogTest(decimal value, decimal expected, decimal tolerance)
        {
            Assert.That(MathExt.Log(value), Is.EqualTo(expected).Within(tolerance));
        }
        #endregion

    }
}
