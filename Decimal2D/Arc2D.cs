using System;
using System.Diagnostics;

namespace DecimalMath
{
    [DebuggerDisplay("Center: (X = {Circle.X} Y = {Circle.Y})  Radius: {Circle.Radius}  Angle: {StartAngle} to {EndAngle}")]
    public struct Arc2D: ITransformable<Transform2D, Arc2D>
    {

        public Circle2D Circle;
        private decimal _startAngle;

        private decimal _endAngle;
        /// <summary>
        /// Creates and arc with the given circle and start/end angle.
        /// </summary>
        /// <param name="c">The circle that the arc lies on.</param>
        /// <param name="startAngle">Start angle.</param>
        /// <param name="endAngle">End angle.</param>
        public Arc2D(Circle2D c, decimal startAngle, decimal endAngle)
        {
            this.Circle = c;
            _startAngle = startAngle;
            _endAngle = endAngle;
        }
        /// <summary>
        /// Creates an arc centered at (0,0) with the given radius and start/end angle.
        /// </summary>
        /// <param name="radius">Radius of the arc.</param>
        /// <param name="startAngle">Start angle.</param>
        /// <param name="endAngle">End angle.</param>
        public Arc2D(decimal radius, decimal startAngle, decimal endAngle)
        {
            this.Circle = new Circle2D(0, 0, radius);
            _startAngle = startAngle;
            _endAngle = endAngle;
        }
        /// <summary>
        /// Creates an arc centered on the given point with the specified radius and start/end angle.
        /// </summary>
        /// <param name="center">Center of the arc.</param>
        /// <param name="radius">Radius of the arc.</param>
        /// <param name="startAngle">Start angle.</param>
        /// <param name="endAngle">End angle.</param>
        public Arc2D(Point2D center, decimal radius, decimal startAngle, decimal endAngle)
        {
            this.Circle = new Circle2D(center, radius);
            _startAngle = startAngle;
            _endAngle = endAngle;
        }
        /// <summary>
        /// Creates an arc on the given circle with the start angle projected through
        /// the start point and the end angle projected through the end point.
        /// </summary>
        /// <param name="c">Circle that the arc lies on.</param>
        /// <param name="startPoint">Point that start angle is projected through.</param>
        /// <param name="endPoint">Point that end angle is projected through.</param>

        public Arc2D(Circle2D c, Point2D startPoint, Point2D endPoint)
        {
            this.Circle = c;
            _startAngle = c.AngleThroughPoint(startPoint);
            _endAngle = c.AngleThroughPoint(endPoint);

        }
        /// <summary>
        /// Creates an arc with the given center, start, and end points. Performs a check
        /// at the given precision to make sure that both the start and end points are
        /// the same distance from the center, throwing an exception if they're not. Note
        /// that because radius is generated from start point, the resultant arc's start
        /// point will be more accurate than the end point.
        /// </summary>
        /// <param name="center">Center of the arc.</param>
        /// <param name="startPoint">Start point of the arc.</param>
        /// <param name="endPoint">Point that end angle is projected through.</param>
        /// <param name="decimals">Precision at which to check the distance of the endpoints to the center.
        /// A negative value will perform an exact check.</param>

        public Arc2D(Point2D center, Point2D startPoint, Point2D endPoint, int decimals = 15)
            : this(center, center.DistanceTo(startPoint), 0, 0)
        {


            if ((decimals < 0 && center.DistanceTo(endPoint) != Radius) || (decimals >= 0 && center.DistanceTo(endPoint).RoundFromZero(decimals) != Radius.RoundFromZero(decimals)))
            {
                throw new Exception("Can't create arc from center and endpoints because endpoints have different distances from the center!");

            }

            _startAngle = Circle.AngleThroughPoint(startPoint);
            _endAngle = Circle.AngleThroughPoint(endPoint);

        }
        /// <summary>
        /// Creates an arc based on another arc, but with a different radius.
        /// </summary>
        /// <param name="a">An arc from which to take the center and start/end angles.</param>
        /// <param name="radius">A radius for the new arc.</param>
        public Arc2D(Arc2D a, decimal radius)
        {
            Circle = new Circle2D(a.Center, radius);
            _startAngle = a._startAngle;
            _endAngle = a._endAngle;
        }

