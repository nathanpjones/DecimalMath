using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MathExtensions.TwoD;

namespace MathExtensions
{
    public static partial class MathExt
    {
        //public static decimal Distance(PointF pt1, PointF pt2)
        //{

        //    return Math.Sqrt((pt1.X - pt2.X) * (pt1.X - pt2.X) + (pt1.Y - pt2.Y) * (pt1.Y - pt2.Y));

        //}
        public static decimal Distance(decimal x1, decimal y1, decimal x2, decimal y2)
        {

            return Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

        }

        //public static AnglePoint CartToPolar(PointF p)
        //{

        //    AnglePoint a = default(AnglePoint);

        //    a.ARad = Math.Atan2(p.Y, p.X);
        //    a.Y = Math.Sqrt(p.X * p.X + p.Y * p.Y);

        //    return a;

        //}
        //public static PointF PolarToCart(AnglePoint a)
        //{

        //    PointF p = default(PointF);

        //    p.X = a.Y * Cos(a.ARad);
        //    p.Y = a.Y * Sin(a.ARad);

        //    return p;

        //}
    }
}
