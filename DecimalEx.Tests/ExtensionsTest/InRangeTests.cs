using System;
using System.Collections;
using NUnit.Framework;

namespace DecimalEx.Tests.ExtensionsTest
{

    public class InRangeTests
    {
        public static IEnumerable InclusiveTestCases
        {
            get
            {
                yield return new TestCaseData(0m, 4m, 1m).Throws(typeof(Exception)).SetName("Argument Test");

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

        public static IEnumerable ExclusiveTestCases
        {
            get
            {
                yield return new TestCaseData(0m, 4m, 1m).Throws(typeof(Exception)).SetName("Argument Test");

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
    }

}