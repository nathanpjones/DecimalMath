﻿using System;
using System.Linq;

namespace DecimalMath
{
    /// <summary>
    /// Contains mathematical operations performed in Decimal precision.
    /// </summary>
    public static partial class DecimalEx
    {
        /// <summary>
        /// Returns the square root of a given number. 
        /// </summary>
        /// <param name="s">A non-negative number.</param>
        /// <remarks> 
        /// Uses an implementation of the "Babylonian Method".
        /// See http://en.wikipedia.org/wiki/Methods_of_computing_square_roots#Babylonian_method 
        /// </remarks>
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

            // Begin with an estimate for the square root.
            // Use hardware to get us there quickly.
            x = (decimal)Math.Sqrt(decimal.ToDouble(s));

            while (true)
            {
                nextX = x / 2m + halfS / x;

                // The next check effectively sees if we've ran out of
                // precision for our data type.
                if (nextX == x || nextX == lastX) break;

                lastX = x;
                x = nextX;
            }

            return nextX;
        }

        /// <summary>
        /// Returns a specified number raised to the power of 2 - squared.
        /// </summary>
        /// <param name="x">A number to be squared.</param>
        public static decimal Sqr(decimal x)
        {
            return Pow(x, 2m);
        }

        /// <summary>
        /// Returns a specified number raised to the specified power.
        /// </summary>
        /// <param name="x">A number to be raised to a power.</param>
        /// <param name="y">A number that specifies a power.</param>
        public static decimal Pow(decimal x, decimal y)
        {
            decimal result;
            var isNegativeExponent = false;
            
            // Handle negative exponents
            if (y < 0)
            {
                isNegativeExponent = true;
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
            else
            {
                var t = decimal.Truncate(y);

                if (y == t) // Integer powers
                {
                    result = ExpBySquaring(x, y);
                }
                else // Fractional power < 1
                {
                    // See http://en.wikipedia.org/wiki/Exponent#Real_powers
                    // The next line is an optimization of Exp(y * Log(x)) for better precision
                    result = ExpBySquaring(x, t) * Exp((y - t) * Log(x));
                }
            }

            if (isNegativeExponent)
            {
                // Note, for IEEE floats this would be Infinity and not an exception...
                if (result == 0) throw new OverflowException("Negative power of 0 is undefined!");

                result = 1 / result;
            }

            return result;
        }

        /// <summary>
        /// Raises one number to an integral power.
        /// </summary>
        /// <remarks>
        /// See http://en.wikipedia.org/wiki/Exponentiation_by_squaring
        /// </remarks>
        private static decimal ExpBySquaring(decimal x, decimal y)
        {
            //Debug.Assert(y >= 0 && decimal.Truncate(y) == y, "Only non-negative, integer powers supported.");
            if (y < 0) throw new ArgumentOutOfRangeException("y", "Negative exponents not supported!");
            if (decimal.Truncate(y) != y) throw new ArgumentException("Exponent must be an integer!", "y");

            var result = 1m;
            var multiplier = x;

            while (y > 0)
            {
                if ((y % 2) == 1)
                {
                    result *= multiplier;
                    y -= 1;
                    if (y == 0) break;
                }

                multiplier *= multiplier;
                y /= 2;
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
                result = ExpBySquaring(E, d);
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
                        nextAdd = 1;               // == Pow(d, 0) / Factorial(0) == 1 / 1 == 1
                    }
                    else
                    {
                        nextAdd *= d / iteration;  // == Pow(d, iteration) / Factorial(iteration)
                    }

                    if (nextAdd == 0) break;

                    result += nextAdd;

                    iteration += 1;
                }
            }

