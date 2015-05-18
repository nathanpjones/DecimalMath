using DecimalMath;
using NUnit.Framework;

namespace DecimalExTests.DecimalExTests
{
    class AverageTests
    {
        [Test]
        public void GenericTest()
        {
            Assert.That(DecimalEx.Average(5, 10, 34, 8), Is.EqualTo(14.25m));
        }

        [Test]
        public void OverflowTest()
        {
            const decimal halfMax = decimal.MaxValue / 2m;
            Assert.That(DecimalEx.Average(halfMax, halfMax, halfMax), Is.EqualTo(halfMax).Within(1m));
        }
    }
}
