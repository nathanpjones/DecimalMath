using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecimalEx
{
    /// <summary>
    /// Represents a right triangle without specifying the location of any of its points.
    /// </summary>
    /// <remarks>
    /// See http://mathworld.wolfram.com/RightTriangle.html
    /// </remarks>
    public struct RightTriangleAbstract
    {

        private decimal _lengthA;
        private decimal _angleA;
        private decimal _lengthB;
        private decimal _angleB;

        private decimal _hypotenuse;

        /// <summary>
        /// Creates a right triangle from its two sides.
        /// </summary>
        /// <param name="lengthA">Length of side A.</param>
        /// <param name="lengthB">Length of side B.</param>
        public static RightTriangleAbstract FromTwoSides(decimal lengthA, decimal lengthB)
        {
            var t = default(RightTriangleAbstract);

            t._FromTwoSides(lengthA, lengthB);

            return t;
        }

        private void _FromTwoSides(decimal lengthA, decimal lengthB)
        {
            _lengthA = lengthA;
            _lengthB = lengthB;
            _hypotenuse = RightTriangle.GetHypFromSides(lengthA, lengthB);
            _angleA = RightTriangle.GetAngleFromSides(lengthB, lengthA);
            _angleB = RightTriangle.GetAngleFromSides(lengthA, lengthB);

        }
        /// <summary>
        /// Creates a right triangle from one side and the hypotenuse.
        /// </summary>
        /// <param name="lengthA">Length of side A.</param>
        /// <param name="hypotenuse">Length of the hypotenuse.</param>
        public static object FromSideAHypotenuse(decimal lengthA, decimal hypotenuse)
        {

            RightTriangleAbstract t = default(RightTriangleAbstract);

            t._FromSideAHypotenuse(lengthA, hypotenuse);

            return t;

        }

        private void _FromSideAHypotenuse(decimal lengthA, decimal hypotenuse)
        {
            _lengthA = lengthA;
            _hypotenuse = hypotenuse;
            _lengthB = RightTriangle.GetSideFromSideHyp(lengthA, hypotenuse);
            _angleA = RightTriangle.GetAngleFromAdjSideHyp(lengthA, hypotenuse);
            _angleB = RightTriangle.GetAngleFromOppSideHyp(lengthA, hypotenuse);

        }
        /// <summary>
        /// Creates a right triangle from one side and its adjacent angle in degrees.
        /// </summary>
        /// <param name="lengthA">Length of side A.</param>
        /// <param name="angleA">Angle adjacent to side A in degrees.</param>
        public static object FromSideAAngleA(decimal lengthA, decimal angleA)
        {

            RightTriangleAbstract t = default(RightTriangleAbstract);

            t._FromSideAAngleA(lengthA, angleA);

            return t;

        }

        private void _FromSideAAngleA(decimal lengthA, decimal angleA)
        {
            _lengthA = lengthA;
            _angleA = angleA;
            _lengthB = RightTriangle.GetSideFromOppAngleOppSide(angleA, lengthA);
            _angleB = RightTriangle.GetAngleFromOtherAngle(angleA);
            _hypotenuse = RightTriangle.GetHypFromSideAdjAngle(lengthA, angleA);

            // tan(A) = LB / LA
            // LB = tan(A) * LA
            _lengthB = DecimalEx.Tan(DecimalEx.ToRad(angleA)) * lengthA;
            _angleB = 90m - angleA;

            // cos(A) = LA / H
            // H = LA / cos(A)
            _hypotenuse = lengthA / DecimalEx.Cos(DecimalEx.ToRad(angleA));

        }

        private void _FromSideBAngleB(decimal lengthB, decimal angleB)
        {
            _lengthB = lengthB;
            _angleB = angleB;

            // tan(B) = LA / LB
            // LA = tan(B) * LB
            _lengthA = DecimalEx.Tan(DecimalEx.ToRad(angleB)) * lengthB;
            _angleA = 90m - angleB;

            // cos(B) = LB / H
            // H = LB / cos(B)
            _hypotenuse = lengthB / DecimalEx.Cos(angleB);

        }
        /// <summary>
        /// Gets or sets the angle adjacent to side A. Angle is in degrees.
        /// </summary>
        public decimal AngleA
        {
            get { return _angleA; }
            set { _FromSideAAngleA(_lengthA, value); }
        }
        /// <summary>
        /// Gets or sets the angle adjacent to side B. Angle is in degrees.
        /// </summary>
        public decimal AngleB
        {
            get { return _angleB; }
            set { _FromSideBAngleB(_lengthB, value); }
        }
        /// <summary>
        /// Gets or sets the length of the hypotenuse.
        /// </summary>
        public decimal Hypotenuse
        {
            get { return _hypotenuse; }
            set { _hypotenuse = value; }
        }
        /// <summary>
        /// Gets or sets the length of side A.
        /// </summary>
        public decimal LengthA
        {
            get { return _lengthA; }
            set { _FromSideAAngleA(value, _angleA); }
        }
        /// <summary>
        /// Gets or sets the length of side B.
        /// </summary>
        public decimal LengthB
        {
            get { return _lengthB; }
            set { _FromSideBAngleB(value, _angleB); }
        }

    }

}
