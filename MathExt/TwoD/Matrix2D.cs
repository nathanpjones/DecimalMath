using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathExtensions.TwoD
{
    /// <summary>
    /// Provides storage and transformation functions on a 3 x 3 matrix of
    /// an affine transformation.
    /// </summary>
    /// <remarks>
    /// Internal matrix organization is addressed by row and then column.
    /// In other words, matrix(0, 2) is first row and the last column.
    /// Points are represented as column vectors and consequently transformations 
    /// are applied by left-multiplying the transformation matrix (i.e.
    /// transform * existing matrix = new matrix). NOTE: GDI+ uses row vectors
    /// and right-multiplies. This other format was chosen because it is more
    /// often used in mathematics and computer science texts (and Wikipedia).
    /// </remarks>
    public class Matrix2D : MatrixBase<Matrix2D>
    {

        // Orientation (translation transformation shown)
        //
        // [ x ] [ 1 0 tx ]
        // [ y ] [ 0 1 ty ]
        // [ 1 ] [ 0 0 1  ]
        //
        // Transformations are applied by left-multiplying transformation matrix.


        public Matrix2D()
            : base(3)
        {

        }

        #region "        Actual Transformations    "

        /// <summary>
        /// Scales around the origin. Returns a reference to self.
        /// </summary>
        /// <param name="scaleX">Scale factor in X.</param>
        /// <param name="scaleY">Scale factor in Y.</param>
        public Matrix2D Scale(decimal scaleX, decimal scaleY)
        {

            decimal[,] r = new decimal[3, 3];

            //      0     1    2
            // 0    Sx    0    0
            // 1    0     Sy   0
            // 2    0     0    1

            r[0, 0] = scaleX;
            r[1, 1] = scaleY;
            r[2, 2] = 1;

            M = Multiply(r, M);

            return this;

        }
        /// <summary>
        /// Scale around a given point.  Returns a reference to self.
        /// </summary>
        /// <param name="x">The X coordinate of the point at which to scale from.</param>
        /// <param name="y">The Y coordinate of the point at which to scale from.</param>
        /// <param name="scaleX">Scale factor in X.</param>
        /// <param name="scaleY">Scale factor in Y.</param>
        public Matrix2D ScaleAt(decimal x, decimal y, decimal scaleX, decimal scaleY)
        {

            // Translate so (x,y) is now at origin, perform scaling, and then
            // translate so (x,y) is back at its original location
            return Translate(-x, -y).Scale(scaleX, scaleY).Translate(x, y);

        }
        /// <summary>
        /// Rotates about the origin. Returns a reference to self.
        /// </summary>
        /// <param name="degrees">The degrees to rotate.</param>
        /// <param name="clockwise">If False, then + degrees rotates counter clockwise.
        /// If True, then + degrees rotates clockwise. Of course, if the sign of the degrees
        /// is -, then the rotation will be opposite whatever the + direction is.</param>
        public Matrix2D Rotate(decimal degrees, bool clockwise = false)
        {

            decimal theta = 0;
            decimal[,] r = null;

            r = GetIdentityMatrix();

            theta = MathExt.ToRad(degrees);
            if (clockwise)
                theta *= -1;

            //      0     1    2
            // 0   cos  -sin   0
            // 1   sin   cos   0
            // 2    0     0    1

            r[0, 0] = MathExt.Cos(theta);
            r[0, 1] = -MathExt.Sin(theta);
            r[1, 0] = MathExt.Sin(theta);
            r[1, 1] = MathExt.Cos(theta);

            M = Multiply(r, M);

            return this;

        }
        /// <summary>
        /// Rotates about a given point. Returns a reference to self.
        /// </summary>
        /// <param name="x">The X coordinate of the point at which to rotate.</param>
        /// <param name="y">The Y coordinate of the point at which to rotate.</param>
        /// <param name="degrees">The degrees to rotate.</param>
        /// <param name="clockwise">If False, then + degrees rotates counter clockwise.
        /// If True, then + degrees rotates clockwise. Of course, if the sign of the degrees
        /// is -, then the rotation will be opposite whatever the + direction is.</param>
        public Matrix2D RotateAt(decimal x, decimal y, decimal degrees, bool clockwise = false)
        {

            // Translate so (x,y) is now at origin, perform rotation, and then
            // translate so (x,y) is back at its original location.
            return Translate(-x, -y).Rotate(degrees, clockwise).Translate(x, y);

        }
        /// <summary>
        /// Rotates about a given point. Returns a reference to self.
        /// </summary>
        /// <param name="pt">The point at which to rotate.</param>
        /// <param name="degrees">The degrees to rotate.</param>
        /// <param name="clockwise">If False, then + degrees rotates counter clockwise.
        /// If True, then + degrees rotates clockwise. Of course, if the sign of the degrees
        /// is -, then the rotation will be opposite whatever the + direction is.</param>
        public Matrix2D RotateAt(Point2D pt, decimal degrees, bool clockwise = false)
        {

            return RotateAt(pt.X, pt.Y, degrees, clockwise);

        }
        /// <summary>
        /// Mirrors across line through line segment. Returns a reference to self.
        /// </summary>
        /// <param name="l">The across which to mirror.</param>
        public Matrix2D Mirror(LineSeg2D l)
        {

            // See here: http://planetmath.org/encyclopedia/DerivationOf2DReflectionMatrix.html

            // Translate to origin because mirroring around a vector is relative to the origin.
            Translate(-l.Pt1.X, -l.Pt1.Y);

            var v = l.GetVectorP1toP2().Normalize();

            //         0           1       2
            // 0   x^2 - y^2      2xy      0
            // 1      2xy      y^2 - x^2   0
            // 2       0           0       1

            var r = new decimal[,]
                    {
                        { MathExt.Pow(v.X, 2) - MathExt.Pow(v.Y, 2), 2 * v.X * v.Y, 0 },
                        { 2 * v.X * v.Y, MathExt.Pow(v.Y, 2) - MathExt.Pow(v.X, 2), 0 },
                        { 0, 0, 1 }
                    };

            M = Multiply(r, M);

            // Translate back where we came from
            Translate(l.Pt1.X, l.Pt1.Y);

            return this;

        }
        /// <summary>
        /// Mirrors across line that passes through the given points. Returns a reference to self.
        /// </summary>
        /// <param name="pt1">A point.</param>
        /// <param name="pt2">A point.</param>
        public Matrix2D Mirror(Point2D pt1, Point2D pt2)
        {

            return Mirror(new LineSeg2D(pt1, pt2));

        }

        /// <summary>
        /// Translates by the given amounts in X and Y. For example, translating
        /// a point from (0,0) by X = 1, Y = 2 will yield (1,2).  Returns a reference to self.
        /// </summary>
        /// <param name="x">The X distance to translate.</param>
        /// <param name="y">The Y distance to translate.</param>
        public Matrix2D Translate(decimal x, decimal y)
        {

            decimal[,] t = null;

            t = GetIdentityMatrix();

            //      0     1    2
            // 0    1     0    x
            // 1    0     1    y
            // 2    0     0    1

            t[0, 2] = x;
            t[1, 2] = y;

            M = Multiply(t, M);

            return this;

        }

        #endregion

        #region "        Applying To Objects    "

        /// <summary>
        /// Transforms a geometric element. Must be a supported element (see
        /// strongly typed overloads).
        /// </summary>
        /// <param name="obj">The element to transform.</param>
        /// <remarks>
        /// NOTE: If you add a public Transform routine, make sure its type is added to
        /// this generic Transform routine.
        /// </remarks>
        public override object Transform(object obj)
        {
            if (obj is Point2D)
            {
                return Transform((Point2D)obj);
            }
            else if (obj is LineSeg2D)
            {
                return Transform((LineSeg2D)obj);
            }
            else if (obj is Vector2D)
            {
                return Transform((Vector2D)obj);
            }
            else if (obj is Arc2D)
            {
                return Transform((Arc2D)obj);
            }
            else
            {
                throw new ArgumentException("No routine is registered to transform type " + obj.GetType().Name);
            }
        }

        /// <summary>
        /// Transforms a point using the matrix and returning a new point.
        /// </summary>
        /// <param name="pt">The point to transform.</param>
        public Point2D Transform(Point2D pt)
        {

            decimal[] transformed = null;
            decimal[] m = {
                pt.X,
                pt.Y,
                1
            };

            transformed = Multiply(m, M);

            pt.X = transformed[0];
            pt.Y = transformed[1];

            return pt;

        }
        /// <summary>
        /// Transforms a line segment using the matrix and returning a new line segment.
        /// </summary>
        /// <param name="l">The line segment to transform.</param>
        public LineSeg2D Transform(LineSeg2D l)
        {

            l.Pt1 = Transform(l.Pt1);
            l.Pt2 = Transform(l.Pt2);

            return l;

        }
        /// <summary>
        /// Transforms a vector as a line segment starting at the origin and 
        /// ending at the (X,Y) of the vector elements. Returns a new vector
        /// translated back to the origin.
        /// </summary>
        /// <param name="v">The vector to transform.</param>
        public Vector2D Transform(Vector2D v)
        {

            Point2D basePt = Point2D.Origin;
            Point2D endPt = new Point2D(v.X, v.Y);

            basePt = Transform(basePt);
            endPt = Transform(endPt);

            return new Vector2D(basePt, endPt);

        }
        /// <summary>
        /// Attempts to transform an arc by applying the transform to the
        /// center, start, and end points. If the resulting points do not
        /// form an arc, for example if they are skewed, then an exception
        /// is thrown.
        /// </summary>
        /// <param name="a">The arc to transform.</param>
        public Arc2D Transform(Arc2D a)
        {

            var centPt = Transform(a.Center);
            var startPt = Transform(a.StartPt);
            var midPt = Transform(a.MidPt);
            var endPt = Transform(a.EndPt);

            var newA = new Arc2D(centPt, startPt, endPt);

            if (centPt.DistanceTo(midPt).RoundFromZero(15) != newA.Radius.RoundFromZero(15))
            {
                throw new Exception("Can't transform arc from distance from transformed midpoint to center does not equal the radius of the arc!");
            }

            // Make sure we haven't flipped the start and end angles in the transform
            if (!newA.IsAngleOnArc(newA.Circle.AngleThroughPoint(midPt)))
            {
                var tmp = newA.StartAngle;
                newA.StartAngle = newA.EndAngle;
                newA.EndAngle = tmp;
            }

            return newA;

        }

        #endregion

    }
}
