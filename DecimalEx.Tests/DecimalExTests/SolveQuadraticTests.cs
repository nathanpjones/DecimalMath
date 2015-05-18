using System.Collections;
using DecimalMath;
using NUnit.Framework;

namespace DecimalExTests.DecimalExTests
{

    public class SolveQuadraticTests
    {
        public static IEnumerable TestCases
        {
            get
            {                
                yield return new TestCaseData(1m, 4m, 1m).Returns(new[] { -0.2679491924311227064725536585m, -3.7320508075688772935274463415m });
                yield return new TestCaseData(.001m, .004m, .001m).Returns(new[] { -0.2679491924311227064725536585m, -3.7320508075688772935274463415m });
                yield return new TestCaseData(4m, 78m, 3m).Returns(new[] { -0.0385377002224840022315433965m, -19.461462299777515997768456604m });
                yield return new TestCaseData(2m, 3m, 4m).Returns(new decimal[] {});
                yield return new TestCaseData(0m, 0m, 4m).Returns(new decimal[] {});
                yield return new TestCaseData(0m, 2m, 4m).Returns(new [] { -2m });
                yield return new TestCaseData(1m, 2m, 1m).Returns(new [] { -1m });
                yield return new TestCaseData(-0.0635m, 0.0002m, 0.000456m)
                    .Returns(new[] { -0.0831812135522464037086398875m, 0.0863308198514590021338367379m });
                yield return new TestCaseData(0.0000000000063525m, -0.000000000000021m, -0.000000045625m)
                    .Returns(new[] { 84.74958343011745328203598717m, -84.74627764499348633988722686m });
                yield return new TestCaseData(0.0000000000063525m, -121m, -0.000000045625m)
                    .Returns(new[] { 19047619047619.047619047996113m, -.00000000037706611570247933884m });
                yield return new TestCaseData(314286000m, 314159000m, 195313m)
                    .Returns(new[] { -0.0006220882633818043324833699m, -0.9989738211948823245183085839m });
                yield return new TestCaseData(DecimalEx.SmallestNonZeroDec, .002m, DecimalEx.SmallestNonZeroDec)
                    .Returns(new[] { -.00000000000000000000000005m, -20000000000000000000000000m });
            }
        }

        [TestCaseSource("TestCases")]
        public decimal[] Test(decimal a, decimal b, decimal c)
        {
            return DecimalEx.SolveQuadratic(a, b, c);
        }
    }

}