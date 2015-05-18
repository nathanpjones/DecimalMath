using DecimalMath;
using NUnit.Framework;

namespace DecimalExTests.DecimalExTests
{

    public class ExpTests
    {
        public const decimal Tolerance = 5m;

        public static decimal[][] TestCases =
        {
                new[] {3m, 20.085536923187667740928529654582m, Tolerance},
                new[] {2m, 7.389056098930650227230427460575m, Tolerance},
                new[] {1.5m, 4.4816890703380648226020554601193m, Tolerance},
                new[] {1m, 2.7182818284590452353602874713527m, 0m},
                new[] {0m, 1m, 0m},
                new[] {-42m, 0.0000000000000000005749522264m, Tolerance},
                new[] {-66m, 0m, 0m}, // This is not actually 0, but so close that it resolves to 0
        };

        [TestCaseSource("TestCases")]
        public void Test(decimal d, decimal expected, decimal tolerance)
        {
            tolerance = Helper.GetScaledTolerance(expected, (int)tolerance, true);
            Assert.That(DecimalEx.Exp(d), Is.EqualTo(expected).Within(tolerance));
        }
    }

}