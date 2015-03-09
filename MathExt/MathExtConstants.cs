using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MathExtensions
{
    public static partial class MathExt
    {
        /// <summary> The pi (π) constant. Pi radians is equivalent to 180 degrees. </summary>
        /// <remarks> See http://en.wikipedia.org/wiki/Pi </remarks>
        public const decimal Pi = 3.1415926535897932384626433833m;              // 180 degrees - see http://en.wikipedia.org/wiki/Pi
        /// <summary> π/2 - in radians is equivalent to 90 degrees. </summary> 
        public const decimal PiHalf = 1.5707963267948966192313216916m;          //  90 degrees
        /// <summary> π/4 - in radians is equivalent to 45 degrees. </summary>
        public const decimal PiQuarter = 0.7853981633974483096156608458m;       //  45 degrees
        /// <summary> π/12 - in radians is equivalent to 15 degrees. </summary>
        public const decimal PiTwelfth = 0.2617993877991494365385536153m;       //  15 degrees
        /// <summary> 2π - in radians is equivalent to 360 degrees. </summary>
        public const decimal TwoPi = 6.2831853071795864769252867666m;           // 360 degrees

        /// <summary>
        /// Smallest non-zero decimal value.
        /// </summary>
        public const decimal SmallestNonZeroDec = 0.0000000000000000000000000001m; // aka new decimal(1, 0, 0, false, 28); //1e-28m

        /// <summary>
        /// The e constant, also known as "Euler's number" or "Napier's constant"
        /// </summary>
        /// <remarks>
        /// Full value is 2.718281828459045235360287471352662497757, 
        /// see http://mathworld.wolfram.com/e.html
        /// </remarks>
        public const decimal E = 2.7182818284590452353602874714m;

        // Fast access for 10^n
        private static readonly decimal[] PowersOf10 =
        {
            1m,
            10m,
            100m,
            1000m,
            10000m,
            100000m,
            1000000m,
            10000000m,
            100000000m,
            1000000000m,
            10000000000m,
            100000000000m,
            1000000000000m,
            10000000000000m,
            100000000000000m,
            1000000000000000m,
            10000000000000000m,
            100000000000000000m,
            1000000000000000000m,
            10000000000000000000m,
            100000000000000000000m,
            1000000000000000000000m,
            10000000000000000000000m,
            100000000000000000000000m,
            1000000000000000000000000m,
            10000000000000000000000000m,
            100000000000000000000000000m,
            1000000000000000000000000000m,
            10000000000000000000000000000m,
        };
    }
}