        /// <summary>
        /// Creates an arc using three points, two of which are endpoints and 
        /// one of which is designated as on the arc but not an endpoint.
        /// </summary>
        /// <param name="endPointA">One endpoint of the arc.</param>
        /// <param name="endPointB">One endpoint of the arc.</param>
        /// <param name="pointOnArc">A point on the arc which is not an endpoint.</param>
        /// <remarks></remarks>
        public static Arc2D FromPointsOnArc(Point2D endPointA, Point2D endPointB, Point2D pointOnArc)
        {

            Arc2D a = default(Arc2D);

            a.Circle = new Circle2D(endPointA, pointOnArc, endPointB);

            decimal angleA = 0m;
            decimal angleOnArc = 0m;
            decimal angleB = 0m;

            angleA = a.Circle.AngleThroughPoint(endPointA);
            angleOnArc = a.Circle.AngleThroughPoint(pointOnArc);
            angleB = a.Circle.AngleThroughPoint(endPointB);

            if (angleA == angleB)
                throw new Exception("End points are too close together or equal! Can't create arc.");
            if (angleOnArc == angleA || angleOnArc == angleB)
                throw new Exception("Point on arc is too close or equal to one of the end points! Can't create arc.");

            // We want angle B to be greater than angle A so that
            // if the angle on the arc is between them then A is
            // the start otherwise B is for certain the start.
            if (angleB < angleA)
            {
                Helper.Swap(ref angleA, ref angleB);
                Helper.Swap(ref endPointA, ref endPointB);
            }

            if (angleA < angleOnArc && angleOnArc < angleB)
            {
                a._startAngle = angleA;
                a._endAngle = angleB;
            }
            else
            {
                a._startAngle = angleB;
                a._endAngle = angleA;
            }

            return a;

        }

        public static Arc2D operator +(Arc2D arc, Vector2D vector)
        {
            return new Arc2D(arc.Circle + vector, arc._startAngle, arc._endAngle);
        }

        public static bool operator ==(Arc2D objA, Arc2D objB)
        {
            return objA.Circle == objB.Circle && objA._startAngle == objB._startAngle && objA._endAngle == objB._endAngle;
        }
        public static bool operator !=(Arc2D objA, Arc2D objB)
        {
            return objA.Circle != objB.Circle || objA._startAngle != objB._startAngle || objA._endAngle != objB._endAngle;
        }

        /// <summary>
        /// Gets or sets the center of the arc as an XY point.
        /// </summary>
        public Point2D Center
        {
            [DebuggerStepThrough()]
            get { return Circle.Center; }
            [DebuggerStepThrough()]
            set { Circle.Center = value; }
        }
        /// <summary> Radius of the arc. </summary>
        public decimal Radius
        {
            [DebuggerStepThrough()]
            get { return Circle.Radius; }
            [DebuggerStepThrough()]
            set { Circle.Radius = value; }
        }
        /// <summary>
        /// The start angle of the arc in degrees. Arc starts at this angle
        /// and extends counter-clockwise toward the end angle.
        /// Normalized to be >= 0 and &lt; 360.
        /// </summary>
        public decimal StartAngle
        {
            [DebuggerStepThrough()]
            get { return _startAngle; }
            [DebuggerStepThrough()]
            set { _startAngle = DecimalEx.NormalizeAngleDeg(value); }
        }
        /// <summary>
        /// The end angle of the arc in degrees. Arc starts at the start
        /// angle and extends counter-clockwise toward this angle.
        /// Normalized to be >= 0 and &lt; 360.
        /// </summary>
        public decimal EndAngle
        {
            [DebuggerStepThrough()]
            get { return _endAngle; }
            [DebuggerStepThrough()]
            set { _endAngle = DecimalEx.NormalizeAngleDeg(value); }
        }
        /// <summary>
        /// The angle in degrees that bisects the arc.
        /// </summary>
        public decimal BisectingAngle
        {

            get
            {
                if (_startAngle <= _endAngle)
                {
                    return (_startAngle + _endAngle) / 2m;
                }
                else
                {
                    return DecimalEx.NormalizeAngleDeg((_startAngle + (_endAngle + 360m)) / 2m);
                }

            }
        }
        /// <summary>
        /// Determines whether or not the given angle lies on the arc.
        /// </summary>
        /// <param name="angle">The angle in question.</param>
        public bool IsAngleOnArc(decimal angle)
        {

            angle = DecimalEx.NormalizeAngleDeg(angle);

            if (_startAngle <= _endAngle)
            {
                return (_startAngle <= angle) && (angle <= _endAngle);
            }
            else
            {
                return (angle > _startAngle) || (angle < _endAngle);
            }

        }

        /// <summary>
        /// Returns the sagitta (the distance between the highest point of
        /// the arc and the center of the chord) for a circle with the
        /// supplied radius the chord length.
        /// </summary>
        public decimal Sagitta
        {

            get
            {
                System.Diagnostics.Debugger.Break();
                // and test

                decimal ret = Circle2D.GetSagitta(Circle.Radius, GetChord().Length);

                if (CentralAngle > 180)
                    ret = 2 * Circle.Radius - ret;

                return ret;

            }
        }

