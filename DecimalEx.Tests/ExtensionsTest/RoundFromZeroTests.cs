using System;
using System.Collections;
using DecimalMath;
using NUnit.Framework;

namespace DecimalExTests.ExtensionsTest
{

    public class RoundFromZeroTests
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(3.5m, 0).Returns(4m);
                yield return new TestCaseData(2.8m, 0).Returns(3m);
                yield return new TestCaseData(2.5m, 0).Returns(3m);
                yield return new TestCaseData(2.1m, 0).Returns(2m);
                yield return new TestCaseData(-2.1m, 0).Returns(-2m);
                yield return new TestCaseData(-2.5m, 0).Returns(-3m);
                yield return new TestCaseData(-2.8m, 0).Returns(-3m);
                yield return new TestCaseData(-3.5m, 0).Returns(-4m);

                yield return new TestCaseData(.35m, 1).Returns(.4m);
                yield return new TestCaseData(.28m, 1).Returns(.3m);
                yield return new TestCaseData(.25m, 1).Returns(.3m);
                yield return new TestCaseData(.21m, 1).Returns(.2m);
                yield return new TestCaseData(-.21m, 1).Returns(-.2m);
                yield return new TestCaseData(-.25m, 1).Returns(-.3m);
                yield return new TestCaseData(-.28m, 1).Returns(-.3m);
                yield return new TestCaseData(-.35m, 1).Returns(-.4m);
            }
        }

        [TestCaseSource("TestCases")]
        public decimal Test(decimal value, int places)
        {
            return value.RoundFromZero(places);
        }

        [Test]
        public void TestArgumentBounds()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 10m.RoundFromZero(-1));
        }
    }

}