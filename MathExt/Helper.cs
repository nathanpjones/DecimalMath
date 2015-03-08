using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathExtensions
{
    internal static class Helper
    {
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            var temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
    }
}
