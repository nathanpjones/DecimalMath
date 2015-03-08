using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace MathExtensions
{
    public static partial class MathExt
    {
#if DEBUG
        private static int maxSqrtIterations;
#endif
        /// <summary>
        /// Returns the square root of a specified number using Decimal precision.
        /// Uses an implementation of the "Babylonian Method".
        /// </summary>
        /// <param name="s">A number.</param>
        /// <remarks> See http://en.wikipedia.org/wiki/Methods_of_computing_square_roots#Babylonian_method </remarks>
        public static decimal Sqrt(decimal s)
        {

            if (s < 0)
                throw new ArgumentException("Square root not defined for Decimal data type when less than zero!", "s");
            
            // Prevent divide-by-zero errors below. Dividing either
            // of the numbers below will yield a recurring 0 value
            // for halfS eventually converging on zero.
            if (s == 0 || s == SmallestNonZeroDec) return 0;

            decimal x;
            var halfS = s / 2m;
            var lastX = -1m;
            decimal nextX;

#if DEBUG
            var iterations = 1;
            var estimateIterations = 0;
#endif

            // Begin with an estimate for the square root
            var sForEstimate = s;
            var estimateMultiplier = 1m;
            while (sForEstimate >= 100m)
            {
                estimateMultiplier *= 10m;
                sForEstimate /= 100m;
#if DEBUG
                estimateIterations++;
#endif
            }
#if DEBUG
            estimateIterations++;
#endif
            if (sForEstimate < 10m)
            {
                x = 2 * estimateMultiplier;
            }
            else
            {
                x = 6 * estimateMultiplier;
            }

            while (true)
            {
                nextX = x / 2m + halfS / x;

                // The next check effectively sees if we've ran out of
                // precision for our data type.
                if (nextX == x || nextX == lastX) break;

                lastX = x;
                x = nextX;

#if DEBUG
                iterations++;
#endif
            }

#if DEBUG
            var sqrtIterations = estimateIterations + iterations;
            Debug.WriteLine("Sqrt: {0} Iterations: {1}+{2}={3}", s, estimateIterations, iterations, sqrtIterations);
            if (sqrtIterations > maxSqrtIterations)
            {
                maxSqrtIterations = sqrtIterations;
                Debug.WriteLine("  NEW MAX ITERATIONS");
            }
#endif

            return nextX;

        }

        /// <summary>
        /// Returns a specified number raised to the specified power.
        /// </summary>
        /// <param name="x">A decimal-precision floating-point number that specifies a power.</param>
        /// <param name="y">A decimal-precision floating-point number to be raised to a power.</param>
        public static decimal Pow(decimal x, decimal y)
        {
            decimal result;
            bool reciprocal = false;

            // Handle negative exponents
            if (y < 0)
            {
                reciprocal = true;
                y = Math.Abs(y);
            }

            if (y == 0)
            {
                result = 1;
            }
            else if (y == 1)
            {
                result = x;
            }
            else if (y == Decimal.Truncate(y)) // Integer powers
            {
                // See http://en.wikipedia.org/wiki/Exponentiation_by_squaring
                decimal multiplier;

                result = 1;
                multiplier = x;
                while (true)
                {
                    if ((y % 2m) == 1m)
                    {
                        result *= multiplier;
                        y -= 1;
                    }

                    if (y == 0) break;

                    multiplier *= multiplier;
                    y /= 2;
                }
            }
            else // Fractional power < 1
            {
                // See http://en.wikipedia.org/wiki/Exponent#Real_powers
                result = Exp(y * Log(x));
            }

            if (reciprocal)
            {
                if (result == 0) throw new Exception("Negative power of 0 is undefined!");
                result = 1 / result;
            }

            return result;
        }

        /// <summary>
        /// Returns e raised to the specified power.
        /// </summary>
        /// <param name="d">A number specifying a power.</param>
        public static decimal Exp(decimal d)
        {
            decimal result;
            decimal nextAdd;
            int iteration;
            bool reciprocal;
            decimal t;

            reciprocal = d < 0;
            d = Math.Abs(d);

            t = decimal.Truncate(d);

            if (d == 0)
            {
                result = 1;
            }
            else if (d == 1)
            {
                result = E;
            }
            else if (Math.Abs(d) > 1 && t != d)
            {
                // Split up into integer and fractional
                result = Exp(t) * Exp(d - t);
            }
            else if (d == t)   // Integer power
            {
                // See http://en.wikipedia.org/wiki/Exponentiation_by_squaring
                decimal multiplier;

                result = 1;
                multiplier = E;
                while (d > 0)
                {
                    if ((d % 2m) == 1m)
                    {
                        result *= multiplier;
                        d -= 1;
                    }

                    multiplier *= multiplier;
                    d /= 2;
                }
            }
            else                // Fractional power < 1
            {
                // See http://mathworld.wolfram.com/ExponentialFunction.html
                iteration = 0;
                nextAdd = 0;
                result = 0;

                while (true)
                {
                    if (iteration == 0)
                    {
                        nextAdd = 1;
                    }
                    else
                    {
                        nextAdd *= d / iteration;
                    }

                    if (nextAdd == 0) break;

                    result += nextAdd;

                    iteration += 1;
                }
            }

            // Take reciprocal if this was a negative power
            if (reciprocal) result = 1 / result;

            return result;

        }

        /// <summary>
        /// Returns the natural (base e) logarithm of a specified number.
        /// </summary>
        /// <param name="d">A number whose logarithm is to be found.</param>
        /// <remarks>
        /// I'm still not satisfied with the speed. I tried several different
        /// algorithms that you can find in a historical version of this 
        /// source file. The one I settled on was the best of mediocrity.
        /// </remarks>
        public static decimal Log(Decimal d)
        {
            if (d < 0) throw new ArgumentException("Natural logarithm is a complex number for values less than zero!", "d");
            if (d == 0) throw new OverflowException("Natural logarithm is defined as negative infinitiy at zero which the Decimal data type can't represent!");
            
            if (d == 1) return 0;

            if (d >= 1)
            {
                const decimal ln10 = 2.3025850929940456840179914547m;
                //                   2.30258509299404568401799145468436420760110148862877297603332790096757
                //                   from http://oeis.org/A002392/constant
                var power = 0m;

                var x = d;
                while (x > 1)
                {
                    x /= 10;
                    power += 1;
                }

                return Log(x) + power * ln10;
            }
            
            // See http://en.wikipedia.org/wiki/Natural_logarithm#Numerical_value
            // for more information on this faster-converging series.

            decimal y;
            decimal ySquared;

            var iteration = 0;
            var exponent = 0m;
            var nextAdd = 0m;
            var result = 0m;

            y = (d - 1) / (d + 1);
            ySquared = y * y;

            while (true)
            {
                if (iteration == 0)
                {
                    exponent = 2 * y;
                }
                else
                {
                    exponent = exponent * ySquared;
                }
                
                nextAdd = exponent / (2 * iteration + 1);

                if (nextAdd == 0) break;

                result += nextAdd;

                iteration += 1;
            }

            return result;

        }

        /// <summary>
        /// Returns the factorial of a number n expressed as n!. Factorial is
        /// calculated as follows: n * (n - 1) * (n - 2) * ... * 1
        /// </summary>
        /// <param name="n">An integer.</param>
        public static decimal Factorial(int n)
        {

            decimal ret = 1;

            for (int i = n; i >= 2; i += -1)
            {
                ret *= n;
            }

            return ret;

        }

        /// <summary>
        /// Uses the quadratic formula to factor and solve the equation ax^2 + bx + c = 0
        /// </summary>
        /// <param name="a">The coefficient of x^2.</param>
        /// <param name="b">The coefficient of x.</param>
        /// <param name="c">The constant.</param>
        /// <remarks>See http://www.wikihow.com/Factor-Second-Degree-Polynomials-%28Quadratic-Equations%29</remarks>
        public static decimal[] SolveQuadratic(decimal a, decimal b, decimal c)
		{

			decimal h = default(decimal);
			decimal k = default(decimal);
			decimal sqrtOfBSqMin4AC = default(decimal);


            // Horizontal line is either 0 nowhere or everywhere so no solution.
            if ((a == 0) && (b == 0)) return new decimal[] { };

            if ((a == 0)) {
                // This is actually a linear equation. Quadratic would result in a divide by zero
                // so use separate equation.
                // 0 = b * x + c
                // -c = b * x
                // -c / b = x
                return new decimal[] { -c / b };

            }

            // No solution -- shape does not intersect 0. This means that
			// the endpoints will be the min/max.
            if (Pow(b, 2) - 4 * a * c < -SmallestNonZeroDec) return new decimal[] { };

			// Since we're solving for  ax^2 + bx + c = 0  then we can
			// multiply the coefficients by whatever we want until they
			// are in a range that we can get a square root without 
			// exceeding the precision of a Decimal value. We'll make
			// sure here that at least one number is greater than 1
			// or less than -1.

			while ((-1 < a && a < 1) && (-1 < b && b < 1) && (-1 < c && c < 1)) {
				a *= 10;
				b *= 10;
				c *= 10;

			}

			sqrtOfBSqMin4AC = Pow(b, 2) - 4 * a * c;
			if (sqrtOfBSqMin4AC == -SmallestNonZeroDec)
				sqrtOfBSqMin4AC = 0;
			sqrtOfBSqMin4AC = Sqrt(sqrtOfBSqMin4AC);
			h = (-b + sqrtOfBSqMin4AC) / (2 * a);
			k = (-b - sqrtOfBSqMin4AC) / (2 * a);

			// ax^2 + bx + c = (x - h)(x - k) 
			// (x - h)(x - k) = 0 means h and k are the values for x 
			//   that will make the equation = 0
			if (h == k) {
				return new decimal[] { h };
			} else {
				return new decimal[] {
					h,
					k
				};
			}

		}

        /// <summary>
        /// Returns the floor of a Decimal value at the given number of digits.
        /// </summary>
        /// <param name="value">A decimal value.</param>
        /// <param name="places">An integer representing the maximum number of digits 
        /// after the decimal point to end up with.</param>
        [DebuggerStepThrough()]
        public static decimal Floor(decimal value, int places = 0)
        {
            if (places < 0) throw new ArgumentOutOfRangeException("places", "Places must be greater than or equal to 0.");

            if (places == 0) return decimal.Floor(value);

            // At or beyond precision of decimal data type
            if (places >= 28) return value;

            // TODO: Use powers of 10 array?
            //var factor = 1m;
            //for (var i = 1; i <= places; i++)
            //{
            //    factor *= 10m;
            //}
            var factor = PowersOf10[places];
            
            return decimal.Floor(value * factor) / factor;

        }
        /// <summary>
        /// Returns the ceiling of a Decimal value at the given number of digits.
        /// </summary>
        /// <param name="value">A decimal value.</param>
        /// <param name="places">An integer representing the maximum number of digits 
        /// after the decimal point to end up with.</param>
        [DebuggerStepThrough()]
        public static decimal Ceiling(decimal value, int places = 0)
        {

            decimal factor = default(decimal);

            // At or beyond precision of decimal data type
            if (places >= 28)
                return value;

            factor = 1;
            for (int i = 1; i <= places; i++)
            {
                factor *= 10;
            }

            return decimal.Ceiling(value * factor) / factor;

        }

        /// <summary>
        /// Calculates the greatest common factor of a and b to the highest level of
        /// precision represented by either number.
        /// </summary>
        [DebuggerStepThrough()]
        public static decimal GCF(decimal a, decimal b)
        {

            decimal decAdj = default(decimal);
            decimal r = default(decimal);

            // Convert both a and b to an integer if necessary, multiplying by 10
            decAdj = 1;
            while ((Floor(a) != a) || (Floor(b) != b))
            {
                decAdj *= 10;
                a = a * 10;
                b = b * 10;
            }

            // Run Euclid's algorithm
            do
            {
                if (b == 0)
                    break; // TODO: might not be correct. Was : Exit Do
                r = a % b;
                a = b;
                b = r;
            } while (true);

            // Return the adjusted value of a
            a = a / decAdj;

            return a;

        }

        /// Returns a specified number raised to the specified power.
        /// </summary>
        /// <summary>
        /// Tests whether or not a given value is within the upper and lower limit, inclusive.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="lowerLimit">The lower limit.</param>
        /// <param name="upperLimit">The upper limit.</param>
        public static bool InRangeIncl(decimal value, decimal lowerLimit, decimal upperLimit)
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
        public static bool InRangeExcl(decimal value, decimal lowerLimit, decimal upperLimit)
        {

            if (upperLimit < lowerLimit)
                throw new Exception("Upper limit is less than lower limit!");

            return (value > lowerLimit) && (value < upperLimit);

        }

        /// <summary>
        /// Computes arithmetic-geometric mean which is the convergence of the
        /// series of the arithmetic and geometric means and their mean values.
        /// </summary>
        /// <param name="x">A number.</param>
        /// <param name="y">A number.</param>
        /// <remarks>
        /// See http://en.wikipedia.org/wiki/Arithmetic-geometric_mean
        /// Originally implemented to try to get a fast approximation of the
        /// natural logarithm: http://en.wikipedia.org/wiki/Natural_logarithm#High_precision
        /// But it didn't yield a precise enough answer.
        /// </remarks>
        [DebuggerStepThrough()]
        public static decimal AGMean(decimal x, decimal y)
        {

            decimal a = default(decimal);
            decimal g = default(decimal);


            do
            {
                a = (x + y) / 2;
                g = Sqrt(x * y);

                if (a == g)
                    break; // TODO: might not be correct. Was : Exit Do
                if (g == y && a == x)
                    break; // TODO: might not be correct. Was : Exit Do

                x = a;
                y = g;

            } while (true);

            return a;

        }

        /// <summary>
        /// Returns the maximum value in the Decimal array.
        /// </summary>
        /// <param name="values">Decimal values.</param>
        public static decimal Max(params decimal[] values)
        {

            if (values == null)
                throw new ArgumentException("Array is null!");
            if (values.Length == 0)
                throw new ArgumentException("Array is empty!");

            int highest = 0;

            highest = 0;
            for (int i = 1; i <= values.Length - 1; i++)
            {
                if (values[i] > values[highest])
                    highest = i;
            }

            return values[highest];

        }
        /// <summary>
        /// Returns the minimum value in the Decimal array.
        /// </summary>
        /// <param name="values">Decimal values.</param>
        public static decimal Min(params decimal[] values)
        {

            if (values == null)
                throw new ArgumentException("Array is null!");
            if (values.Length == 0)
                throw new ArgumentException("Array is empty!");

            int lowest = 0;

            lowest = 0;
            for (int i = 1; i <= values.Length - 1; i++)
            {
                if (values[i] < values[lowest])
                    lowest = i;
            }

            return values[lowest];

        }
        /// <summary>
        /// Calculates the average of the supplied numbers.
        /// </summary>
        /// <param name="values">The numbers to average.</param>
        public static decimal Average(params decimal[] values)
        {

            decimal avg;

            try
            {
                avg = 0;
                for (int i = 0; i <= values.Length - 1; i++)
                {
                    avg += values[i];
                }
                avg /= values.Length;
            }
            catch (OverflowException ex)
            {
                avg = 0;
                for (int i = 0; i <= values.Length - 1; i++)
                {
                    avg += values[i] / values.Length;
                }
            }

            return avg;

        }

        #region Decimal Rounding

        // Sign mask for the flags field. A value of zero in this bit indicates a
        // positive Decimal value, and a value of one in this bit indicates a
        // negative Decimal value. 
        private const int SignMask = unchecked((int)0x80000000);

        // Scale mask for the flags field. This byte in the flags field contains
        // the power of 10 to divide the Decimal value by. The scale byte must 
        // contain a value between 0 and 28 inclusive.
        private const int ScaleMask = 0x00FF0000;

        // Number of bits scale is shifted by.
        private const int ScaleShift = 16;

        // The maximum power of 10 that a 32 bit integer can store
        private const Int32 MaxInt32Scale = 9;

        // Fast access for 10^n where n is 0-9
        private static UInt32[] Powers10 = new UInt32[]
                                           {
                                               1,
                                               10,
                                               100,
                                               1000,
                                               10000,
                                               100000,
                                               1000000,
                                               10000000,
                                               100000000,
                                               1000000000
                                           };

        // Does an in-place round the specified number of digits, rounding mid-point values 
        // away from zero 
        private static void InternalRoundFromZero(ref Decimal d, int decimalCount)
        {
            var x = Decimal.GetBits(d);
            var flags = x[3];
            Int32 scale = (flags & ScaleMask) >> ScaleShift;
            Int32 scaleDifference = scale - decimalCount;
            if (scaleDifference <= 0)
            {
                return;
            }
            // Divide the value by 10^scaleDifference
            UInt32 lastRemainder;
            UInt32 lastDivisor;
            do
            {
                Int32 diffChunk = (scaleDifference > MaxInt32Scale) ? MaxInt32Scale : scaleDifference;
                lastDivisor = Powers10[diffChunk];
                Debugger.Break();
                lastRemainder = 0; // remove this line
                //lastRemainder = InternalDivRemUInt32(ref d, lastDivisor);
                scaleDifference -= diffChunk;
            }
            while (scaleDifference > 0);

            // Round away from zero at the mid point 
            if (lastRemainder >= (lastDivisor >> 1))
            {
                Debugger.Break();
                //InternalAddUInt32RawUnchecked(ref d, 1);
            }

            // the scale becomes the desired decimal count
            flags = ((decimalCount << ScaleShift) & ScaleMask) | (flags & SignMask);
        }

        public static decimal RoundFromZero(this decimal d, int decimals)
        {
            Contract.Requires<ArgumentNullException>(decimals >= 0);

            var factor = decimals > 0 ? Pow(10m, decimals - 1) : 1m;

            return (d * factor + .5m) / factor;
        }

        #endregion
    }
}
