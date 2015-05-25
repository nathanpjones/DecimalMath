using System;
using System.Diagnostics;

namespace DecimalMath
{
    public static partial class DecimalEx
    {
        /// <summary>
        /// Converts degrees to radians. (π radians = 180 degrees)
        /// </summary>
        /// <param name="degrees">The degrees to convert.</param>
        public static decimal ToRad(decimal degrees)
        {
            if (degrees % 360m == 0)
            {
                return (degrees / 360m) * TwoPi;
            }
            if (degrees % 270m == 0)
            {
                return (degrees / 270m) * (Pi + PiHalf);
            }
            if (degrees % 180m == 0)
            {
                return (degrees / 180m) * Pi;
            }
            if (degrees % 90m == 0)
            {
                return (degrees / 90m) * PiHalf;
            }
            if (degrees % 45m == 0)
            {
                return (degrees / 45m) * PiQuarter;
            }
            if (degrees % 15m == 0)
            {
                return (degrees / 15m) * PiTwelfth;
            }

            return degrees * Pi / 180m;
        }

        /// <summary>
        /// Converts radians to degrees. (π radians = 180 degrees)
        /// </summary>
        /// <param name="radians">The radians to convert.</param>
        public static decimal ToDeg(decimal radians)
        {
            const decimal ratio = 180m / Pi;

            return radians * ratio;
        }

        /// <summary>
        /// Normalizes an angle in radians to the 0 to 2Pi interval.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        public static decimal NormalizeAngle(decimal radians)
        {
            radians = Remainder(radians, TwoPi);
            if (radians < 0) radians += TwoPi;
            return radians;
        }

        /// <summary>
        /// Normalizes an angle in degrees to the 0 to 360 degree interval.
        /// </summary>
        /// <param name="degrees">Angle in degrees.</param>
        public static decimal NormalizeAngleDeg(decimal degrees)
        {
            degrees = degrees % 360m;
            if (degrees < 0) degrees += 360m;
            return degrees;
        }

        /// <summary>
        /// Returns the sine of the specified angle.
        /// </summary>
        /// <param name="x">An angle, measured in radians.</param>
        /// <remarks>
        /// Uses a Taylor series to calculate sine. See 
        /// http://en.wikipedia.org/wiki/Trigonometric_functions for details.
        /// </remarks>
        public static decimal Sin(decimal x)
        {
            // Normalize to between -2Pi <= x <= 2Pi
            x = Remainder(x, TwoPi);

            if (x == 0 || x == Pi || x == TwoPi)
            {
                return 0;
            }
            if (x == PiHalf)
            {
                return 1;
            }
            if (x == Pi + PiHalf)
            {
                return -1;
            }

            var result = 0m;
            var doubleIteration = 0; // current iteration * 2
            var xSquared = x * x;
            var nextAdd = 0m;

            while (true)
            {
                if (doubleIteration == 0)
                {
                    nextAdd = x;
                }
                else
                {
                    // We multiply by -1 each time so that the sign of the component
                    // changes each time. The first item is positive and it
                    // alternates back and forth after that.
                    // Following is equivalent to: nextAdd *= -1 * x * x / ((2 * iteration) * (2 * iteration + 1));
                    nextAdd *= -1 * xSquared / (doubleIteration * doubleIteration + doubleIteration);
                }

                Debug.WriteLine("{0:000}:{1,33:+0.0000000000000000000000000000;-0.0000000000000000000000000000} ->{2,33:+0.0000000000000000000000000000;-0.0000000000000000000000000000}",
                    doubleIteration / 2, nextAdd, result + nextAdd);
                if (nextAdd == 0) break;

                result += nextAdd;

                doubleIteration += 2;
            }

            return result;
        }

        /// <summary>
        /// Returns the cosine of the specified angle.
        /// </summary>
        /// <param name="x">An angle, measured in radians.</param>
        /// <remarks>
        /// Uses a Taylor series to calculate sine. See 
        /// http://en.wikipedia.org/wiki/Trigonometric_functions for details.
        /// </remarks>
        public static decimal Cos(decimal x)
        {
            // Normalize to between -2Pi <= x <= 2Pi
            x = Remainder(x, TwoPi);

            if (x == 0 || x == TwoPi)
            {
                return 1;
            }
            if (x == Pi)
            {
                return -1;
            }
            if (x == PiHalf || x == Pi + PiHalf)
            {
                return 0;
            }

            var result = 0m;
            var doubleIteration = 0; // current iteration * 2
            var xSquared = x * x;
            var nextAdd = 0m;

            while (true)
            {
                if (doubleIteration == 0)
                {
                    nextAdd = 1;
                }
                else
                {
                    // We multiply by -1 each time so that the sign of the component
                    // changes each time. The first item is positive and it
                    // alternates back and forth after that.
                    // Following is equivalent to: nextAdd *= -1 * x * x / ((2 * iteration - 1) * (2 * iteration));
                    nextAdd *= -1 * xSquared / (doubleIteration * doubleIteration - doubleIteration);
                }

                if (nextAdd == 0) break;

                result += nextAdd;

                doubleIteration += 2;
            }

            return result;
        }

