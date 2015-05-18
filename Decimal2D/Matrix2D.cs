namespace DecimalMath
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
        { }
        public Matrix2D(decimal[,] values)
            : base(3, values)
        { }

        #region "        Actual Transformations    "

        /// <summary>
        /// Scales around the origin. Returns a reference to self.
        /// </summary>
        /// <param name="scaleX">Scale factor in X.</param>
        /// <param name="scaleY">Scale factor in Y.</param>
        public Matrix2D Scale(decimal scaleX, decimal scaleY)
        {

            var r = new Matrix2D();

            //      0     1    2
            // 0    Sx    0    0
            // 1    0     Sy   0
            // 2    0     0    1

            r[0, 0] = scaleX;
            r[1, 1] = scaleY;
            r[2, 2] = 1;

            return r.Multiply(this);

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
            var r = new Matrix2D();

            var theta = DecimalEx.ToRad(degrees);
            if (clockwise) theta *= -1;

            //      0     1    2
            // 0   cos  -sin   0
            // 1   sin   cos   0
            // 2    0     0    1

            r[0, 0] = DecimalEx.Cos(theta);
            r[0, 1] = -DecimalEx.Sin(theta);
            r[1, 0] = DecimalEx.Sin(theta);
            r[1, 1] = DecimalEx.Cos(theta);

            return r.Multiply(this);
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

            
            // Mirror around origin
            //
            //         0           1       2
            // 0   x^2 - y^2      2xy      0
            // 1      2xy      y^2 - x^2   0
            // 2       0           0       1

            var v = l.GetVectorP1toP2().Normalize();
            var mirror = new Matrix2D(
                new[,]
                {
                    { DecimalEx.Pow(v.X, 2) - DecimalEx.Pow(v.Y, 2), 2 * v.X * v.Y, 0 },
                    { 2 * v.X * v.Y, DecimalEx.Pow(v.Y, 2) - DecimalEx.Pow(v.X, 2), 0 },
                    { 0, 0, 1 }
                });

            // Translate to origin because mirroring around a vector is relative to the origin.
            var r = Translate(-l.Pt1.X, -l.Pt1.Y);
            
            // Left multiply this transformation matrix
            r = mirror.Multiply(r);

            // Translate back where we came from
            r = r.Translate(l.Pt1.X, l.Pt1.Y);

            return r;
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
            var t = new Matrix2D();

            //      0     1    2
            // 0    1     0    x
            // 1    0     1    y
            // 2    0     0    1

            t[0, 2] = x;
            t[1, 2] = y;

            return t.Multiply(this);
        }

        #endregion
    }
}