            // Take reciprocal if this was a negative power
            // Note that result will never be zero at this point.
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
        public static decimal Log(decimal d)
        {
            if (d < 0) throw new ArgumentException("Natural logarithm is a complex number for values less than zero!", "d");
            if (d == 0) throw new OverflowException("Natural logarithm is defined as negative infinity at zero which the Decimal data type can't represent!");
            
            if (d == 1) return 0;

            if (d >= 1)
            {
                var power = 0m;

                var x = d;
                while (x > 1)
                {
                    x /= 10;
                    power += 1;
                }

                return Log(x) + power * Ln10;
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
        /// Returns the logarithm of a specified number in a specified base.
        /// </summary>
        /// <param name="d">A number whose logarithm is to be found.</param>
        /// <param name="newBase">The base of the logarithm.</param>
        /// <remarks>
        /// This is a relatively naive implementation that simply divides the
        /// natural log of <paramref name="d"/> by the natural log of the base.
        /// </remarks>
        public static decimal Log(decimal d, decimal newBase)
        {
            // Short circuit the checks below if d is 1 because
            // that will yield 0 in the numerator below and give us
            // 0 for any base, even ones that would yield infinity.
            if (d == 1) return 0m;

            if (newBase == 1) throw new InvalidOperationException("Logarithm for base 1 is undefined.");
            if (d < 0) throw new ArgumentException("Logarithm is a complex number for values less than zero!", nameof(d));
            if (d == 0) throw new OverflowException("Logarithm is defined as negative infinity at zero which the Decimal data type can't represent!");
            if (newBase < 0) throw new ArgumentException("Logarithm base would be a complex number for values less than zero!", nameof(newBase));
            if (newBase == 0) throw new OverflowException("Logarithm base would be negative infinity at zero which the Decimal data type can't represent!");

            return Log(d) / Log(newBase);
        }

        /// <summary>
        /// Returns the base 10 logarithm of a specified number.
        /// </summary>
        /// <param name="d">A number whose logarithm is to be found.</param>
        public static decimal Log10(decimal d)
        {
            if (d < 0) throw new ArgumentException("Logarithm is a complex number for values less than zero!", nameof(d));
            if (d == 0) throw new OverflowException("Logarithm is defined as negative infinity at zero which the Decimal data type can't represent!");

            // Shrink precision from the input value and get bits for analysis
            var parts = decimal.GetBits(d / 1.000000000000000000000000000000000m);
            var scale = (parts[3] >> 16) & 0x7F;

            // Handle special cases of .1, .01, .001, etc.
            if (parts[0] == 1 && parts[1] == 0 && parts[2] == 0)
            {
                return -1 * scale;
            }

            // Handle special cases of powers of 10
            // Note: A binary search was actually found to be faster on average probably because it takes fewer iterations to find no match.
            //       It's even faster than doing a modulus 10 check first.
            if (scale == 0)
            {
                var powerOf10 = Array.BinarySearch(PowersOf10, d);
                if (powerOf10 >= 0)
                {
                    return powerOf10;
                }
            }

            return Log(d) / Ln10;
        }

        /// <summary>
        /// Returns the base 2 logarithm of a specified number.
        /// </summary>
        /// <param name="d">A number whose logarithm is to be found.</param>
        public static decimal Log2(decimal d)
        {
            if (d < 0) throw new ArgumentException("Logarithm is a complex number for values less than zero!", nameof(d));
            if (d == 0) throw new OverflowException("Logarithm is defined as negative infinity at zero which the Decimal data type can't represent!");

            return Log(d) / Ln2;
        }

        /// <summary>
        /// Returns the factorial of a number n expressed as n!. Factorial is
        /// calculated as follows: n * (n - 1) * (n - 2) * ... * 1
        /// </summary>
        /// <param name="n">An integer.</param>
        /// <remarks>
        /// Only supports non-negative integers.
        /// </remarks>
        public static decimal Factorial(decimal n)
        {
            if (n < 0) throw new ArgumentException("Values less than zero are not supoprted!", "n");
            if (Decimal.Truncate(n) != n) throw new ArgumentException("Fractional values are not supoprted!", "n");

            var ret = 1m;

            for (var i = n; i >= 2; i += -1)
            {
                ret *= i;
            }

            return ret;
        }

        /// <summary>
        /// Uses the quadratic formula to factor and solve the equation ax^2 + bx + c = 0
        /// </summary>
        /// <param name="a">The coefficient of x^2.</param>
        /// <param name="b">The coefficient of x.</param>
        /// <param name="c">The constant.</param>
        /// <remarks>
        /// Will return empty results where there is no solution and for complex solutions.
        /// See http://www.wikihow.com/Factor-Second-Degree-Polynomials-%28Quadratic-Equations%29
        /// </remarks>
        public static decimal[] SolveQuadratic(decimal a, decimal b, decimal c)
        {
            // Horizontal line is either 0 nowhere or everywhere so no solution.
            if ((a == 0) && (b == 0)) return new decimal[] { };

            if ((a == 0))
            {
                // This is actually a linear equation. Using quadratic would result in a
                // divide by zero so use the following equation.
                // 0 = b * x + c
                // -c = b * x
                // -c / b = x
                return new[] { -c / b };
            }

            // If all our coefficients have an absolute value less than 1,
            // then we'll lose precision in calculating the discriminant and
            // its root. Since we're solving for  ax^2 + bx + c = 0  we can
            // multiply the coefficients by whatever we want until they are 
            // in a more favorable range. In this case, we'll make sure here 
            // that at least one number is greater than 1 or less than -1.
            while ((-1 < a && a < 1) && (-1 < b && b < 1) && (-1 < c && c < 1)) 
            {
                a *= 10;
                b *= 10;
                c *= 10;
            }

            var discriminant = b * b - 4 * a * c;

            // Allow for a little rounding error and treat this as 0
            if (discriminant == -SmallestNonZeroDec) discriminant = 0;

            // Solution is complex -- shape does not intersect 0.
            if (discriminant < 0) return new decimal[] { };

            var sqrtOfDiscriminant = Sqrt(discriminant);

            // Select quadratic or "citardauq" depending on which one has a matching
            // sign between -b and the square root. This improves precision, sometimes
            // dramatically. See: http://math.stackexchange.com/a/56982
            var h = Math.Sign(b) == -1 ? (-b + sqrtOfDiscriminant) / (2 * a) : (2 * c) / (-b - sqrtOfDiscriminant);
            var k = Math.Sign(b) == +1 ? (-b - sqrtOfDiscriminant) / (2 * a) : (2 * c) / (-b + sqrtOfDiscriminant);

            // ax^2 + bx + c = (x - h)(x - k) 
            // (x - h)(x - k) = 0 means h and k are the values for x 
            //   that will make the equation = 0
            return h == k
                       ? new[] { h }
                       : new[] { h, k };
        }

        /// <summary>
        /// Returns the floor of a Decimal value at the given number of digits.
        /// </summary>
        /// <param name="value">A decimal value.</param>
        /// <param name="places">An integer representing the maximum number of digits 
        /// after the decimal point to end up with.</param>
        public static decimal Floor(decimal value, int places = 0)
        {
            if (places < 0) throw new ArgumentOutOfRangeException("places", "Places must be greater than or equal to 0.");

            if (places == 0) return decimal.Floor(value);

            // At or beyond precision of decimal data type
            if (places >= 28) return value;

            return decimal.Floor(value * PowersOf10[places]) / PowersOf10[places];
        }
        /// <summary>
        /// Returns the ceiling of a Decimal value at the given number of digits.
        /// </summary>
        /// <param name="value">A decimal value.</param>
        /// <param name="places">An integer representing the maximum number of digits 
        /// after the decimal point to end up with.</param>
        public static decimal Ceiling(decimal value, int places = 0)
        {
            if (places < 0) throw new ArgumentOutOfRangeException("places", "Places must be greater than or equal to 0.");

            if (places == 0) return decimal.Ceiling(value);

            // At or beyond precision of decimal data type
            if (places >= 28) return value;

            return decimal.Ceiling(value * PowersOf10[places]) / PowersOf10[places];
        }

        /// <summary>
        /// Calculates the greatest common factor of a and b to the highest level of
        /// precision represented by either number.
        /// </summary>
        /// <remarks>
        /// If either number is not an integer, the factor sought will be at the
        /// same precision as the most precise value.
        /// For example, 1.2 and 0.42 will yield 0.06.
        /// </remarks>
        public static decimal GCF(decimal a, decimal b)
        {
            // Run Euclid's algorithm
            while (true)
            {
                if (b == 0) break;
                var r = a % b;
                a = b;
                b = r;
            }

            return a;
        }

        /// <summary>
        /// Gets the greatest common factor of three or more numbers.
        /// </summary>
        public static decimal GCF(decimal a, decimal b, params decimal[] values)
        {
            return values.Aggregate(GCF(a, b), (current, value) => GCF(current, value));
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
        public static decimal AGMean(decimal x, decimal y)
        {
            decimal a;
            decimal g;

            // Handle special case
            if (x == 0 || y == 0) return 0;

            // Make sure signs match or we'll end up with a complex number
            var sign = Math.Sign(x);
            if (sign != Math.Sign(y))
                throw new Exception("Arithmetic geometric mean of these values is complex and cannot be expressed in Decimal data type!");

            // At this point, both signs match. If they're both negative, evaluate ag mean using them
            // as positive numbers and multiply result by -1.
            if (sign == -1)
            {
                x = decimal.Negate(x);
                y = decimal.Negate(y);
            }

            while (true)
            {
                a = x / 2 + y / 2;
                g = Sqrt(x * y);

                if (a == g) break;
                if (g == y && a == x) break;

                x = a;
                y = g;
            }

            return sign == -1 ? -a : a;
        }

        /// <summary>
        /// Calculates the average of the supplied numbers.
        /// </summary>
        /// <param name="values">The numbers to average.</param>
        /// <remarks>
        /// Simply uses LINQ's Average function, but switches to a potentially less
        /// accurate method of summing each value divided by the number of values.
        /// </remarks>
        public static decimal Average(params decimal[] values)
        {
            decimal avg;

            try
            {
                avg = values.Average();
            }
            catch (OverflowException)
            {
                // Use less accurate method that won't overflow
                avg = values.Sum(v => v / values.Length);
            }

            return avg;
        }

        /// <summary>
        /// Gets the number of decimal places in a decimal value.
        /// </summary>
        /// <remarks>
        /// Started with something found here: http://stackoverflow.com/a/6092298/856595
        /// </remarks>
        public static int GetDecimalPlaces(decimal dec, bool countTrailingZeros)
        {
            const int signMask = unchecked((int)0x80000000);
            const int scaleMask = 0x00FF0000;
            const int scaleShift = 16;

            int[] bits = Decimal.GetBits(dec);
            var result = (bits[3] & scaleMask) >> scaleShift;  // extract exponent

            // Return immediately for values without a fractional portion or if we're counting trailing zeros
            if (countTrailingZeros || (result == 0)) return result;

            // Get a raw version of the decimal's integer
            bits[3] = bits[3] & ~unchecked(signMask | scaleMask); // clear out exponent and negative bit
            var rawValue = new decimal(bits);

            // Account for trailing zeros
            while ((result > 0) && ((rawValue % 10) == 0))
            {
                result--;
                rawValue /= 10;
            }

            return result;
        }

        /// <summary>
        /// Gets the remainder of one number divided by another number in such a way as to retain maximum precision.
        /// </summary>
        public static decimal Remainder(decimal d1, decimal d2)
        {
            if (Math.Abs(d1) < Math.Abs(d2)) return d1;
            
            var timesInto = decimal.Truncate(d1 / d2);
            var shiftingNumber = d2;
            var sign = Math.Sign(d1);

            for (var i = 0; i <= GetDecimalPlaces(d2, true); i++)
            {
                // Note that first "digit" will be the integer portion of d2
                var digit = decimal.Truncate(shiftingNumber);

                d1 -= timesInto * (digit / PowersOf10[i]);

                shiftingNumber = (shiftingNumber - digit) * 10m; // remove used digit and shift for next iteration
                if (shiftingNumber == 0m) break;
            }

            // If we've crossed zero because of the precision mismatch, 
            // we need to add a whole d2 to get a correct result.
            if (d1 != 0 && Math.Sign(d1) != sign)
            {
                d1 = Math.Sign(d2) == sign
                         ? d1 + d2
                         : d1 - d2;
            }

            return d1;
        }
    }
}
