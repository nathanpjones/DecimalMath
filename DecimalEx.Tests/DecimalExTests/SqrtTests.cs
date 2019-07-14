using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DecimalMath;
using NUnit.Framework;

namespace DecimalExTests.DecimalExTests
{
    /// <summary>
    /// Tests for <see cref="DecimalEx.Sqrt"/>.
    /// </summary>
    public class SqrtTests
    {
        public static decimal[][] TestCases =
        {
            new[] { DecimalEx.SmallestNonZeroDec, 0m },
            new[] { 0m, 0m },
            new[] { 1m, 1m },
            new[] { 9m, 3m },
            new[] { 1777m, 42.15447781671598408129937593716m },
            new[] { 982451653m, 31344.084816756095550750189105763m },
            new[] { 775351687m, 27845.137582709121891734259812835m },
            new[] { 8137454422m, 90207.840135988180082281179096568m },
            new[] { 4294967296m, 65536m },
            new[] { 2199023255551m, 1482910.4003785933391041923650903m },
            new[] { 2199023255552m, 1482910.4003789305138922795556769m },
            new[] { 2199023255553m, 1482910.4003792676886803666695989m },
            new[] { decimal.MaxValue, 281474976710655.99999999999999822m },
        };

        [TestCaseSource(nameof(TestCases))]
        public void Test(decimal s, decimal expected)
        {
            Assert.That(DecimalEx.Sqrt(s), Is.EqualTo(expected).Within(0m));
        }

        [Test]
        public void RejectNegativeValue()
        {
            Assert.Throws<ArgumentException>(() => DecimalEx.Sqrt(-1));
        }

        /// <summary>
        /// This test is just to exercise the algorithm so ignore for automatic tests.
        /// </summary>
        [Test]
        [Ignore("Exercise algorithm.")]
        public void TestRange()
        {
            var reset = new AutoResetEvent(false);
            var step = 1m;
            var i = 0m;
            while (true)
            {
                Task.Factory.StartNew(() =>
                                      {
                                          Debug.WriteLine("Sqrt({0})={1}", i, DecimalEx.Sqrt(i));
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
    }

}