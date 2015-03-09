using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExtensions.Tests
{
    static class Helper
    {
        /// <summary>
        /// Scales a tolerance to match the precision of the expected value.
        /// </summary>
        public static decimal GetScaledTolerance(decimal expected, int tolerance, bool countTrailingZeros)
        {
            decimal toleranceAtScale = tolerance;
            var precision = MathExt.GetDecimalPlaces(expected, countTrailingZeros);
            for (var i = 0; i < precision; i++) toleranceAtScale /= 10m;
            return toleranceAtScale;
        }
    }
}