        /// <summary>
        /// Gets the central angle of the arc, i.e. the total angle covered by the arc.
        /// </summary>
        /// <remarks>http://www.mathopenref.com/circlecentral.html</remarks>
        public decimal CentralAngle
        {
            get { return _endAngle - _startAngle; }
        }
        /// <summary>
        /// Gets the length of the arc.
        /// </summary>
        /// <remarks>http://www.mathopenref.com/arclength.html</remarks>
        public decimal ArcLength
        {
            get { return Circle.Radius * ((2m * DecimalEx.Pi * CentralAngle) / 360m); }
        }

        /// <summary>
        /// Gets or sets the point on the circle at the start angle. Note
        /// that when setting the point, the angle projected from the center
        /// of the arc through the specified point will be used if the point
        /// is not on the circle.
        /// </summary>
        public Point2D StartPt
        {
            get { return Circle.PointAt(_startAngle); }
            set { _startAngle = Circle.AngleThroughPoint(value); }
        }
        /// <summary>
        /// Gets or sets the point on the circle at the ending angle. Note
        /// that when setting the point, the angle projected from the center
        /// of the arc through the specified point will be used if the point
        /// is not on the circle.
        /// </summary>
        public Point2D EndPt
        {
            get { return Circle.PointAt(_endAngle); }
            set { _endAngle = Circle.AngleThroughPoint(value); }
        }
        /// <summary>
        /// Gets the middle point on the arc, i.e. the point on the circle
        /// at the bisecting angle.
        /// </summary>
        public Point2D MidPt
        {
            get { return Circle.PointAt(BisectingAngle); }
        }

        /// <summary>
        /// Gets a tangent line at the start angle.
        /// See <see cref="Circle2D.TangentAt"/>.
        /// </summary>
        public LineSeg2D TangentAtStart(decimal length, bool clockwise)
        {
            return Circle.TangentAt(_startAngle, length, clockwise);
        }
        /// <summary>
        /// Gets a tangent line at the end angle.
        /// See <see cref="Circle2D.TangentAt"/>.
        /// </summary>
        public LineSeg2D TangentAtEnd(decimal length, bool clockwise)
        {
            return Circle.TangentAt(_endAngle, length, clockwise);
        }

        /// <summary>
        /// Gets the chord where the first point is the start point of the arc and
        /// the second point is the end point of the arc.
        /// </summary>
        public LineSeg2D GetChord()
        {
            return new LineSeg2D(StartPt, EndPt);
        }

        public Arc2D GrowRadius(decimal amount)
        {

            return new Arc2D(Circle.Grow(amount), _startAngle, _endAngle);

        }
        public Arc2D ShrinkRadius(decimal amount)
        {

            return new Arc2D(Circle.Shrink(amount), _startAngle, _endAngle);

        }

        /// <summary>
        /// Attempts to transform this arc by applying the transform to the
        /// center, start, and end points. If the resulting points do not
        /// form an arc, for example if they are skewed, then an exception
        /// is thrown.
        /// </summary>
        public Arc2D Transform(Transform2D matrix)
        {
            var centPt = matrix.Transform(Center);
            var startPt = matrix.Transform(StartPt);
            var midPt = matrix.Transform(MidPt);
            var endPt = matrix.Transform(EndPt);

            var newA = new Arc2D(centPt, startPt, endPt);

            // Detect translations that would alter the shape so it's no longer an arc
            if (centPt.DistanceTo(midPt).RoundFromZero(15) != newA.Radius.RoundFromZero(15))
            {
                throw new Exception("Can't transform arc. Distance from transformed midpoint to center does not equal the radius of the arc!");
            }

            // Check if the start and end angles were flipped in the transform
            if (!newA.IsAngleOnArc(newA.Circle.AngleThroughPoint(midPt)))
            {
                var tmp = newA.StartAngle;
                newA.StartAngle = newA.EndAngle;
                newA.EndAngle = tmp;
            }

            return newA;
        }

        public static Arc2D FromLinesCircle(LineSeg2D line1, LineSeg2D line2, Circle2D c, int decimals = -1)
        {

            Point2D[] intersects1 = null;
            Point2D[] intersects2 = null;

            intersects1 = c.GetIntersect(line1, decimals);
            intersects2 = c.GetIntersect(line2, decimals);

            throw new NotImplementedException();
        }

        public string ToAutoCADCmd(bool linefeedTerminate = true)
        {

            return string.Format("_arc {0},{1} c {2},{3} {4},{5}{6}", StartPt.X, StartPt.Y, Circle.Center.X, Circle.Center.Y, EndPt.X, EndPt.Y, (linefeedTerminate ? "\r\n" : " "));

        }

    }
}
