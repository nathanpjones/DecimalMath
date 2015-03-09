using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

namespace MathExtensions.Tests.MathExtTests
{
    class MiscTests
    {
        [Test]
        public void GetDecimalPlacesTest()
        {
            // Test "Normal" Values

            var x = 55.9016994374m;
            Assert.That(MathExt.GetDecimalPlaces(x, false), Is.EqualTo(10));

            x = 42m;
            Assert.That(MathExt.GetDecimalPlaces(x, false), Is.EqualTo(0));

            x = 0m;
            Assert.That(MathExt.GetDecimalPlaces(x, false), Is.EqualTo(0));

            // Test Values With Trailing Zeros

            x = MathExt.SmallestNonZeroDec;
            for (int i = 28; i >= 0; i--)
            {
                Assert.That(MathExt.GetDecimalPlaces(x, false), Is.EqualTo(i));
                x *= 10;
            }

            x = -MathExt.SmallestNonZeroDec;
            for (int i = 28; i >= 0; i--)
            {
                Assert.That(MathExt.GetDecimalPlaces(x, false), Is.EqualTo(i));
                x *= 10;
            }

            x = 0.0100m;
            Assert.That(MathExt.GetDecimalPlaces(x, false), Is.EqualTo(2));

            x = 100.0000m;
            Assert.That(MathExt.GetDecimalPlaces(x, false), Is.EqualTo(0));

            x = 0.0000m;
            Assert.That(MathExt.GetDecimalPlaces(x, false), Is.EqualTo(0));
        }
    }
}
