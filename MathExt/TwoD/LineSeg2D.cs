using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MathExtensions.TwoD
{
    [DebuggerDisplay("(X = {Pt1.X} Y = {Pt1.Y}) - (X = {Pt2.X} Y = {Pt2.Y})")]
    public struct LineSeg2D
    {

        public Point2D Pt1;

        public Point2D Pt2;
        public LineSeg2D(Point2D pt1, Point2D pt2)
        {
            this.Pt1 = pt1;
            this.Pt2 = pt2;
        }
        public LineSeg2D(decimal x1, decimal y1, decimal x2, decimal y2)
        {
            Pt1 = new Point2D(x1, y1);
            Pt2 = new Point2D(x2, y2);
        }

        /// <summary>
        /// Offsets a line segment by offsetting each of its points by adding the components of a vector.
        /// </summary>
        /// <param name="l">A 2D line segment.</param>
        /// <param name="v">A 2D vector.</param>
        public static LineSeg2D operator +(LineSeg2D l, Vector2D v)
        {

            return new LineSeg2D(l.Pt1 + v, l.Pt2 + v);

        }
        /// <summary>
        /// Offsets a line segment by offsetting each of its points by subtracting the components of a vector.
        /// </summary>
        /// <param name="l">A 2D line segment.</param>
        /// <param name="v">A 2D vector.</param>
        public static LineSeg2D operator -(LineSeg2D l, Vector2D v)
        {

            return new LineSeg2D(l.Pt1 - v, l.Pt2 - v);

        }
        public static bool operator !=(LineSeg2D lineSegA, LineSeg2D lineSegB)
        {
            return (lineSegA.Pt1 != lineSegB.Pt1) || (lineSegA.Pt2 != lineSegB.Pt2);
        }
        public static bool operator ==(LineSeg2D lineSegA, LineSeg2D lineSegB)
        {
            return (lineSegA.Pt1 == lineSegB.Pt1) && (lineSegA.Pt2 == lineSegB.Pt2);
        }

        /// <summary>
        /// Gets or sets the midpoint of the line segment.
        /// </summary>
        public Point2D MidPoint
        {


            get { return new Point2D((Pt1.X + Pt2.X) / 2m, (Pt1.Y + Pt2.Y) / 2m); }

            set
            {
                var oldMidPt = MidPoint;

                Pt1 = new Point2D((Pt1.X - oldMidPt.X) + value.X,
                                  (Pt1.Y - oldMidPt.Y) + value.Y);

                Pt2 = new Point2D((Pt2.X - oldMidPt.X) + value.X,
                                  (Pt2.Y - oldMidPt.Y) + value.Y);
            }
        }
        /// <summary>
        /// Returns a perpendicular bisector of the line segment by
        /// rotating the line segment around its midpoint by 90 
        /// degrees (clockwise).
        /// </summary>
        /// <remarks>This has been tested against a large random
        /// sampled set of points and matches the output of using
        /// a matrix transformation.</remarks>
        public LineSeg2D PerpendicularBisector()
        {

            Point2D pt = default(Point2D);
            Point2D newPt1 = default(Point2D);
            Point2D newPt2 = default(Point2D);

            _CheckIsValid();

            pt = MidPoint;

            // This is the long-hand of a matrix that rotates
            // the line segment around its midpoint.

            newPt1 = new Point2D(-Pt1.Y + (pt.X + pt.Y),
                                 Pt1.X + (-pt.X + pt.Y));

            newPt2 = new Point2D(-Pt2.Y + (pt.X + pt.Y),
                                 Pt2.X + (-pt.X + pt.Y));

            return new LineSeg2D(newPt1, newPt2);
        }
        /// <summary>
        /// Gets the slope perpendicular to this line segment's slope. Equal to
        /// -1 / slope
        /// </summary>
        /// <exception cref="Exception">The line is horizontal and it's perpendicular slope would be vertical which has no slope.</exception>
        public decimal PerpendicularSlope
        {

            get
            {
                _CheckIsValid();

                if (IsHorizontal)
                    throw new Exception("Line segment has no perpendicular slope! Perpendicular line would be vertical.");

                return -(Pt2.X - Pt1.X) / (Pt2.Y - Pt1.Y);

            }
        }
        /// <summary>
        /// Gets the length of the line segment, i.e. the distance between
        /// the two points.
        /// </summary>
        public decimal Length
        {


            get { return Pt1.DistanceTo(Pt2); }
        }

        /// <summary>
        /// Gets whether or not this line segment is zero length,
        /// i.e. Pt1 and Pt2 are equal.
        /// </summary>
        public bool IsZeroLength()
        {
            // TODO: Rename to GetIsZeroLength

            return Pt1.Equals(Pt2);
        }
        /// <summary>
        /// Gets whether or not this line segment is zero length
        /// at the given precision.
        /// </summary>
        public bool IsZeroLength(int decimals)
        {
            // TODO: Rename to GetIsZeroLength
            return Length.RoundFromZero(decimals) == 0;
        }
        private void _CheckIsValid()
        {
            if (Pt1 == Pt2)
            {
                throw new Exception("Can't perform action on or retrieve information from invalid line segment!");
            }
        }
        public bool IsHorizontal
        {

            get
            {
                _CheckIsValid();

                // NOTE: Use calculation instead of Pt2.Y = Pt1.Y since this
                // calculation would be what causes a divide by zero error.
                return Pt2.Y - Pt1.Y == 0;

            }
        }
        public bool IsVertical
        {

            get
            {
                _CheckIsValid();

                // NOTE: Use calculation instead of Pt2.X = Pt1.X since this
                // calculation would be what causes a divide by zero error.
                return Pt2.X - Pt1.X == 0;

            }
        }

        /// <summary>
        /// Gets slope of the line, aka the ratio of the change in Y to the change in X.
        /// </summary>
        /// <exception cref="Exception">The line is vertical which has no slope.</exception>
        public decimal Slope
        {

            get
            {
                _CheckIsValid();

                if (IsVertical)
                    throw new Exception("Line segment has no slope! Line is vertical.");

                return (Pt2.Y - Pt1.Y) / (Pt2.X - Pt1.X);

            }
        }
        public decimal YIntersect
        {

            get
            {
                _CheckIsValid();

                return Pt1.Y - Slope * Pt1.X;

            }
        }

        /// <summary>
        /// Treats this line segment as a line and solves for X given the specified Y.
        /// </summary>
        /// <param name="y">Y value.</param>
        public decimal GetX(decimal y)
        {

            _CheckIsValid();

            // y - y1 = m * (x - x1)
            // y - y1 = m * x - mx1
            // m * x = y - y1 + mx1
            // x = (y - y1 + mx1) / m
            // x = (y - y1) / m + x1

            if (this.IsHorizontal)
                throw new Exception("Line is horizontal!");


            if (this.IsVertical)
            {
                // If vertical, then X is the same for all Y values
                return Pt1.X;


            }
            else
            {
                return (y - Pt1.Y) / Slope + Pt1.X;

            }

        }
        /// <summary>
        /// Treats this line segment as a line and solves for Y given the specified X.
        /// </summary>
        /// <param name="x">X value.</param>
        public decimal GetY(decimal x)
        {

            _CheckIsValid();

            // y - y1 = m * (x - x1)
            // y = m * (x - x1) + y1

            if (this.IsVertical)
                throw new Exception("Line is vertical!");

            return Slope * (x - Pt1.X) + Pt1.Y;

        }
        /// <summary>
        /// Treats this line segment as a line and returns a
        /// point on the line whose X component matches the
        /// supplied value.
        /// </summary>
        /// <param name="x">X component of the point.</param>
        public Point2D PointAtX(decimal x)
        {
            return new Point2D(x, GetY(x));
        }
        /// <summary>
        /// Treats this line segment as a line and returns a
        /// point on the line whose Y component matches the
        /// supplied value.
        /// </summary>
        /// <param name="y">Y component of the point.</param>
        public Point2D PointAtY(decimal y)
        {
            return new Point2D(GetX(y), y);
        }

        public decimal Top
        {
            [DebuggerStepThrough()]
            get { return Math.Max(Pt1.Y, Pt2.Y); }
        }
        public decimal Bottom
        {
            [DebuggerStepThrough()]
            get { return Math.Min(Pt1.Y, Pt2.Y); }
        }
        public decimal Left
        {
            [DebuggerStepThrough()]
            get { return Math.Min(Pt1.X, Pt2.X); }
        }
        public decimal Right
        {
            [DebuggerStepThrough()]
            get { return Math.Max(Pt1.X, Pt2.X); }
        }
        public decimal Height
        {
            get { return Top - Bottom; }
        }
        public decimal Width
        {
            get { return Right - Left; }
        }

        public bool IsInBounds(Point2D pt)
        {

            return Bottom <= pt.Y && pt.Y <= Top && Left <= pt.X && pt.X <= Right;

        }

        public Point2D[] GetIntersect(Circle2D c, int decimals = -1)
        {

            _CheckIsValid();

            return c.GetIntersect(this, decimals);

        }
        /// <summary>
        /// Gets the intersection of one line segment with another.
        /// </summary>
        /// <param name="other">The other line segment to find intersection with.</param>
        /// <param name="treatLineSegmentsAsLines">
        /// If this is true, will return the intersection of the lines of which
        /// the segments are made up. Otherwise, will only return intersections that lie on both
        /// segments.
        /// </param>
        public Nullable<Point2D> GetIntersect(LineSeg2D other, bool treatLineSegmentsAsLines = false)
        {

            _CheckIsValid();

            // Solved Point-Slope Form
            // y - y1 = m1(x - x1)
            // y - y2 = m2(x - x2)
            //
            // y = m1x - m1x1 + y1
            // y = m2x - m2x2 + y2
            //
            // m1x - m1x1 + y1 = m2x - m2x2 + y2
            // m1x - m2x = m1x1 - m2x2 + y2 - y1
            // x(m1 - m2) = m1x1 - m2x2 + y2 - y1
            // x = (m1x1 - m2x2 + y2 - y1) / (m1 - m2)

            bool vertical1 = false;
            bool vertical2 = false;
            decimal m1 = 0m;
            decimal m2 = 0m;
            Point2D pt = default(Point2D);

            vertical1 = this.IsVertical;
            vertical2 = other.IsVertical;


            if (vertical1 && vertical2)
            {
                // Parallel
                return null;
            }
            else if (vertical1)
            {
                pt = new Point2D(this.Pt1.X, other.GetY(pt.X));
            }
            else if (vertical2)
            {
                pt = new Point2D(other.Pt1.X, this.GetY(pt.X));
            }
            else
            {
                m1 = this.Slope;
                m2 = other.Slope;

                // Lines are parallel
                if (m1 == m2)
                    return null;

                pt = new Point2D((m1 * this.Pt1.X - m2 * other.Pt1.X + other.Pt1.Y - this.Pt1.Y) / (m1 - m2),
                                 m1 * pt.X - m1 * this.Pt1.X + this.Pt1.Y);

                // If we're not going to treat these segments as lines, then the
                // matching point must lie on the line segments, i.e. within the
                // bounds of the line segment. If it doesn't, then we can't consider
                // this a match.
                if (!treatLineSegmentsAsLines && (!this.IsInBounds(pt) || !other.IsInBounds(pt)))
                {
                    return null;
                }

            }

            return pt;
        }
        /// <summary>
        /// Finds the closest distance between a point and this line segment or
        /// the line through this line segment.
        /// </summary>
        /// <param name="pt">A 2D point.</param>
        /// <param name="treatLineSegmentAsLine">If True, will return distance to closest
        /// point on the line through the line segment. If False, will return distance to the 
        /// closest point on the segment itself.</param>
        public decimal DistanceTo(Point2D pt, bool treatLineSegmentAsLine = false)
        {

            return pt.DistanceTo(this.ClosestPointTo(pt, treatLineSegmentAsLine));

        }
        /// <summary>
        /// Returns the point on the line segment closest to the given point. Can also
        /// return the closest point on the line through the line segment.
        /// </summary>
        /// <param name="pt">A 2D point.</param>
        /// <param name="treatLineSegmentAsLine">If True, will return closest point on 
        /// the line through the line segment even if it doesn't lie on the segment
        /// itself. If False, will find the closest point on the segment itself.</param>
        public Point2D ClosestPointTo(Point2D pt, bool treatLineSegmentAsLine = false)
        {

            _CheckIsValid();

            LineSeg2D l = default(LineSeg2D);
            Point2D ret = default(Point2D);

            if (this.IsHorizontal)
            {
                ret = new Point2D(pt.X, this.Pt1.Y);
            }
            else
            {
                l = FromPointSlope(pt, this.PerpendicularSlope);
                ret = this.GetIntersect(l, true).Value;
            }

            if (!treatLineSegmentAsLine && !this.IsInBounds(ret))
            {
                if (pt.DistanceTo(Pt1) < pt.DistanceTo(Pt2))
                {
                    ret = Pt1;
                }
                else
                {
                    ret = Pt2;
                }
            }

            return ret;

            //' Alt method using vector project
            //'   http://mathworld.wolfram.com/Point-LineDistance2-Dimensional.html
            //Dim v As Vector2D
            //Dim r As Vector2D
            //Dim proj As Vector2D
            //Dim ret As XYPoint

            //v = New Vector2D(Pt2.Y - Pt1.Y, -(Pt2.X - Pt1.X))
            //r = New Vector2D(Pt1.X - pt.X, Pt1.Y - pt.Y)
            //proj = r.ProjectOnto(v)
            //ret = pt + proj
            //' End alt method

            //' Alt method using vectors
            //'   http://www.gamedev.net/community/forums/topic.asp?topic_id=198199&whichpage=1&#1250842
            //' Not used because of round off problems that affect precision.
            //Dim d As Decimal
            //Dim t As Decimal
            //Dim aToB As Vector2D
            //Dim aToPt As Vector2D

            //aToB = (New Vector2D(Pt1, Pt2)).Normalize
            //aToPt = New Vector2D(Pt1, pt)
            //d = Me.Length
            //t = aToB.Dot(aToPt)

            //If Not treatLineSegmentAsLine Then
            //    If t < 0 Then
            //        ret = Pt1
            //    ElseIf t > d Then
            //        ret = Pt2
            //    Else
            //        ret = Pt1 + aToB * t
            //    End If
            //Else
            //    If Not (Pt1 + aToB * t).Equals(ret, 23) Then Stop
            //    ret = Pt1 + aToB * t
            //End If
            //' End alt method

        }
        public bool IsParallelTo(LineSeg2D other, int decimals = -1)
        {

            _CheckIsValid();


            if (decimals >= 0)
            {
                return this.Slope.RoundFromZero(decimals) == other.Slope.RoundFromZero(decimals);


            }
            else
            {
                return this.Slope == other.Slope;

            }

        }

        /// <summary>
        /// Gets two parallel line segments translated the given amount perpendicular to
        /// this line segment in either direction.
        /// </summary>
        /// <param name="distance">Distance from this line segment that the parallel lines should be.</param>
        /// <returns>Returns two line segments.</returns>
        public LineSeg2D[] GetParallels(decimal distance)
        {

            _CheckIsValid();

            LineSeg2D[] ret = new LineSeg2D[2];
            Vector2D v = default(Vector2D);

            v = new Vector2D(Pt2.Y - Pt1.Y, -(Pt2.X - Pt1.X));
            v.Magnitude = distance;

            ret[0] = this + v;
            ret[1] = this - v;

            return ret;

        }
        /// <summary>
        /// Of the two parallel line segments, returns the one that matches the
        /// X and Y conditions.
        /// </summary>
        /// <param name="distance">The distance that the parallel line segment should be from this line segment.</param>
        /// <param name="whichX">If > 0 then matches only if the X value for one parallel
        /// is greater than the other's at the origin. If &lt; 0 then matches only if the
        /// X value for one parallel is less than the other's. If 0, then X values ignored.</param>
        /// <param name="whichY">If > 0 then matches only if the Y value for one parallel
        /// is greater than the other's at the origin. If &lt; 0 then matches only if the
        /// Y value for one parallel is less than the other's. If 0, then Y values ignored.</param>
        public LineSeg2D GetParallel(decimal distance, int whichX, int whichY)
        {

            LineSeg2D[] parallels = GetParallels(distance);
            int matchX = -1;
            int matchY = -1;

            if (whichX == 0 && whichY == 0)
            {
                throw new Exception("Can't find parallel without specifying X and Y comparison method!");
            }

            whichX = Math.Sign(whichX);


            if (whichX != 0 && !this.IsHorizontal)
            {
                if (parallels[0].GetX(0) < parallels[1].GetX(0))
                {
                    if (whichX < 0)
                    {
                        matchX = 0;
                    }
                    else
                    {
                        matchX = 1;
                    }
                }
                else if (parallels[0].GetX(0) > parallels[1].GetX(0))
                {
                    if (whichX < 0)
                    {
                        matchX = 1;
                    }
                    else
                    {
                        matchX = 0;
                    }
                }

            }

            whichY = Math.Sign(whichY);


            if (whichY != 0 && !this.IsVertical)
            {
                if (parallels[0].GetY(0) < parallels[1].GetY(0))
                {
                    if (whichY < 0)
                    {
                        matchY = 0;
                    }
                    else
                    {
                        matchY = 1;
                    }
                }
                else if (parallels[0].GetY(0) > parallels[1].GetY(0))
                {
                    if (whichY < 0)
                    {
                        matchY = 1;
                    }
                    else
                    {
                        matchY = 0;
                    }
                }

            }

            if (matchX >= 0 && matchY >= 0)
            {
                if (matchX != matchY)
                {
                    throw new Exception("Couldn't find parallel matching conditions!");
                }
                else
                {
                    return parallels[matchX];
                }
            }
            else if (matchX >= 0 && whichY == 0)
            {
                return parallels[matchX];
            }
            else if (matchY >= 0 && whichX == 0)
            {
                return parallels[matchY];
            }
            else
            {
                throw new Exception("Couldn't find parallel matching conditions!");
            }

        }
        /// <summary>
        /// Gets a vector that points from Pt1 to Pt2 with the magnitude of the length of the segment.
        /// </summary>
        public Vector2D GetVectorP1toP2()
        {

            return Pt1.GetVectorTo(Pt2);

        }

        /// <summary>
        /// Rotates the line segment around the given point.
        /// </summary>
        /// <param name="rotationPoint">Point to rotate around.</param>
        /// <param name="degrees">Degrees to rotate. Positive rotates CCW and negative rotates CW.</param>
        public LineSeg2D Rotate(Point2D rotationPoint, decimal degrees)
        {

            Matrix2D m = new Matrix2D();

            m.RotateAt(rotationPoint, degrees, false);

            return m.Transform(this);

        }

        /// <summary>
        /// Returns a line segment from X = 0 to the given X using the slope / intersect
        /// to calculate the Y values.
        /// </summary>
        /// <param name="slope">The slope to use when calculating Y values.</param>
        /// <param name="intersect">The Y intersect to use for X = 0.</param>
        /// <param name="xDistance">The distance of the new segment in X.</param>
        public static LineSeg2D FromSlopeIntersect(decimal slope, decimal intersect, decimal xDistance = 1)
        {

            return FromPointSlope(new Point2D(0, intersect), slope, xDistance);

        }
        /// <summary>
        /// Returns a line segment with the first point at the given X and the second point 
        /// at the given X + xDistance.
        /// </summary>
        /// <param name="pt">The starting point of the line segment.</param>
        /// <param name="slope">The slope to use when calculating Y values.</param>
        /// <param name="xDistance">The distance fo the new segment in X.</param>
        public static LineSeg2D FromPointSlope(Point2D pt, decimal slope, decimal xDistance = 1)
        {

            LineSeg2D l = default(LineSeg2D);

            l.Pt1 = pt;
            l.Pt2 = new Point2D(pt.X + xDistance,
                                pt.Y + slope * xDistance);

            return l;

        }

        /// <summary>
        /// Gets the chamfer drop along a given angle for a given distance.
        /// </summary>
        /// <param name="chamferDistance">The length of the chamfer.</param>
        /// <param name="chamferAngle">The angle of the chamfer in degrees.</param>
        public static decimal GetChamferDrop(decimal chamferDistance, decimal chamferAngle)
        {

            return RightTriangle.GetSideFromOppAngleOppSide(chamferAngle, chamferDistance);

        }

        public string ToAutoCADCmd(bool asConstructionLine = false, bool linefeedTerminate = true)
        {

            _CheckIsValid();

            return string.Format("{0} {1},{2} {3},{4} {5}", asConstructionLine ? "_xline" : "_line", Pt1.X, Pt1.Y, Pt2.X, Pt2.Y, linefeedTerminate ? "\r\n" : " ");

        }
        public string ToDraftSightCmd(bool asConstructionLine = false, bool linefeedTerminate = true)
        {

            _CheckIsValid();

            return string.Format("{0} {1},{2} {3},{4} {5}", asConstructionLine ? "infiniteline" : "line", Pt1.X, Pt1.Y, Pt2.X, Pt2.Y, linefeedTerminate ? "\r\n" : " ");

        }

    }
}
