using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MathExtensions.TwoD
{
    [DebuggerDisplay("X = {X} Y = {Y}")]
    public struct Point2D
    {
        public readonly decimal X;
        public readonly decimal Y;

        public static readonly Point2D Origin = new Point2D(0, 0);

        public Point2D(decimal x, decimal y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets whether or not a point is on one or both axes.
        /// </summary>
        public bool OnAnAxis
        {
            get { return (X == 0) || (Y == 0); }
        }
        /// <summary>
        /// Gets which quadrant the point is in as long as it's actually
        /// in a quadrant.
        /// </summary>
        /// <remarks>See http://mathworld.wolfram.com/Quadrant.html</remarks>
        /// <exception cref="System.Exception">The point is on one of the axes or is the origin.</exception>
        public int Quadrant
        {
            get
            {
                if (OnAnAxis)
                {
                    throw new Exception("Point lies on an axis or axes and therefore is not in one of the quadrants!");
                }
                if (Y > 0)
                {
                    return X < 0 ? 2 : 1;
                }
                else
                {
                    return X < 0 ? 3 : 4;
                }
            }
        }

        public decimal DistanceTo(Point2D pt)
        {

            return DistanceTo(pt.X, pt.Y);

        }
        public decimal DistanceTo(decimal x, decimal y)
        {
            var xDist = this.X - x;
            var yDist = this.Y - y;

            return MathExt.Sqrt(xDist * xDist + yDist * yDist);
        }
        public decimal DistanceTo(Circle2D c)
        {
            return c.DistanceTo(this);
        }
        public decimal DistanceTo(LineSeg2D l, bool treatLineSegmentAsLine = false)
        {
            return l.DistanceTo(this, treatLineSegmentAsLine);
        }
        public Vector2D GetVectorTo(Point2D other)
        {
            return new Vector2D(this, other);
        }

        public static bool PointsAreColinear(decimal x1, decimal y1, decimal x2, decimal y2, decimal x3, decimal y3)
        {
            var l1 = new LineSeg2D(x1, y1, x2, y2);
            var l2 = new LineSeg2D(x2, y2, x3, y3);
            var intersect = l1.GetIntersect(l2, true);

            // Points won't have an intersection if they are parallel. If they
            // are parallel and share a point, then they are colinear.
            return (!intersect.HasValue);
        }
        public static bool PointsAreColinear(Point2D pt1, Point2D pt2, Point2D pt3)
        {
            return PointsAreColinear(pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y);
        }

        /// <summary>
        /// Offsets a point by the adding the components of a vector.
        /// </summary>
        /// <param name="pt">A 2D point.</param>
        /// <param name="v">A 2D vector.</param>
        public static Point2D operator +(Point2D pt, Vector2D v)
        {
            return new Point2D(pt.X + v.X, pt.Y + v.Y);
        }
        /// <summary>
        /// Offsets a point by the subtracting the components of a vector.
        /// </summary>
        /// <param name="pt">A 2D point.</param>
        /// <param name="v">A 2D vector.</param>
        public static Point2D operator -(Point2D pt, Vector2D v)
        {
            return new Point2D(pt.X - v.X, pt.Y - v.Y);
        }
        public static bool operator !=(Point2D ptA, Point2D ptB)
        {
            return (ptA.X != ptB.X) || (ptA.Y != ptB.Y);
        }
        public static bool operator ==(Point2D ptA, Point2D ptB)
        {
            return (ptA.X == ptB.X) && (ptA.Y == ptB.Y);
        }

        /// <summary>
        /// Creates a new XYPoint with the X and Y components rounded to the given
        /// decimal places. Rounding used is always AwayFromZero.
        /// </summary>
        /// <param name="decimals">The number of significant decimal places (precision) in the return value.</param>
        public Point2D RoundTo(int decimals)
        {
            return new Point2D(X.RoundFromZero(decimals), Y.RoundFromZero(decimals));
        }

        /// <summary>
        /// Compares this point against another object.
        /// </summary>
        /// <param name="obj">The object to compare against.</param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Point2D && Equals((Point2D)obj);
        }

        /// <summary>
        /// Compares this point against another.
        /// </summary>
        /// <param name="other">A 2D point.</param>
        public bool Equals(Point2D other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            unchecked { return Helper.HashStart.HashValue(X).HashValue(Y); }
        }

        /// <summary>
        /// Compares this point against another to the given number of decimal places. Rounds away from
        /// zero to the given number of decimals.
        /// </summary>
        /// <param name="other">A 2D point.</param>
        /// <param name="decimals">The number of significant decimal places (precision) in the return value.</param>
        public bool Equals(Point2D other, int decimals)
        {
            return RoundTo(decimals) == other.RoundTo(decimals);
        }

        /// <summary>
        /// Compares a list of points and returns the one with the minimum Y value.
        /// If more than one point has the same minimum Y value, then the first
        /// match is returned.
        /// </summary>
        /// <param name="points">Points to compare.</param>
        public static Point2D MinY(params Point2D[] points)
        {
            if (points == null) throw new ArgumentException("Null array when expecting list of points!");
            if (points.Length == 0) throw new ArgumentException("List of points is empty!");

            var lowest = 0;
            for (var i = 1; i < points.Length; i++)
            {
                if (points[i].Y < points[lowest].Y) lowest = i;
            }

            return points[lowest];
        }
        /// <summary>
        /// Compares a list of points and returns the one with the maximum Y value.
        /// If more than one point has the same maximum Y value, then the first
        /// match is returned.
        /// </summary>
        /// <param name="points">Points to compare.</param>
        public static Point2D MaxY(params Point2D[] points)
        {
            if (points == null) throw new ArgumentException("Null array when expecting list of points!");
            if (points.Length == 0) throw new ArgumentException("List of points is empty!");

            var highest = 0;
            for (var i = 1; i < points.Length; i++)
            {
                if (points[i].Y > points[highest].Y) highest = i;
            }

            return points[highest];
        }

        /// <summary>
        /// Compares a list of points and returns the one with the minimum X value.
        /// If more than one point has the same minimum X value, then the first
        /// match is returned.
        /// </summary>
        /// <param name="points">Points to compare.</param>
        public static Point2D MinX(params Point2D[] points)
        {
            if (points == null) throw new ArgumentException("Null array when expecting list of points!");
            if (points.Length == 0) throw new ArgumentException("List of points is empty!");

            var lowest = 0;
            for (var i = 1; i < points.Length; i++)
            {
                if (points[i].X < points[lowest].X) lowest = i;
            }

            return points[lowest];
        }
        /// <summary>
        /// Compares a list of points and returns the one with the maximum X value.
        /// If more than one point has the same maximum X value, then the first
        /// match is returned.
        /// </summary>
        /// <param name="points">Points to compare.</param>
        public static Point2D MaxX(params Point2D[] points)
        {
            if (points == null) throw new ArgumentException("Null array when expecting list of points!");
            if (points.Length == 0) throw new ArgumentException("List of points is empty!");

            var highest = 0;
            for (var i = 1; i < points.Length; i++)
            {
                if (points[i].X > points[highest].X) highest = i;
            }

            return points[highest];
        }

        public override string ToString()
        {
            return X + "," + Y;
        }
    }

}