        /// <summary>
        /// Returns the tangent of the specified angle.
        /// </summary>
        /// <param name="radians">An angle, measured in radians.</param>
        /// <remarks>
        /// Uses a Taylor series to calculate sine. See 
        /// http://en.wikipedia.org/wiki/Trigonometric_functions for details.
        /// </remarks>
        public static decimal Tan(decimal radians)
        {
            try
            {
                return Sin(radians) / Cos(radians);
            }
            catch (DivideByZeroException)
            {
                throw new Exception("Tangent is undefined at this angle!");
            }
        }

        /// <summary>
        /// Returns the angle whose sine is the specified number.
        /// </summary>
        /// <param name="z">A number representing a sine, where -1 ≤d≤ 1.</param>
        /// <remarks>
        /// See http://en.wikipedia.org/wiki/Inverse_trigonometric_function
        /// and http://mathworld.wolfram.com/InverseSine.html
        /// I originally used the Taylor series for ASin, but it was extremely slow
        /// around -1 and 1 (millions of iterations) and still ends up being less
        /// accurate than deriving from the ATan function.
        /// </remarks>
        public static decimal ASin(decimal z)
        {
            if (z < -1 || z > 1)
            {
                throw new ArgumentOutOfRangeException("z", "Argument must be in the range -1 to 1 inclusive.");
            }

            // Special cases
            if (z == -1) return -PiHalf;
            if (z == 0) return 0;
            if (z == 1) return PiHalf;

            return 2m * ATan(z / (1 + Sqrt(1 - z * z)));
        }

        /// <summary>
        /// Returns the angle whose cosine is the specified number.
        /// </summary>
        /// <param name="z">A number representing a cosine, where -1 ≤d≤ 1.</param>
        /// <remarks>
        /// See http://en.wikipedia.org/wiki/Inverse_trigonometric_function
        /// and http://mathworld.wolfram.com/InverseCosine.html
        /// </remarks>
        public static decimal ACos(decimal z)
        {
            if (z < -1 || z > 1)
            {
                throw new ArgumentOutOfRangeException("z", "Argument must be in the range -1 to 1 inclusive.");
            }

            // Special cases
            if (z == -1) return Pi;
            if (z == 0) return PiHalf;
            if (z == 1) return 0;

            return 2m * ATan(Sqrt(1 - z * z) / (1 + z));
        }

        /// <summary>
        /// Returns the angle whose tangent is the quotient of two specified numbers.
        /// </summary>
        /// <param name="x">A number representing a tangent.</param>
        /// <remarks>
        /// See http://mathworld.wolfram.com/InverseTangent.html for faster converging 
        /// series from Euler that was used here.
        /// </remarks>
        public static decimal ATan(decimal x)
        {
            // Special cases
            if (x == -1) return -PiQuarter;
            if (x == 0) return 0;
            if (x == 1) return PiQuarter;
            if (x < -1)
            {
                // Force down to -1 to 1 interval for faster convergence
                return -PiHalf - ATan(1 / x);
            }
            if (x > 1)
            {
                // Force down to -1 to 1 interval for faster convergence
                return PiHalf - ATan(1 / x);
            }

            var result = 0m;
            var doubleIteration = 0; // current iteration * 2
            var y = (x * x) / (1 + x * x);
            var nextAdd = 0m;

            while (true)
            {
                if (doubleIteration == 0)
                {
                    nextAdd = x / (1 + x * x);  // is = y / x  but this is better for very small numbers where y = 9
                }
                else
                {
                    // We multiply by -1 each time so that the sign of the component
                    // changes each time. The first item is positive and it
                    // alternates back and forth after that.
                    // Following is equivalent to: nextAdd *= y * (iteration * 2) / (iteration * 2 + 1);
                    nextAdd *= y * doubleIteration / (doubleIteration + 1);
                }

                if (nextAdd == 0) break;

                result += nextAdd;

                doubleIteration += 2;
            }

            return result;
        }

        /// <summary>
        /// Returns the angle whose tangent is the quotient of two specified numbers.
        /// </summary>
        /// <param name="y">The y coordinate of a point.</param>
        /// <param name="x">The x coordinate of a point.</param>
        /// <returns>
        /// An angle, θ, measured in radians, such that -π≤θ≤π, and tan(θ) = y / x,
        /// where (x, y) is a point in the Cartesian plane. Observe the following: 
        /// For (x, y) in quadrant 1, 0 &lt; θ &lt; π/2.
        /// For (x, y) in quadrant 2, π/2 &lt; θ ≤ π.
        /// For (x, y) in quadrant 3, -π &lt; θ &lt; -π/2.
        /// For (x, y) in quadrant 4, -π/2 &lt; θ &lt; 0.
        /// </returns>
        public static decimal ATan2(decimal y, decimal x)
        {
            if (x == 0 && y == 0)
            {
                return 0;                   // X0, Y0
            }
            
            if (x == 0)
            {
                return y > 0
                           ? PiHalf         // X0, Y+
                           : -PiHalf;       // X0, Y-
            }
            
            if (y == 0)
            {
                return x > 0
                           ? 0              // X+, Y0
                           : Pi;            // X-, Y0
            }
            
            var aTan = ATan(y / x);
            
            if (x > 0) return aTan;         // Q1&4: X+, Y+-
                                            
            return y > 0                    
                       ? aTan + Pi          //   Q2: X-, Y+
                       : aTan - Pi;         //   Q3: X-, Y-

        }
    }
}
