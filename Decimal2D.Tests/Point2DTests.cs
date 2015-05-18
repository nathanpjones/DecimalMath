using System;
using DecimalMath;
using NUnit.Framework;

namespace Decimal2DTests
{
    public class Point2DTests
    {
        [TestCase(1, 3)]
        public void TestConstructor(int x, int y)
        {
            var p = new Point2D(x, y);
            Assert.AreEqual(p.X, x);
            Assert.AreEqual(p.Y, y);
        }

        [TestCase(0, 3, true)]
        [TestCase(1, 0, true)]
        [TestCase(1, 3, false)]
        public void TestOnAnAxis(int x, int y, bool isOnAnAxis)
        {
            var p = new Point2D(x, y);
            Assert.That(p.OnAnAxis, Is.EqualTo(isOnAnAxis));
        }

        [TestCase(1, 3, 1)]
        [TestCase(-1, 3, 2)]
        [TestCase(-1, -3, 3)]
        [TestCase(1, -3, 4)]
        public void TestQuadrant(int x, int y, int quadrant)
        {
            var p = new Point2D(x, y);
            Assert.That(p.Quadrant, Is.EqualTo(quadrant));
        }
        [TestCase(0, 3)]
        [TestCase(1, 0)]
        public void TestQuadrantOnAxis(int x, int y)
        {
            var p = new Point2D(x, y);
            Assert.Throws<Exception>(() => {var q = p.Quadrant;});
        }

        [TestCase("0", "3", "0", "3", "0")]
        [TestCase("0", "0", "0", "1", "1")]
        [TestCase("0", "0", "1", "0", "1")]
        public void TestDistanceTo(string x1, string y1, string x2, string y2, string distance)
        {
            var p1 = new Point2D(Convert.ToDecimal(x1), Convert.ToDecimal(y1));
            var p2 = new Point2D(Convert.ToDecimal(x2), Convert.ToDecimal(y2));
            Assert.AreEqual(p1.DistanceTo(p2), Convert.ToDecimal(distance));
        }
    }
}
