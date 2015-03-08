using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MathExtensions.TwoD
{
    /// <summary>
    /// Circle Equation: (x - H) ^ 2 + (y - K) ^ 2 = R ^ 2 where (H,K) is the center of the circle.
    /// </summary>
    [DebuggerDisplay("Center: (X = {X} Y = {Y})  Radius: {Radius}")]
    public struct Circle2D
    {

        /// <summary> X component of the center of the circle. </summary>
        private Point2D _center;
        /// <summary> Radius of the circle. </summary>
        private decimal _radius;

        /// <summary>
        /// Creates a new instance with the center at the origin with the provided radius.
        /// </summary>
        /// <param name="radius">The radius of the circle.</param>
        public Circle2D(decimal radius)
        {
            _center = Point2D.Origin;
            _radius = 0m; // to satisfy compiler
            this.Radius = radius;
        }
        /// <summary>
        /// Creates a new instance with the center at (x, y) and a radius of r.
        /// </summary>
        /// <param name="x">The X component of the center.</param>
        /// <param name="y">The Y component of the center.</param>
        /// <param name="radius">The radius of the circle.</param>
        public Circle2D(decimal x, decimal y, decimal radius)
        {
            _center = new Point2D(x, y);
            _radius = 0m; // to satisfy compiler
            this.Radius = radius;
        }
        /// <summary>
        /// Creates a new instance with the center at the given point and a radius of r.
        /// </summary>
        /// <param name="center">The center of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        public Circle2D(Point2D center, decimal radius)
        {
            _center = center;
            _radius = 0m; // to satisfy compiler
            this.Radius = radius;
        }
        /// <summary>
        /// Creates a circle from three points.
        /// </summary>
        /// <param name="x1">A number.</param>
        /// <param name="y1">A number.</param>
        /// <param name="x2">A number.</param>
        /// <param name="y2">A number.</param>
        /// <param name="x3">A number.</param>
        /// <param name="y3">A number.</param>
        /// <remarks>See http://www.topcoder.com/tc?module=Static&d1=tutorials&d2=geometry2#circle</remarks>

        public Circle2D(decimal x1, decimal y1, decimal x2, decimal y2, decimal x3, decimal y3)
        {
            _radius = 0m;               // to satisfy compiler
            _center = default(Point2D); // to satisfy compiler

            if (Point2D.PointsAreColinear(x1, y1, x2, y2, x3, y3))
            {
                throw new Exception("Can't create circle! Points are colinear.");
            }

            LineSeg2D l1 = new LineSeg2D(x1, y1, x2, y2).PerpendicularBisector();
            LineSeg2D l2 = new LineSeg2D(x2, y2, x3, y3).PerpendicularBisector();
            Point2D? intersect = l1.GetIntersect(l2, true);

            if (intersect == null)
            {
                // Should never get null here because lines won't be parallel so 
                // long as they aren't colinear which we checked for above.
                throw new Exception("Something bad happened. Should have found an intersect so long as points are not colinear.");                
            }

            Center = intersect.Value;
            Radius = intersect.Value.DistanceTo(x1, y1);

        }
        /// <summary>
        /// Create a circle from three points.
        /// </summary>
        /// <param name="pt1">A point.</param>
        /// <param name="pt2">A point.</param>
        /// <param name="pt3">A point.</param>

        public Circle2D(Point2D pt1, Point2D pt2, Point2D pt3)
            : this(pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y)
        {

        }
        /// <summary>
        /// Create a circle from two points.
        /// </summary>
        /// <param name="pt1">A point.</param>
        /// <param name="pt2">A point.</param>

        public Circle2D(Point2D pt1, Point2D pt2)
        {
            _center = default(Point2D);
            _radius = 0m;

            LineSeg2D l = new LineSeg2D(pt1, pt2);

            if (l.IsZeroLength())
                throw new Exception("Can't create circle from two points that are the same point!");

            Center = l.MidPoint;
            Radius = l.Length / 2m;

            if (Radius == 0)
                throw new Exception("Can't create circle from two points that are the same point!");

        }

        public static Circle2D operator +(Circle2D circle, Vector2D vector)
        {
            return new Circle2D(circle.Center + vector, circle.Radius);
        }

        public static bool operator ==(Circle2D objA, Circle2D objB)
        {
            return objA.Center == objB.Center && objA._radius == objB._radius;
        }
        public static bool operator !=(Circle2D objA, Circle2D objB)
        {
            return objA.Center != objB.Center || objA._radius != objB._radius;
        }

        /// <summary>
        /// Returns a new circle with a radius equal to this circle's radius
        /// plus the amount specified. Negative amounts are allowed.
        /// </summary>
        /// <param name="amount">The amount to expand this circle.</param>
        public Circle2D Grow(decimal amount)
        {

            if (_radius + amount < 0)
            {
                throw new OverflowException("Shrinking this circle by the given amount results in a radius that is less than zero!");
            }

            return new Circle2D(_center, _radius + amount);

        }
        /// <summary>
        /// Returns a new circle with a radius equal to this circle's radius
        /// minus the amount specified. Negative amounts are allowed.
        /// </summary>
        /// <param name="amount">The amount to shrink this circle.</param>
        public Circle2D Shrink(decimal amount)
        {

            if (_radius - amount < 0)
            {
                throw new OverflowException("Shrinking this circle by the given amount results in a radius that is less than zero!");
            }

            return new Circle2D(_center, _radius - amount);

        }

        /// <summary>
        /// Returns circles that are tangent to two lines.
        /// </summary>
        /// <param name="lineA">First line.</param>
        /// <param name="lineB">Second line.</param>
        /// <param name="radius">Radius of the tangent circles.</param>
        /// <returns>Will return no circles if the two lines are parallel or the same line.</returns>
        [DebuggerStepThrough()]
        public static Circle2D[] FromTangentTangentRadius(LineSeg2D lineA, LineSeg2D lineB, decimal radius)
        {

            List<LineSeg2D> parallels = new List<LineSeg2D>();
            Nullable<Point2D> centerPoint = default(Nullable<Point2D>);
            List<Point2D> centerPoints = new List<Point2D>();
            List<Circle2D> tangentCircles = new List<Circle2D>();

            parallels.AddRange(lineA.GetParallels(radius));
            parallels.AddRange(lineB.GetParallels(radius));


            for (int i = 0; i <= parallels.Count - 2; i++)
            {

                for (int j = i + 1; j <= parallels.Count - 1; j++)
                {
                    centerPoint = parallels[i].GetIntersect(parallels[j], true);
                    if (centerPoint.HasValue)
                        centerPoints.Add(centerPoint.Value);

                }

            }


            for (int i = 0; i <= centerPoints.Count - 1; i++)
            {
                tangentCircles.Add(new Circle2D(centerPoints[i].X, centerPoints[i].Y, radius));

            }

            return tangentCircles.ToArray();

        }
        /// <summary>
        /// Returns circles that are tangent to another circle and line.
        /// </summary>
        /// <param name="circle">Circle that returned circles should be tangent to.</param>
        /// <param name="line">Line that returned circles should be tangent to.</param>
        /// <param name="radius">Radius of new circles.</param>
        /// <returns>External tangent circles are listed first.</returns>
        [DebuggerStepThrough()]
        public static Circle2D[] FromTangentTangentRadius(Circle2D circle, LineSeg2D line, decimal radius)
        {

            List<Circle2D> orbits = new List<Circle2D>();
            LineSeg2D[] parallels = new LineSeg2D[2];
            List<Point2D> centerPoints = new List<Point2D>();
            List<Circle2D> tangentCircles = new List<Circle2D>();

            orbits.Add(new Circle2D(circle.X, circle.Y, circle.Radius + radius));
            if (radius < circle.Radius)
            {
                orbits.Add(new Circle2D(circle.X, circle.Y, circle.Radius - radius));
            }

            parallels = line.GetParallels(radius);


            foreach (Circle2D orbit in orbits)
            {

                for (int i = 0; i <= 1; i++)
                {
                    centerPoints.AddRange(orbit.GetIntersect(parallels[i]));

                }

            }


            for (int i = 0; i <= centerPoints.Count - 1; i++)
            {
                tangentCircles.Add(new Circle2D(centerPoints[i].X, centerPoints[i].Y, radius));

            }

            return tangentCircles.ToArray();

        }
        /// <summary>
        /// Returns circles that are tangent to another circle and line.
        /// </summary>
        /// <param name="line">Line that returned circles should be tangent to.</param>
        /// <param name="circle">Circle that returned circles should be tangent to.</param>
        /// <param name="radius">Radius of new circles.</param>
        /// <returns>External tangent circles are listed first.</returns>
        [DebuggerStepThrough()]
        public static Circle2D[] FromTangentTangentRadius(LineSeg2D line, Circle2D circle, decimal radius)
        {

            return FromTangentTangentRadius(circle, line, radius);

        }
        /// <summary>
        /// Returns circles that are tangent to two other circles.
        /// </summary>
        /// <param name="circle1">Circle that returned circles should be tangent to.</param>
        /// <param name="circle2">Circle that returned circles should be tangent to.</param>
        /// <param name="radius">Radius of new circles.</param>
        /// <returns>External tangent circles are listed first.</returns>
        [DebuggerStepThrough()]
        public static Circle2D[] FromTangentTangentRadius(Circle2D circle1, Circle2D circle2, decimal radius)
        {

            Circle2D outerOrbit1 = default(Circle2D);
            Circle2D outerOrbit2 = default(Circle2D);
            Nullable<Circle2D> innerOrbit1 = null;
            Nullable<Circle2D> innerOrbit2 = null;
            List<Point2D> centerPoints = new List<Point2D>();
            List<Circle2D> tangentCircles = new List<Circle2D>();

            outerOrbit1 = new Circle2D(circle1.X, circle1.Y, circle1.Radius + radius);
            if (radius < circle1.Radius)
                innerOrbit1 = new Circle2D(circle1.X, circle1.Y, circle1.Radius - radius);

            outerOrbit2 = new Circle2D(circle2.X, circle2.Y, circle2.Radius + radius);
            if (radius < circle2.Radius)
                innerOrbit2 = new Circle2D(circle2.X, circle2.Y, circle2.Radius - radius);

            // Add intersections of outer orbits
            centerPoints.AddRange(outerOrbit1.GetIntersect(outerOrbit2));

            // Add intersections of outer orbit with the other's inner orbit
            if (innerOrbit2.HasValue)
                centerPoints.AddRange(outerOrbit1.GetIntersect(innerOrbit2.Value));
            if (innerOrbit1.HasValue)
                centerPoints.AddRange(outerOrbit2.GetIntersect(innerOrbit1.Value));

            // Add intersections of inner orbits

            if (innerOrbit1.HasValue && innerOrbit2.HasValue)
            {
                centerPoints.AddRange(innerOrbit1.Value.GetIntersect(innerOrbit2.Value));

            }

            // Now render center points to circles

            for (int i = 0; i <= centerPoints.Count - 1; i++)
            {
                tangentCircles.Add(new Circle2D(centerPoints[i].X, centerPoints[i].Y, radius));

            }

            return tangentCircles.ToArray();


        }

        public static Circle2D[] FromTwoPointsAndRadius(Point2D pt1, Point2D pt2, decimal radius)
        {

            return FromTwoPointsAndRadius(pt1.X, pt1.Y, pt2.X, pt2.Y, radius);

        }
        public static Circle2D[] FromTwoPointsAndRadius(decimal x1, decimal y1, decimal x2, decimal y2, decimal radius)
		{

			LineSeg2D pointToPoint = default(LineSeg2D);
			Point2D midPoint = default(Point2D);
			Vector2D vPerpBisector = default(Vector2D);

            if (x1 == x2 && y1 == y2) return new Circle2D[] { };

			pointToPoint = new LineSeg2D(x1, y1, x2, y2);
			midPoint = pointToPoint.MidPoint;
			vPerpBisector = pointToPoint.PerpendicularBisector().GetVectorP1toP2();
			vPerpBisector.Magnitude = RightTriangle.GetSideFromSideHyp(pointToPoint.Length / 2, radius);

			return new Circle2D[] {
				new Circle2D(midPoint + vPerpBisector, radius),
				new Circle2D(midPoint - vPerpBisector, radius)
			};

		}

        /// <summary>
        /// Returns the circle that is closest to the given points. Distance to
        /// each point is summed and the circle with the lowest distance is returned.
        /// If two circles are the same distance, the first one in the list is
        /// returned.
        /// </summary>
        /// <param name="circles">List of circles to check.</param>
        /// <param name="points">Point or points to use to calculate distance.</param>
        [DebuggerStepThrough()]
        public static Circle2D? ClosestCircle(Circle2D[] circles, params Point2D[] points)
        {
            if ((circles == null) || (circles.Length == 0))
                return null;

            decimal[] distance = new decimal[circles.Length];
            int lowest = 0;


            for (int i = 0; i <= circles.Length - 1; i++)
            {

                for (int j = 0; j <= points.Length - 1; j++)
                {
                    distance[i] += circles[i].DistanceTo(points[j]);

                }

            }

            lowest = 0;
            for (int i = 1; i <= circles.Length - 1; i++)
            {
                if (distance[i] < distance[lowest])
                    lowest = i;
            }

            return circles[lowest];

        }

        /// <summary> X component of the center of the circle. </summary>
        public decimal X
        {
            [DebuggerStepThrough()]
            get { return _center.X; }
            [DebuggerStepThrough()]
            set { _center.X = value; }
        }
        /// <summary> Y component of the center of the circle. </summary>
        public decimal Y
        {
            [DebuggerStepThrough()]
            get { return _center.Y; }
            [DebuggerStepThrough()]
            set { _center.Y = value; }
        }
        /// <summary> Radius of the circle. </summary>
        public decimal Radius
        {
            [DebuggerStepThrough()]
            get { return _radius; }
            [DebuggerStepThrough()]
            set
            {
                if (value < 0)
                    throw new OverflowException("Can't set radius of circle to less than zero! Use absolute value if necessary.");
                _radius = value;
            }
        }
        /// <summary>
        /// Gets or sets the center of the circle as an XY point.
        /// </summary>
        public Point2D Center
        {
            [DebuggerStepThrough()]
            get { return _center; }
            [DebuggerStepThrough()]
            set { _center = value; }
        }

        /// <summary>
        /// Returns distance to the given point from the circle. If the
        /// point is on the circle, distance is 0. If the point is inside
        /// the circle, the distance is expressed as a negative number
        /// whose absolute value is the distance from the outside of the circle.
        /// </summary>
        /// <param name="pt">The point to get distance to.</param>
        [DebuggerStepThrough()]
        public decimal DistanceTo(Point2D pt)
        {

            return Center.DistanceTo(pt) - Radius;

        }
        /// <summary>
        /// Returns distance to the given point from the circle. If the
        /// point is on the circle, distance is 0. If the point is inside
        /// the circle, the distance is expressed as a negative number
        /// whose absolute value is the distance from the outside of the circle.
        /// </summary>
        /// <param name="x">The x value of the point to get distance to.</param>
        /// <param name="y">The y value of the point to get distance to.</param>
        [DebuggerStepThrough()]
        public decimal DistanceTo(decimal x, decimal y)
        {

            return Center.DistanceTo(x, y) - Radius;

        }

        /// <summary>
        /// Gets the X values for which the equation of the circle yields the given Y.
        /// </summary>
        /// <param name="y">The Y value.</param>
        public decimal[] SolveForX(decimal y)
        {

            decimal aCoeff = default(decimal);
            decimal bCoeff = default(decimal);
            decimal cCoeff = default(decimal);

            // Solve for x
            //  (x - h)^2 + (y - k)^2 = r^2
            // simplifies to
            //  x^2 - 2 * h * x + h^2 + (y - k)^2 - r^2 = 0

            aCoeff = 1;
            bCoeff = -2 * _center.X;
            cCoeff = _center.X * _center.X + (y - _center.Y) * (y - _center.Y) - _radius * _radius;

            return MathExt.SolveQuadratic(aCoeff, bCoeff, cCoeff);

        }
        /// <summary>
        /// Gets the Y values for which the equation of the circle yields the given X.
        /// </summary>
        /// <param name="x">The X value.</param>
        public decimal[] SolveForY(ref decimal x)
        {

            // Alternative calculations. May be faster/more accurate, but need to be tested.
            //Dim absDiff = Math.Abs(x - _center.X)
            //Select Case absDiff
            //    Case Is > _radius
            //        Return New Decimal() {}
            //    Case _radius
            //        Return New Decimal() {_center.Y}
            //    Case 0
            //        Return New Decimal() {_center.Y + _radius, _center.Y - _radius}
            //    Case Else
            //        Dim height = RightTriangle.GetSideFromSideHyp(absDiff, _radius)
            //        Return New Decimal() {_center.Y + height, _center.Y - height}
            //End Select

            decimal aCoeff = default(decimal);
            decimal bCoeff = default(decimal);
            decimal cCoeff = default(decimal);

            // Solve for y
            //  (x - h)^2 + (y - k)^2 = r^2
            // simplifies to
            //  y^2 - 2 * k * y + k^2 + (x - h)^2 - r^2 = 0

            aCoeff = 1;
            bCoeff = -2 * _center.Y;
            cCoeff = _center.Y * _center.Y + (x - _center.X) * (x - _center.X) - _radius * _radius;

            return MathExt.SolveQuadratic(aCoeff, bCoeff, cCoeff);

        }
        /// <summary>
        /// Gets the points on the circle that have the given Y value.
        /// Like SolveForX but returning points.
        /// </summary>
        /// <param name="y">Y value.</param>
        public Point2D[] PointsForY(decimal y)
		{

			decimal[] xValues = SolveForX(y);

			switch (xValues.Length) {
				case 2:
					return new Point2D[] {
						new Point2D(xValues[0], y),
						new Point2D(xValues[1], y)
					};
				case 1:
					return new Point2D[] { new Point2D(xValues[0], y) };
				case 0:
			        return new Point2D[] { };
				default:
					throw new Exception("Unexpected number of points returned from SolveForX!");
			}

		}
        /// <summary>
        /// Gets the points on the circle that have the given X value.
        /// Like SolveForY but returning points.
        /// </summary>
        /// <param name="x">X value.</param>
        public Point2D[] PointsForX(decimal x)
		{

			decimal[] yValues = SolveForY(ref x);

			switch (yValues.Length) {
				case 2:
					return new Point2D[] {
						new Point2D(x, yValues[0]),
						new Point2D(x, yValues[1])
					};
				case 1:
					return new Point2D[] { new Point2D(x, yValues[0]) };
				case 0:
			        return new Point2D[] { };
				default:
					throw new Exception("Unexpected number of points returned from SolveForY!");
			}

		}

        /// <summary>
        /// Gets the coordinates of the point on the circle at the given angle.
        /// </summary>
        /// <param name="degrees">The angle in degrees.</param>
        public Point2D PointAt(decimal degrees)
        {

            decimal rad = MathExt.ToRad(degrees);

            return new Point2D(_center.X + _radius * MathExt.Cos(rad), _center.Y + _radius * MathExt.Sin(rad));

        }
        /// <summary>
        /// Gets a line segment of length 1 starting at the point on the circle
        /// and extending either clockwise or counterclockwise.
        /// </summary>
        /// <param name="degrees">An angle.</param>
        public LineSeg2D TangentAt(decimal degrees, decimal length, bool clockwise)
        {

            var r = RightTriangleAbstract.FromTwoSides(_radius, length);
            decimal rad = MathExt.ToRad(degrees + r.AngleA);
            LineSeg2D ret = default(LineSeg2D);

            Debug.Assert(r.AngleA == RightTriangle.GetAngleFromSides(length, _radius));
            Debug.Assert(r.Hypotenuse == RightTriangle.GetHypFromSides(length, _radius));

            ret.Pt1 = PointAt(degrees);
            if (!clockwise)
            {
                ret.Pt2 = new Point2D(_center.X + r.Hypotenuse * MathExt.Cos(rad), _center.Y + r.Hypotenuse * MathExt.Sin(rad));
            }
            else
            {
                ret.Pt2 = new Point2D(ret.Pt1.X - (_center.X + r.Hypotenuse * MathExt.Cos(rad) - ret.Pt1.X), ret.Pt1.Y - (_center.Y + r.Hypotenuse * MathExt.Sin(rad) - ret.Pt1.Y));
            }

            return ret;

        }
        /// <summary>
        /// Gets the two line segments that start at the given point and terminate
        /// on the circle and are tangent to the circle.
        /// </summary>
        /// <param name="point">The point for the tangent segments to go through.</param>
        /// <remarks>
        /// Got strategy from Wikipedia's description involving Thale's theorem.
        /// http://en.wikipedia.org/wiki/Tangent_lines_to_circles
        /// </remarks>
        public LineSeg2D[] TangentsThroughPoint(Point2D point)
        {

            Circle2D intersectingCircle = default(Circle2D);
            Point2D[] intersects = null;

            if (point.DistanceTo(this.Center) <= this.Radius)
                return null;

            intersectingCircle = new Circle2D(this.Center, point);

            intersects = this.GetIntersect(intersectingCircle);
            if (intersects.Length != 2)
                throw new Exception("Unexpected number of intersects when getting tangents through a point.");

            return new LineSeg2D[] {
				new LineSeg2D(point, intersects[0]),
				new LineSeg2D(point, intersects[1])
			};

        }

        /// <summary>
        /// Returns a line segment with the coordinates of a chord centered on
        /// the given angle with the given length.
        /// </summary>
        /// <param name="angleThroughCenter">The angle that passes through the
        /// center of the chord, in degrees.</param>
        /// <param name="chordLength">The length of the chord.</param>
        public LineSeg2D GetChord(decimal angleThroughCenter, decimal chordLength)
        {

            LineSeg2D l = default(LineSeg2D);
            decimal a = default(decimal);

            // Create a right triangle where one side is half the chord length,
            // the other side is from the center of the circle to the midpoint
            // of the chord, and the hypotenuse is from the center of the circle
            // to the chord start point
            a = RightTriangle.GetAngleFromOppSideHyp(0.5m * chordLength, _radius);

            l.Pt1 = PointAt(angleThroughCenter - a);
            l.Pt2 = PointAt(angleThroughCenter + a);

            return l;

        }
        /// <summary>
        /// Gets the total angle on a circle of the given radius from one side of the chord to the other.
        /// </summary>
        /// <param name="chordLength">The length of the chord.</param>
        /// <param name="circleRadius">The radius of the circle.</param>
        public static decimal GetChordTotalAngle(decimal chordLength, decimal circleRadius)
        {

            if (chordLength == 0)
            {
                return 0m;
            }
            else if (chordLength > circleRadius * 2m)
            {
                throw new Exception("Chord can't fit inside a circle with the specified radius!");
            }
            else if (chordLength == circleRadius * 2m)
            {
                return 180m;
            }
            else
            {
                return RightTriangle.GetAngleFromOppSideHyp(0.5m * chordLength, circleRadius) * 2m;
            }

        }
        /// <summary>
        /// Gets the total angle on this circle of the given radius from one side of the chord to the other.
        /// </summary>
        /// <param name="chordLength">The length of the chord.</param>
        public decimal GetChordTotalAngle(decimal chordLength)
        {

            return GetChordTotalAngle(chordLength, this.Radius);

        }

        /// <summary>
        /// Returns the sagitta (the distance between the highest point of
        /// an arc and the center of the chord) for the circle using
        /// the supplied chord length.
        /// </summary>
        /// <param name="chordLength">
        /// The distance between the two endpoints of the chord.
        /// </param>
        public decimal GetSagitta(decimal chordLength)
        {

            return GetSagitta(_radius, chordLength);

        }
        /// <summary>
        /// Returns the sagitta (the distance between the highest point of
        /// an arc and the center of the chord) for a circle with the
        /// supplied radius the chord length.
        /// </summary>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="chordLength">
        /// The distance between the two endpoints of the chord.
        /// </param>
        public static decimal GetSagitta(decimal radius, decimal chordLength)
        {

            // s = r - sqrt(r^2 - l^2)
            // where r = radius
            //       l = 1/2 chord length
            return radius - RightTriangle.GetSideFromSideHyp(chordLength / 2, radius);

        }
        /// <summary>
        /// Computes the radius of a circle given a chord length and the chord's sagitta.
        /// </summary>
        /// <param name="chordLength">Length of the chord.</param>
        /// <param name="sagitta">Sagitta.</param>
        /// <remarks> http://www.physicsforums.com/showpost.php?p=2069646&postcount=2 </remarks>
        public static decimal GetRadiusFromChordLengthSagitta(decimal chordLength, decimal sagitta)
        {

            return (MathExt.Pow(sagitta, 2) + 0.25m * MathExt.Pow(chordLength, 2)) / (2 * sagitta);

        }
        /// <summary>
        /// Gets the chord length for the sagitta and this circle's radius.
        /// </summary>
        public decimal GetChordLengthFromSagitta(decimal sagitta)
        {

            return GetChordLengthFromRadiusSagitta(Radius, sagitta);

        }
        /// <summary>
        /// Gets the chord length for the sagitta and radius.
        /// </summary>
        public static decimal GetChordLengthFromRadiusSagitta(decimal radius, decimal sagitta)
        {

            return 2 * RightTriangle.GetSideFromSideHyp(radius - sagitta, radius);

        }

        /// <summary>
        /// Gets the angle in degrees for a ray that extends from the center
        /// of the circle through the given point. An exception will occur,
        /// however, if the point specified is the same point as the center
        /// of the circle.
        /// </summary>
        /// <param name="pt">The point.</param>
        public decimal AngleThroughPoint(Point2D pt)
        {

            decimal a = default(decimal);

            // Shift point so it's relative to the origin.
            pt += new Vector2D(-Center.X, -Center.Y);

            // Do check here after we've translated the point
            // to account for small precision rounding.
            if (pt == Point2D.Origin)
            {
                throw new Exception("No angle through given point since point is at center of circle!");
            }

            // Special case for 0 and 180 where we would get
            // a divide by zero below.
            if (pt.Y == 0)
            {
                if (pt.X > 0)
                {
                    return 0;
                }
                else
                {
                    return 180;
                }
            }

            // Special case for 90 and 270 where we can't
            // determine quadrant below.
            if (pt.X == 0)
            {
                if (pt.Y > 0)
                {
                    return 90;
                }
                else
                {
                    return 270;
                }
            }

            a = MathExt.ToDeg(MathExt.ATan(pt.X / pt.Y));
            switch (pt.Quadrant)
            {
                case 1:
                case 2:
                    a = 90 - a;
                    break;
                case 3:
                case 4:
                    a = 270 - a;
                    break;
            }

            return a;

        }

        /// <summary>
        /// Gets the points of intersection between this circle and the line which
        /// contains the given line segment.
        /// </summary>
        /// <param name="l">Line segment used to determine line equation.</param>
        /// <param name="decimals">Determines rounding that should be performed when determining 
        /// if line intersection happens on zero, one, or two points. Default is -1 for 
        /// no rounding.</param>
        public Point2D[] GetIntersect(LineSeg2D l, int decimals = -1)
		{

			LineSeg2D centToLine = default(LineSeg2D);
			decimal centToLineLength = default(decimal);
			decimal centToLineLengthForComp = default(decimal);
			decimal radiusForComp = default(decimal);

			if (l.IsHorizontal) {
				// The center to any horizontal line is a vertical line through
				// the center of the circle.
				centToLine = new LineSeg2D(this.Center, this.Center + new Vector2D(0, 1));
			} else {
				centToLine = LineSeg2D.FromPointSlope(this.Center, l.PerpendicularSlope);
			}
			centToLine.Pt2 = l.GetIntersect(centToLine, true).Value;
			centToLineLength = centToLine.Length;

			// Get numbers for comparison, rounding if necessary
			centToLineLengthForComp = centToLineLength;
			radiusForComp = _radius;
			if (decimals >= 0) {
				centToLineLengthForComp = centToLineLengthForComp.RoundFromZero(decimals);
				radiusForComp = radiusForComp.RoundFromZero(decimals);
			}

			// See if line is outside of circle
			if (centToLineLengthForComp > radiusForComp)
			{
			    return new Point2D[] { };
			}

			// See if line is tangent to circle
			if (centToLineLengthForComp == radiusForComp) {
				return new Point2D[] { centToLine.Pt2 };
			}

			// Line must intersect in two places
			Vector2D vCentToChord = default(Vector2D);
			decimal halfChord = default(decimal);

			// Get a vector from the center to the intersecting chord
			vCentToChord = centToLine.GetVectorP1toP2();


			if (vCentToChord.Magnitude == 0) {
				Vector2D offsetVector = default(Vector2D);

				// Line goes through circle center


				if (l.IsVertical) {
					// Slope undefined so just go up the length of the radius
					offsetVector = new Vector2D(0, _radius);


				} else {
					offsetVector = new Vector2D(_radius * MathExt.Cos(MathExt.ATan(l.Slope)), _radius * MathExt.Sin(MathExt.ATan(l.Slope)));

				}

				return new Point2D[] {
					this.Center + offsetVector,
					this.Center - offsetVector
				};


			} else {
				Vector2D vChord = default(Vector2D);

				// Get a vector along the chord
				vChord = vCentToChord.GetPerpendicular();

				// Determine the length of half the chord
				halfChord = RightTriangle.GetSideFromSideHyp(centToLineLength, _radius);

				// Set the magnitude of the vector along the chord
				// to be half the chord length
				vChord.Magnitude = halfChord;

				// The two intersecting points are points translated
				// from the center of the circle to the chord (+vCentToChord)
				// and then translated to the ends of the chord (+-vChord)
				return new Point2D[] {
					this.Center + vCentToChord + vChord,
					this.Center + vCentToChord - vChord
				};

			}

		}
        /// <summary>
        /// Gets the points of intersection between this circle and the line which
        /// contains the given line segment.
        /// </summary>
        /// <param name="l">Line segment used to determine line equation.</param>
        public Point2D[] GetIntersectFast(ref LineSeg2D l)
		{

			decimal[] x = new decimal[2];
			decimal m = default(decimal);
			decimal b = default(decimal);
			decimal aCoeff = default(decimal);
			decimal bCoeff = default(decimal);
			decimal cCoeff = default(decimal);
			decimal lineX = default(decimal);
			decimal t = default(decimal);
			decimal p = default(decimal);
			decimal q = default(decimal);
			Point2D[] pts = null;



			if (!l.IsVertical) {
				// Circle    (x - h) ^ 2 + (y - k) ^ 2 = r ^ 2
				//   Center: (h, k)
				//   Radius: r
				// Line      y = m * x + b

				// (x - h) ^ 2 + (m * x + b - k) ^ 2 = r ^ 2
				// (x - h) * (x - h) + (m * x + b - k) * (m * x + b - k) = r^2
				// (x^2 - 2 * h * x + h^2) + (m^2 * x^2 + 2 * (b - k) * m * x + (b - k)^2 = r^2
				// (m^2 + 1) * x^2 + (2 * (b - k) * m - 2 * h) * x + (h^2 + (b - k)^2 - r^2) = 0

				m = l.Slope;
				b = l.YIntersect;

				aCoeff = MathExt.Pow(m, 2) + 1;
				bCoeff = 2 * (b - _center.Y) * m - 2 * _center.X;
				cCoeff = MathExt.Pow(_center.X, 2) + MathExt.Pow(b - _center.Y, 2) - MathExt.Pow(_radius, 2);

				x = MathExt.SolveQuadratic(aCoeff, bCoeff, cCoeff);

			    if (x.Length == 0) return new Point2D[] { };

                pts = new Point2D[x.Length];

				for (var i = 0; i <= x.Length - 1; i++) {
					pts[i].X = x[i];
					pts[i].Y = m * x[i] + b;
				}


			} else {
				// Circle    (x - h) ^ 2 + (y - k) ^ 2 = r ^ 2
				//   Center: (h, k)
				//   Radius: r
				// Line      x = lineX

				// Got the following from
				//  http://www.sonoma.edu/users/w/wilsonst/Papers/Geometry/circles/T1--2/T1-3-2.html
				//  http://www.sonoma.edu/users/w/wilsonst/Papers/Geometry/circles/default.html

				lineX = l.Pt1.X;
				t = _radius * _radius - (lineX - _center.X) * (lineX - _center.X);


				if (t < 0)
				{
				    return new Point2D[] { };


				} else {
					p = _center.Y + MathExt.Sqrt(t);
					q = _center.Y - MathExt.Sqrt(t);

                    pts = new Point2D[1];
					pts[0].Y = p;
					pts[0].X = lineX;

					// NOTE that P=Q when t=0
					if (p != q) {
						Array.Resize(ref pts, 2);
						pts[1].Y = q;
						pts[1].X = lineX;
					}

				}

			}

			return pts;

		}

        /// <summary>
        /// Gets the points of intersection between this circle and another circle.
        /// </summary>
        /// <param name="other">The other circle.</param>
        /// <param name="decimals">Determines rounding that should be performed when determining 
        /// if line intersection happens on zero, one, or two points. Default is -1 for 
        /// no rounding.</param>
        /// <param name="impreciseResult">Which value to return for an imprecise result, 
        /// i.e. tangency determined by rounding. If positive, returns the point on the larger circle. If
        /// negative, returns the point on the smaller circle. If 0, returns the midpoint between these
        /// two points.</param>
        public Point2D[] GetIntersect(Circle2D other, int decimals = -1, int impreciseResult = 0)
		{

			Vector2D meToOtherVector = default(Vector2D);

			meToOtherVector = this.Center.GetVectorTo(other.Center);


			if (decimals >= 0) {
				// The only intersection decision that can be made to a given precision is whether
				// the two circles are tangent to each other, either internally or externally.


				if (this.IsTangentTo(other, decimals)) {
					// If the smaller circle is inside the other, then the smaller is
					// considered internally tangent to the larger.

					if ((this.Radius < other.Radius && this.IsInside(other, decimals)) || (other.Radius < this.Radius && other.IsInside(this, decimals))) {
						// Internal tangent

						Point2D pointOnLargeCircle = default(Point2D);
						Point2D pointOnSmallCircle = default(Point2D);

						// Vectors to the two tangent points are both pointing in the same
						// direction--from the center of the larger circle towards the
						// center of the smaller circle. Their magnitude is the same as the
						// radius of the circle whose center they start from.


						if (this.Radius > other.Radius) {
							// Go from center of larger circle, Me, to smaller circle, other.
							pointOnLargeCircle = this.Center + new Vector2D(meToOtherVector, this.Radius);
							pointOnSmallCircle = other.Center + new Vector2D(meToOtherVector, other.Radius);


						} else {
							// Go from center of larger circle, other, to smaller circle, Me.
							pointOnLargeCircle = other.Center - new Vector2D(meToOtherVector, other.Radius);
							pointOnSmallCircle = this.Center - new Vector2D(meToOtherVector, this.Radius);

						}


						if (impreciseResult > 0) {
							// Point on larger
							return new Point2D[] { pointOnLargeCircle };


						} else if (impreciseResult < 0) {
							// Point on smaller
							return new Point2D[] { pointOnSmallCircle };


						} else {
							// Split difference
							LineSeg2D l = new LineSeg2D(pointOnLargeCircle, pointOnSmallCircle);
							return new Point2D[] { l.MidPoint };

						}


					} else {
						// External tangent

						Point2D pointOnMe = default(Point2D);
						Point2D pointOnOther = default(Point2D);

						// Vectors to the two tangent points are simply pointing at the other
						// circle's center with a magnitude of the radius of the circle whose
						// center it originates in.
						pointOnMe = this.Center + new Vector2D(meToOtherVector, this.Radius);
						pointOnOther = other.Center - new Vector2D(meToOtherVector, other.Radius);


						if (impreciseResult > 0) {
							// Point on larger
							return new Point2D[] { (this.Radius > other.Radius ? pointOnMe : pointOnOther) };


						} else if (impreciseResult < 0) {
							// Point on smaller
							return new Point2D[] { (this.Radius < other.Radius ? pointOnMe : pointOnOther) };


						} else {
							if (pointOnMe == pointOnOther) {
								return new Point2D[] { pointOnMe };
							} else {
								// Split difference
								return new Point2D[] { new LineSeg2D(pointOnMe, pointOnOther).MidPoint };
							}

						}

					}

				}

			}

			// Detect situations where two points touch
			decimal a = default(decimal);
			decimal h = default(decimal);
			decimal r2mina2 = default(decimal);
			Vector2D vToIntersectMidPt = default(Vector2D);
			Vector2D vToIntersect1 = default(Vector2D);

			// The following two equations are from:
			//   http://paulbourke.net/geometry/2circle/
			a = (MathExt.Pow(meToOtherVector.Magnitude, 2) - MathExt.Pow(other.Radius, 2) + MathExt.Pow(this.Radius, 2)) / (2 * meToOtherVector.Magnitude);

			r2mina2 = MathExt.Pow(this.Radius, 2) - MathExt.Pow(a, 2);

			// No intersection points -- one circle is inside or outside the other
            if (r2mina2 < 0) return new Point2D[] { };

			vToIntersectMidPt = new Vector2D(this.Center, other.Center);
			vToIntersectMidPt.Magnitude = a;


			if (r2mina2 == 0) {
				// Only one intersection point
				return new Point2D[] { this.Center + vToIntersectMidPt };

			}

			h = MathExt.Sqrt(r2mina2);
			vToIntersect1 = vToIntersectMidPt.GetPerpendicular();
			vToIntersect1.Magnitude = h;

			return new Point2D[] {
				this.Center + vToIntersectMidPt + vToIntersect1,
				this.Center + vToIntersectMidPt - vToIntersect1
			};

		}

        /// <summary>
        /// Determines if this circle is inside the other. Considers this circle inside
        /// if it is tangent on one point and otherwise inside.
        /// </summary>
        /// <param name="other">Other circle that may or may not contain this circle.</param>
        /// <param name="decimals">Decimal precision at which to make this comparison.</param>
        public bool IsInside(Circle2D other, int decimals = -1)
        {
            // TODO: Rename to GetIsInside

            if (decimals >= 0)
            {
                // Can't be inside the other if the radius is larger or equal.
                if (this.Radius.RoundFromZero(decimals) >= other.Radius.RoundFromZero(decimals))
                    return false;

                return (other.Center.DistanceTo(this.Center) + this.Radius).RoundFromZero(decimals) <= other.Radius.RoundFromZero(decimals);


            }
            else
            {
                // Can't be inside the other if the radius is larger or equal.
                if (this.Radius >= other.Radius)
                    return false;

                return other.Center.DistanceTo(this.Center) + this.Radius <= other.Radius;

            }

        }

        /// <summary>
        /// Determines if this circle is tangent to another.
        /// </summary>
        /// <param name="other">Other circle that may or may not be tangent to this circle.</param>
        /// <param name="decimals">Decimal precision at which to make this comparison.</param>
        public bool IsTangentTo(Circle2D other, int decimals = -1)
        {
            // TODO: Rename to GetIsTangentTo

            decimal minRadius = default(decimal);
            decimal maxRadius = default(decimal);


            if (decimals >= 0)
            {
                // Check for external tangency
                if (this.Center.DistanceTo(other.Center).RoundFromZero(decimals) == (this.Radius + other.Radius).RoundFromZero(decimals))
                    return true;

                // Internal tangency impossible
                if (this.Radius.RoundFromZero(decimals) == other.Radius.RoundFromZero(decimals))
                    return false;

                minRadius = Math.Min(this.Radius, other.Radius);
                maxRadius = Math.Max(this.Radius, other.Radius);

                return (this.Center.DistanceTo(other.Center) + minRadius).RoundFromZero(decimals) == maxRadius.RoundFromZero(decimals);


            }
            else
            {
                // Check for external tangency
                if (this.Center.DistanceTo(other.Center) == this.Radius + other.Radius)
                    return true;

                // Internal tangency impossible
                if (this.Radius == other.Radius)
                    return false;

                minRadius = Math.Min(this.Radius, other.Radius);
                maxRadius = Math.Max(this.Radius, other.Radius);

                return this.Center.DistanceTo(other.Center) + minRadius == maxRadius;

            }

        }

        public string ToAutoCADCmd(bool linefeedTerminate = true)
        {

            return string.Format("_circle {0},{1} {2}{3}", _center.X, _center.Y, _radius, (linefeedTerminate ? "\r\n" : " "));

        }

    }
}
