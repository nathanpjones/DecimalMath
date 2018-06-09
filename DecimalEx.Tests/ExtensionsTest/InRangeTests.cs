using System;
using System.Collections;
using DecimalMath;
using NUnit.Framework;

namespace DecimalExTests.ExtensionsTest
{
    public class InRangeTests
    {
        public static IEnumerable InclusiveTestCases
        {
            get
            {
                yield return new TestCaseData(3.5m, 1m, 4m).Returns(true);
                yield return new TestCaseData(1m, 1m, 4m).Returns(true);
                yield return new TestCaseData(4m, 1m, 4m).Returns(true);
                yield return new TestCaseData(0m, 1m, 4m).Returns(false);
                yield return new TestCaseData(4.1m, 1m, 4m).Returns(false);
            }
        }

        [TestCaseSource("InclusiveTestCases")]
        public bool InclusiveTests(decimal value, decimal lowerLimit, decimal upperLimit)
        {
            return value.InRangeIncl(lowerLimit, upperLimit);
        }

        public static IEnumerable InclusiveTestCasesWithException
        {
            get
            {
                yield return new TestCaseData(0m, 4m, 1m);
            }
        }

        [TestCaseSource("InclusiveTestCasesWithException")]
        public void InclusiveTestsWithException(decimal value, decimal lowerLimit, decimal upperLimit)
        {
            Assert.That(() => value.InRangeIncl(lowerLimit, upperLimit), Throws.Exception);
        }

        public static IEnumerable ExclusiveTestCases
        {
            get
            {
                yield return new TestCaseData(3.5m, 1m, 4m).Returns(true);
                yield return new TestCaseData(1m, 1m, 4m).Returns(false);
                yield return new TestCaseData(4m, 1m, 4m).Returns(false);
                yield return new TestCaseData(0m, 1m, 4m).Returns(false);
                yield return new TestCaseData(4.1m, 1m, 4m).Returns(false);
            }
        }

        [TestCaseSource("ExclusiveTestCases")]
        public bool ExclusiveTests(decimal value, decimal lowerLimit, decimal upperLimit)
        {
            return value.InRangeExcl(lowerLimit, upperLimit);
        }

        public static IEnumerable ExclusiveTestCasesWithException
        {
            get
            {
                yield return new TestCaseData(0m, 4m, 1m);
            }
        }

        [TestCaseSource("ExclusiveTestCasesWithException")]
        public void ExclusiveTestsWithException(decimal value, decimal lowerLimit, decimal upperLimit)
        {
            Assert.That(() => value.InRangeExcl(lowerLimit, upperLimit), Throws.Exception);
        }
    }

}