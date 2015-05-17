using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DecimalEx;

namespace Decimal2D
{
    /// <summary>
    /// Represents a 2D vector.
    /// </summary>
    /// <remarks>
    /// Useful links:
    /// http://emweb.unl.edu/math/mathweb/vectors/vectors.html
    /// http://www.mathrec.org/vector.html
    /// </remarks>
    [DebuggerDisplay("X = {X} Y = {Y}")]
    public struct Vector2D
    {

        public decimal X;

        public decimal Y;
        /// <summary>
        /// Creates a vector with the supplied X and Y components.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        public Vector2D(decimal x, decimal y)
        {
            this.X = x;
            this.Y = y;
        }
        /// <summary>
        /// Creates a new vector that starts from the starting point and points to
        /// the end point with a length of the distance between the two points.
        /// </summary>
        /// <param name="startPt">The starting point.</param>
        /// <param name="endPt">The ending point.</param>
        public Vector2D(Point2D startPt, Point2D endPt)
        {
            this.X = endPt.X - startPt.X;
            this.Y = endPt.Y - startPt.Y;
        }
        /// <summary>
        /// Creates a new vector that starts from the starting point and points to
        /// the end point with the given magnitude.
        /// </summary>
        /// <param name="startPt">The starting point.</param>
        /// <param name="endPt">The ending point.</param>
        /// <param name="magnitude">A magnitude.</param>
        public Vector2D(Point2D startPt, Point2D endPt, decimal magnitude)
        {
            this.X = endPt.X - startPt.X;
            this.Y = endPt.Y - startPt.Y;
            this.Magnitude = magnitude;
        }
        /// <summary>
        /// Creates a new vector that starts from the starting point and points to
        /// the end point with a length of the distance between the two points.
        /// </summary>
        /// <param name="startX">The starting point X component.</param>
        /// <param name="startY">The starting point Y component.</param>
        /// <param name="endX">The ending point X component.</param>
        /// <param name="endY">The ending point Y component.</param>
        public Vector2D(decimal startX, decimal startY, decimal endX, decimal endY)
        {
            this.X = endX - startX;
            this.Y = endY - startY;
        }
        /// <summary>
        /// Creates a new vector with the same direction as the given vector
        /// but with the supplied magnitude.
        /// </summary>
        /// <param name="v">A vector.</param>
        /// <param name="magnitude">A magnitude.</param>
        /// <remarks></remarks>
        public Vector2D(Vector2D v, decimal magnitude)
        {
            this.X = v.X;
            this.Y = v.Y;
            this.Magnitude = magnitude;
        }
        /// <summary>
        /// Creates a new vector starting at the origin and terminating at this point.
        /// </summary>
        /// <param name="endPt">End point of vector starting at origin.</param>
        public Vector2D(Point2D endPt)
        {
            this.X = endPt.X;
            this.Y = endPt.Y;
        }

