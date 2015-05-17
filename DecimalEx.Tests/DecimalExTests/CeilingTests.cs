using System;
using System.Collections;
using NUnit.Framework;

namespace DecimalEx.Tests.DecimalExTests
{

    public class CeilingTests
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(1.999m, 0).Returns(2m);
                yield return new TestCaseData(2.2360679774997896964091736687m, 3).Returns(2.237m);
                yield return new TestCaseData(2.2360679774997896964091736687m, 27).Returns(2.236067977499789696409173669m);
                yield return new TestCaseData(-2.2360679774997896964091736687m, 27).Returns(-2.236067977499789696409173668m);
                yield return new TestCaseData(2.2360679774997896964091736687m, 28).Returns(2.2360679774997896964091736687m);
                yield return new TestCaseData(-2.2360679774997896964091736687m, 28).Returns(-2.2360679774997896964091736687m);
            }
        }

        [TestCaseSource("TestCases")]
        public decimal Test(decimal value, int places)
        {
            return DecimalEx.Ceiling(value, places);
        }

        [Test]
        public void TestArgumentBounds()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => DecimalEx.Ceiling(10m, -1));
        }
    }

}