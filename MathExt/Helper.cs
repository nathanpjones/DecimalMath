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

        // See: http://stackoverflow.com/questions/263400
        public const int HashStart = 17;
        private const int HashPrime = 397;

        public static int HashObject(this int hash, object obj)
        {
            unchecked { return hash * HashPrime ^ (ReferenceEquals(null, obj) ? 0 : obj.GetHashCode()); }
        }

        public static int HashValue<T>(this int hash, T value) where T : struct
        {
            unchecked { return hash * HashPrime ^ value.GetHashCode(); }
        }
    }
}
