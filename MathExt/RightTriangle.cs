using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathExtensions
{
    public class RightTriangle
    {

        private RightTriangle()
        {
            // No using instance...
        }

        /// <summary>
        /// Gets the hypotenuse from the two other sides.
        /// </summary>
        /// <param name="sideA">Length of one side.</param>
        /// <param name="sideB">Length of other side.</param>
        public static decimal GetHypFromSides(decimal sideA, decimal sideB)
        {
            // a^2 + b^2 = c^2
            // c = sqrt(a^2 + b^2)
            return MathExt.Sqrt(sideA * sideA + sideB * sideB);
        }
        /// <summary>
        /// Gets hypotenuse from a known side and the angle adjacent to that side.
        /// </summary>
        /// <param name="side">Length of the known side.</param>
        /// <param name="angleAdjacentToSide">The angle adjacent to the known side in degrees.</param>
        public static decimal GetHypFromSideAdjAngle(decimal side, decimal angleAdjacentToSide)
        {
            // cos(a) = s / h
            // h * cos(a) = s
            // h = s / cos(a)
            return side / MathExt.Cos(MathExt.ToRad(angleAdjacentToSide));
        }
        /// <summary>
        /// Gets hypotenuse from a known side and the angle opposite to it.
        /// </summary>
        /// <param name="side">Length of the known side.</param>
        /// <param name="angleOppositeToSide">Angle opposite to the known side in degrees.</param>
        public static decimal GetHypFromSideOppAngle(decimal side, decimal angleOppositeToSide)
        {
            // sin(a) = s / h
            // h = s / sin(a)
            return side / MathExt.Sin(MathExt.ToRad(angleOppositeToSide));
        }

        /// <summary>
        /// Gets a side from a known side and the hypotenuse.
        /// </summary>
        /// <param name="side">Length of the known side.</param>
        /// <param name="hypotenuse">Length of the hypotenuse.</param>
        public static decimal GetSideFromSideHyp(decimal side, decimal hypotenuse)
        {
            // a^2 + b^2 = c^2
            // a^2 = c^2 - b^2
            // a = sqrt(c^2 - b^2)
            decimal sideSquared = hypotenuse * hypotenuse - side * side;
            if (sideSquared < 0)
            {
                throw new Exception("Error finding side from other side and hypotenuse! Side and hypotenuse swapped or invalid right triangle.");
            }
            return MathExt.Sqrt(sideSquared);
        }
        /// <summary>
        /// Gets a side from the adjacent angle and the length of the opposite side.
        /// </summary>
        /// <param name="adjacentAngle">Angle adjacent to the side to calculate in degrees.</param>
        /// <param name="oppositeSide">Length of the opposite side.</param>
        public static decimal GetSideFromAdjAngleOppSide(decimal adjacentAngle, decimal oppositeSide)
        {
            // tan(adjacentAngle) = oppositeSide / x
            // x = oppositeSide / tan(adjacentAngle)
            return oppositeSide / Convert.ToDecimal(MathExt.Tan(MathExt.ToRad(adjacentAngle)));
        }
        /// <summary>
        /// Gets a side from the opposite angle and the length of the opposite side.
        /// </summary>
        /// <param name="oppositeAngle">Angle opposite the side to calculate in degrees.</param>
        /// <param name="oppositeSide">Length of the opposite side.</param>
        public static decimal GetSideFromOppAngleOppSide(decimal oppositeAngle, decimal oppositeSide)
        {
            // tan(oppositeAngle) = x / oppositeSide
            // x = oppositeSide * tan(oppositeAngle)
            return oppositeSide * Convert.ToDecimal(MathExt.Tan(MathExt.ToRad(oppositeAngle)));
        }
        /// <summary>
        /// Gets a side from the adjacent angle and the length of the hypotenuse.
        /// </summary>
        /// <param name="adjacentAngle">Angle adjacent to the side to calculate in degrees.</param>
        /// <param name="hypotenuse">Length of the hypotenuse.</param>
        public static decimal GetSideFromAdjAngleHyp(decimal adjacentAngle, decimal hypotenuse)
        {
            // cos(adjacentAngle) = x / hypotenuse
            // x = hypotenuse * cos(adjacentAngle)
            return hypotenuse * Convert.ToDecimal(MathExt.Cos(MathExt.ToRad(adjacentAngle)));
        }
        /// <summary>
        /// Gets a side from the opposite angle and the length of the hypotenuse.
        /// </summary>
        /// <param name="oppositeAngle">Angle opposite to the side to calculate in degrees.</param>
        /// <param name="hypotenuse">Length of the hypotenuse.</param>
        public static decimal GetSideFromOppAngleHyp(decimal oppositeAngle, decimal hypotenuse)
        {
            // sin(oppositeAngle) = x / hypotenuse
            // x = hypotenuse * sin(oppositeAngle)
            return hypotenuse * Convert.ToDecimal(MathExt.Sin(MathExt.ToRad(oppositeAngle)));
        }

        /// <summary>
        /// Gets angle in degrees from a known angle (other than the 90 degree angle).
        /// </summary>
        /// <param name="otherAngle">Known angle in degrees (not the 90 degree angle).</param>
        public static decimal GetAngleFromOtherAngle(decimal otherAngle)
        {
            return 90m - otherAngle;
        }
        /// <summary>
        /// Gets angle in degrees from the two known sides.
        /// </summary>
        /// <param name="oppositeSide">Length of the known side opposite the angle.</param>
        /// <param name="adjacentSide">Length of the known side adjacent to the angle.</param>
        public static decimal GetAngleFromSides(decimal oppositeSide, decimal adjacentSide)
        {
            // tan(a) = opposideSide / adjacentSide
            // a = atan(opposideSide / adjacentSide)
            return MathExt.ToDeg(MathExt.ATan(oppositeSide / adjacentSide));
        }
        /// <summary>
        /// Gets angle in degrees from the opposite side and the hypotenuse.
        /// </summary>
        /// <param name="oppositeSide">Length of the known side opposite the angle.</param>
        /// <param name="hypotenuse">Length of the hypotenuse.</param>
        public static decimal GetAngleFromOppSideHyp(decimal oppositeSide, decimal hypotenuse)
        {
            // sin(a) = oppositeSide / hypotenuse
            // a = asin(oppositeSide / hypotenuse)
            return MathExt.ToDeg(MathExt.ASin(oppositeSide / hypotenuse));
        }
        /// <summary>
        /// Gets angle in degrees from the adjacent side and the hypotenuse.
        /// </summary>
        /// <param name="adjacentSide">Length of the known side adjacent to the angle.</param>
        /// <param name="hypotenuse">Length of the hypotenuse.</param>
        public static decimal GetAngleFromAdjSideHyp(decimal adjacentSide, decimal hypotenuse)
        {
            // cos(a) = adjacentSide / hypotenuse
            // a = acos(adjacentSide / hypotenuse)
            return MathExt.ToDeg(MathExt.ACos(adjacentSide / hypotenuse));
        }

        /// <summary>
        /// Gets a side from information for a similar triangle.
        /// </summary>
        /// <param name="similarSide">Length of a side in similar triangle. Should correspond to the side to calculate.</param>
        /// <param name="similarHyp">Length of the hypotenusue in the similar triangle.</param>
        /// <param name="hypotenuse">Length of the hypotenuse in the target triangle.</param>
        public static decimal GetSideFromSimilarSideHyp(decimal similarSide, decimal similarHyp, decimal hypotenuse)
        {

            return (hypotenuse / similarHyp) * similarSide;

        }
        /// <summary>
        /// Gets the hypotenuse from information for a similar triangle.
        /// </summary>
        /// <param name="similarSide">Length of a side in similar triangle.</param>
        /// <param name="similarHyp">Length of the hypotenusue in the similar triangle.</param>
        /// <param name="correspondingSide">Length of the side that corresponds to <paramref name="similarSide"/>.</param>
        public static decimal GetHypFromSimilarSideHyp(decimal similarSide, decimal similarHyp, decimal correspondingSide)
        {

            return similarHyp * (correspondingSide / similarSide);

        }
        /// <summary>
        /// Gets a side from similar sides of another right triangle and the corresponding side to one of them.
        /// </summary>
        /// <param name="similarSideA">Length of a side in similar triangle. Corresponds to <paramref name="sideA"/>.</param>
        /// <param name="similarSideB">Length of a side in similar triangle. Corresponds to side to calculate.</param>
        /// <param name="sideA">Length of a side in target triangle. Corresponds to <paramref name="similarSideA"/></param>
        public static decimal GetSideFromSimilarSides(decimal similarSideA, decimal similarSideB, decimal sideA)
        {

            return similarSideB * (sideA / similarSideA);

        }

    }
}
