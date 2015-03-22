using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

namespace MathExtensions.Tests.MathExtTests
{
    class AverageTests
    {
        [Test]
        public void GenericTest()
        {
            Assert.That(MathExt.Average(5, 10, 34, 8), Is.EqualTo(14.25m));
        }

        [Test]
        public void OverflowTest()
        {
            const decimal halfMax = decimal.MaxValue / 2m;
            Assert.That(MathExt.Average(halfMax, halfMax, halfMax), Is.EqualTo(halfMax).Within(1m));
        }
    }
}
