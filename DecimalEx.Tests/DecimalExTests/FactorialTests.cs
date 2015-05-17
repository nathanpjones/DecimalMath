using System;
using System.Collections;
using System.Diagnostics;
using NUnit.Framework;

namespace DecimalEx.Tests.DecimalExTests
{

    public class FactorialTests
    {
        public const decimal Tolerance = 5m;

        public static decimal[][] TestCases =
        {
                new[] {0m, 1m},
                new[] {1m, 1m},
                new[] {2m, 2m},
                new[] {7m, 5040m},
                new[] {13m, 6227020800m},
        };

        [TestCaseSource("TestCases")]
        public void Test(decimal n, decimal expected)
        {
            Assert.That(DecimalEx.Factorial(n), Is.EqualTo(expected));
        }

        [Test]
        public void RejectNegative()
        {
            Assert.Throws<ArgumentException>(() => DecimalEx.Factorial(-1m));
        }

        [Test]
        public void RejectFractional()
        {
            Assert.Throws<ArgumentException>(() => DecimalEx.Factorial(0.5m));
        }
    }

}