        /// <summary>
        /// Creates a new XYPoint with the X and Y components rounded to the given
        /// decimal places. Rounding used is always AwayFromZero.
        /// </summary>
        /// <param name="decimals">The number of significant decimal places (precision) in the return value.</param>
        public Vector2D RoundTo(int decimals)
        {
            return new Vector2D(X.RoundFromZero(decimals), Y.RoundFromZero(decimals));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2D))
            {
                throw new Exception("Can't compare " + this.GetType().Name + " to an object of a different type!");
            }
            return this == (Vector2D)obj;
        }
        /// <summary>
        /// Compares this point against another to the given number of decimal places.
        /// </summary>
        /// <param name="other">A 2D vector.</param>
        /// <param name="decimals">The number of significant decimal places (precision) in the return value.</param>
        public bool Equals(Vector2D other, int decimals)
        {
            return this.RoundTo(decimals) == other.RoundTo(decimals);
        }

        public static bool operator ==(Vector2D objA, Vector2D objB)
        {
            return objA.X == objB.X && objA.Y == objB.Y;
        }
        public static bool operator !=(Vector2D objA, Vector2D objB)
        {
            return objA.X != objB.X || objA.Y != objB.Y;
        }

        /// <summary>
        /// Gets whether or not this is a unit vector, i.e. has a magnitude of 1.
        /// </summary>
        /// <param name="decimals">Optional precision at which to make the comparison.</param>
        public bool IsUnitVector(int decimals = -1)
        {
            // TODO: Change name to GetIsUnitVector
            if (decimals >= 0)
            {
                return (Magnitude.RoundFromZero(decimals) == 1m);
            }
            else
            {
                return (Magnitude == 1m);
            }
        }

        /// <summary>
        /// Gets whether or not this is the null (or zero or empty) vector, in other
        /// words all its components are equal to zero.
        /// </summary>
        /// <remarks>
        /// Could be "X = 0 AndAlso Y = 0" except when you actually make the calculation
        /// to get the magnitude, you sometimes get 0 even when one of the numbers is
        /// a very small non-zero value. Since this routine is called to avoid divide-by-
        /// zero errors when dividing by magnitude, this routine should look at magnitude
        /// instead.
        /// </remarks>
        public bool IsNull
        {


            get { return Magnitude == 0; }
        }
        /// <summary>
        /// Gets or sets the magnitude or length of the vector.
        /// </summary>
        public decimal Magnitude
        {


            get { return Point2D.Origin.DistanceTo(X, Y); }

            set
            {
                if (this.IsNull && value != 0)
                {
                    throw new Exception("Can't set magnitude of null vector to non-zero value since it has no defined direction!");
                }

                // This seems to yield better precision than using an
                // intermediary value for "value / Magnitude".
                decimal origMagnitude = Magnitude;
                X = X * value / origMagnitude;
                Y = Y * value / origMagnitude;

            }
        }

        /// <summary>
        /// Converts this vector to its unit vector, i.e. same direction but
        /// with a magnitude of 1.
        /// </summary>
        public Vector2D Normalize()
        {

            if (this.IsNull)
            {
                throw new Exception("Can't normalize a null vector!");
            }

            return this / Magnitude;

        }

        /// <summary>
        /// Adds the two vectors. Is commutative.
        /// </summary>
        /// <param name="v1">A 2D vector.</param>
        /// <param name="v2">A 2D vector.</param>
        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {

            return new Vector2D(v1.X + v2.X, v1.Y + v2.Y);

        }
        /// <summary>
        /// Subtracts one vector from another. v1 - v2 = v1 + (-v2)
        /// </summary>
        /// <param name="v1">A 2D vector.</param>
        /// <param name="v2">A 2D vector.</param>
        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        {

            return new Vector2D(v1.X - v2.X, v1.Y - v2.Y);

        }
        /// <summary>
        /// Reverses the vector by multiplying its elements by -1.
        /// </summary>
        /// <param name="v">A 2D vector.</param>
        public static Vector2D operator -(Vector2D v)
        {

            return new Vector2D(-v.X, -v.Y);

        }
        /// <summary>
        /// Multiplies a vector by a scalar value.
        /// </summary>
        /// <param name="v">The vector to multiply.</param>
        /// <param name="scale">The scalar value.</param>
        public static Vector2D operator *(Vector2D v, decimal scale)
        {
            return new Vector2D(v.X * scale, v.Y * scale);
        }
        /// <summary>
        /// Multiplies a vector by a scalar value.
        /// </summary>
        /// <param name="scale">The scalar value.</param>
        /// <param name="v">The vector to multiply.</param>
        public static Vector2D operator *(decimal scale, Vector2D v)
        {
            return new Vector2D(v.X * scale, v.Y * scale);
        }
        /// <summary>
        /// Divides each element of a vector by a scalar value.
        /// </summary>
        /// <param name="v">A 2D vector.</param>
        /// <param name="scale">The scalar value.</param>
        public static Vector2D operator /(Vector2D v, decimal scale)
        {
            return new Vector2D(v.X / scale, v.Y / scale);
        }
        /// <summary>
        /// Returns the dot product of this vector and the other vector.
        /// (Is commutative.)
        /// </summary>
        /// <param name="other">The other vector to get the dot product with.</param>
        public decimal Dot(Vector2D other)
        {

            return this.X * other.X + this.Y * other.Y;

        }
        /// <summary>
        /// Projects this vector onto another.
        /// </summary>
        /// <param name="other"></param>
        /// <remarks>See http://mathworld.wolfram.com/Projection.html</remarks>
        public Vector2D ProjectOnto(Vector2D other)
        {

            decimal otherMag = 0m;
            decimal multiplier = 0m;

            if (other.IsNull)
            {
                throw new Exception("Can't project onto a null vector!");
            }

            otherMag = other.Magnitude;
            multiplier = this.Dot(other) / otherMag;

            return multiplier * (other / otherMag);

        }
        /// <summary>
        /// Gets the angle in degrees between this vector and another.
        /// </summary>
        /// <param name="other">The other vector to get the angle to.</param>
        /// <remarks>See http://en.wikipedia.org/wiki/Dot_product</remarks>
        public decimal AngleTo(Vector2D other)
        {

            if (this.IsNull || other.IsNull)
            {
                throw new Exception("Can't find angle when one or both vectors are null vectors!");
            }

            // Cos(theta) = DotProduct(v1,v2) / (length(v1) * length(v2))
            // aka theta = acos(v.normalize.dot(other.normalize)), however, the equation
            //   used gives us better precision
            return DecimalEx.DecimalEx.ToDeg(DecimalEx.DecimalEx.ACos(this.Dot(other) / (this.Magnitude * other.Magnitude)));

        }
        /// <summary>
        /// Gets the angle of the vector in degrees from the X+ axis. Will return result in the range
        /// -180 to +180. Will return 0 for null vector.
        /// </summary>
        public decimal Angle()
        {

            return DecimalEx.DecimalEx.ToDeg(DecimalEx.DecimalEx.ATan2(Y, X));

        }

        /// <summary>
        /// Gets a vector of the same magnitude pointing in the opposite direction.
        /// </summary>
        public Vector2D GetReversed()
        {

            return new Vector2D(-X, -Y);

        }
        /// <summary>
        /// Gets one of the two vectors perpendicular to this vector. New vector will
        /// be 90 degrees clockwise from original vector.
        /// </summary>
        public Vector2D GetPerpendicular()
        {

            return new Vector2D(Y, -X);

        }

        public override string ToString()
        {

            return X + "," + Y;

        }

        /// <summary>
        /// The null (or zero or empty) vector.
        /// </summary>
        public static readonly Vector2D Null = new Vector2D(0, 0);
        /// <summary> The X unit vector. </summary>
        public static readonly Vector2D XUnit = new Vector2D(1, 0);
        /// <summary> The Y unit vector. </summary>

        public static readonly Vector2D YUnit = new Vector2D(0, 1);
    }
}
