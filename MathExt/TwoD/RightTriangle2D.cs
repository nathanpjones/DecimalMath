using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathExtensions.TwoD
{
    [Obsolete("Not fully implemented yet!", true)]
    public struct RightTriangle2D
    {

        public enum SideRelationToAngle
        {
            Opposite,
            Adjacent,
            Hypotenuse
        }

        private Point2D _RightAnglePt;
        private Point2D _A;
        private Point2D _B;

        /// <summary>
        /// Creates a new right triangle from three points.
        /// </summary>
        /// <param name="rightAnglePt">The point opposite the hypotenuse.</param>
        /// <param name="a">One of the points on the hypotenuse.</param>
        /// <param name="b">One of the points on the hypotenuse.</param>
        /// <param name="degreePrecision">Precision at which a 90 degree angle between the
        /// sides of the triangle is checked. The angle is rounded to this many decimal
        /// places and compared to 90.</param>

        public RightTriangle2D(Point2D rightAnglePt, Point2D a, Point2D b, int degreePrecision = 1)
        {
            Vector2D v1 = default(Vector2D);
            Vector2D v2 = default(Vector2D);

            v1 = new Vector2D(rightAnglePt, a);
            v2 = new Vector2D(rightAnglePt, b);
            if (v1.AngleTo(v2).RoundFromZero(degreePrecision) != 90)
            {
                throw new Exception("Points do not form a right triangle!");
            }

            _RightAnglePt = rightAnglePt;
            _A = a;
            _B = b;

        }

        public RightTriangle2D(Point2D rightAnglePt, Point2D anglePt, decimal angle, decimal sideLength, SideRelationToAngle relation)
        {
            _RightAnglePt = rightAnglePt;
            _A = anglePt;
            _B = new Point2D(); // TODO: Is this appropriate?

            switch (relation)
            {
                case SideRelationToAngle.Opposite:
                    break;
                case SideRelationToAngle.Adjacent:
                    break;
                case SideRelationToAngle.Hypotenuse:
                    break;
                default:
                    System.Diagnostics.Debugger.Break();
                    break;
            }

        }

        public Point2D A
        {
            get { return _A; }
        }
        public decimal ALength
        {


            get { return _RightAnglePt.DistanceTo(_A); }

            set
            {
                Vector2D v = new Vector2D(_RightAnglePt, _A);

                v.Magnitude = value;

                _A = _RightAnglePt + v;

            }
        }
        public Point2D B
        {
            get { return _B; }
        }
        public decimal BLength
        {


            get { return _RightAnglePt.DistanceTo(_B); }

            set
            {
                Vector2D v = new Vector2D(_RightAnglePt, _B);

                v.Magnitude = value;

                _B = _RightAnglePt + v;

            }
        }
        public Point2D RightAnglePt
        {
            get { return _RightAnglePt; }
        }

    }

}
