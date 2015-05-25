using System;

namespace DecimalMath
{
    /// <summary>
    /// Extension methods for the Decimal data type.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Tests whether or not a given value is within the upper and lower limit, inclusive.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="lowerLimit">The lower limit.</param>
        /// <param name="upperLimit">The upper limit.</param>
        public static bool InRangeIncl(this decimal value, decimal lowerLimit, decimal upperLimit)
        {
            if (upperLimit < lowerLimit)
                throw new Exception("Upper limit is less than lower limit!");

            return (value >= lowerLimit) && (value <= upperLimit);
        }
        /// <summary>
        /// Tests whether or not a given value is within the upper and lower limit, exclusive.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="lowerLimit">The lower limit.</param>
        /// <param name="upperLimit">The upper limit.</param>
        public static bool InRangeExcl(this decimal value, decimal lowerLimit, decimal upperLimit)
        {
            if (upperLimit < lowerLimit)
                throw new Exception("Upper limit is less than lower limit!");

            return (value > lowerLimit) && (value < upperLimit);
        }

        /// <summary>
        /// Rounds a number away from zero to the given number of decimal places.
        /// </summary>
        public static decimal RoundFromZero(this decimal d, int decimals)
        {
            if (decimals < 0) throw new ArgumentOutOfRangeException("decimals", "Decimals must be greater than or equal to 0.");

            var scaleFactor = DecimalEx.PowersOf10[decimals];
            var roundingFactor = d > 0 ? 0.5m : -0.5m;

            return decimal.Truncate(d * scaleFactor + roundingFactor) / scaleFactor;
        }
    }
}
