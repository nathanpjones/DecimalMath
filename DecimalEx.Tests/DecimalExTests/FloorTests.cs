using System;
using System.Collections;
using NUnit.Framework;

namespace DecimalEx.Tests.DecimalExTests
{

    public class FloorTests
    {
        public static IEnumerable TestCases
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

        [TestCaseSource("TestCases")]
        public decimal Test(decimal value, int places)
        {
            return DecimalEx.Floor(value, places);
        }

        [Test]
        public void TestArgumentBounds()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => DecimalEx.Floor(10m, -1));
        }
    }

}