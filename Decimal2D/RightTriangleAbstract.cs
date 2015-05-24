namespace DecimalMath
{
    /// <summary>
    /// Represents a right triangle without specifying the location of any of its points.
    /// </summary>
    /// <remarks>
    /// See http://mathworld.wolfram.com/RightTriangle.html
    /// </remarks>
    public struct RightTriangleAbstract
    {
        /// <summary>
        /// Gets the angle adjacent to side A. Angle is in degrees.
        /// </summary>
        public decimal AngleA { get; private set; }
        /// <summary>
        /// Gets the angle adjacent to side B. Angle is in degrees.
        /// </summary>
        public decimal AngleB { get; private set; }
        /// <summary>
        /// Gets the length of the hypotenuse.
        /// </summary>
        public decimal Hypotenuse { get; private set; }
        /// <summary>
        /// Gets the length of side A.
        /// </summary>
        public decimal LengthA { get; private set; }
        /// <summary>
        /// Gets the length of side B.
        /// </summary>
        public decimal LengthB { get; private set; }

        /// <summary>
        /// Creates a right triangle from its two sides.
        /// </summary>
        /// <param name="lengthA">Length of side A.</param>
        /// <param name="lengthB">Length of side B.</param>
        public static RightTriangleAbstract FromTwoSides(decimal lengthA, decimal lengthB)
        {
            var t = new RightTriangleAbstract
                    {
                        LengthA = lengthA,
                        LengthB = lengthB,
                        Hypotenuse = RightTriangle.GetHypFromSides(lengthA, lengthB),
                        AngleA = RightTriangle.GetAngleFromSides(lengthB, lengthA),
                        AngleB = RightTriangle.GetAngleFromSides(lengthA, lengthB)
                    };
            return t;
        }

        /// <summary>
        /// Creates a right triangle from one side and the hypotenuse.
        /// This side is treated as side A.
        /// </summary>
        /// <param name="lengthA">Length of side A.</param>
        /// <param name="hypotenuse">Length of the hypotenuse.</param>
        public static RightTriangleAbstract FromSideAHypotenuse(decimal lengthA, decimal hypotenuse)
        {
            var t = new RightTriangleAbstract
                    {
                        LengthA = lengthA,
                        Hypotenuse = hypotenuse,
                        LengthB = RightTriangle.GetSideFromSideHyp(lengthA, hypotenuse),
                        AngleA = RightTriangle.GetAngleFromAdjSideHyp(lengthA, hypotenuse),
                        AngleB = RightTriangle.GetAngleFromOppSideHyp(lengthA, hypotenuse)
                    };
            return t;
        }

        /// <summary>
        /// Creates a right triangle from one side and its adjacent angle in degrees.
        /// This side and angle are treated as side/angle A.
        /// </summary>
        /// <param name="lengthA">Length of side A.</param>
        /// <param name="angleA">Angle adjacent to side A in degrees.</param>
        public static RightTriangleAbstract FromSideAAngleA(decimal lengthA, decimal angleA)
        {
            var t = new RightTriangleAbstract
                    {
                        LengthA = lengthA,
                        AngleA = angleA,
                        LengthB = RightTriangle.GetSideFromOppAngleOppSide(angleA, lengthA),
                        AngleB = RightTriangle.GetAngleFromOtherAngle(angleA),
                        Hypotenuse = RightTriangle.GetHypFromSideAdjAngle(lengthA, angleA)
                    };
            return t;
        }

        /// <summary>
        /// Creates a right triangle from one side and the hypotenuse.
        /// This side is treated as side B.
        /// </summary>
        /// <param name="lengthA">Length of side B.</param>
        /// <param name="hypotenuse">Length of the hypotenuse.</param>
        public static RightTriangleAbstract FromSideBHypotenuse(decimal lengthA, decimal hypotenuse)
        {
            return FromSideAHypotenuse(lengthA, hypotenuse).SwapSides();
        }

        /// <summary>
        /// Creates a right triangle from one side and its adjacent angle in degrees.
        /// This side and angle are treated as side/angle A.
        /// </summary>
        /// <param name="lengthB">Length of side B.</param>
        /// <param name="angleB">Angle adjacent to side B in degrees.</param>
        public static RightTriangleAbstract FromSideBAngleB(decimal lengthB, decimal angleB)
        {
            return FromSideAAngleA(lengthB, angleB).SwapSides();
        }

        /// <summary>
        /// Swaps which sides / angles are considered A and B.
        /// </summary>
        public RightTriangleAbstract SwapSides()
        {
            var t = new RightTriangleAbstract
                    {
                        LengthA = LengthB,
                        AngleA = AngleB,
                        LengthB = LengthA,
                        AngleB = AngleA,
                        Hypotenuse = Hypotenuse,
                    };
            return t;
        }
    }
}