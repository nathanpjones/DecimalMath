namespace DecimalMath
{
    /// <summary>
    /// Helper functions.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Swaps the value between two variables.
        /// </summary>
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            var temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        /// <summary>
        /// Prime number to use to begin a hash of an object.
        /// </summary>
        /// <remarks>
        /// See: http://stackoverflow.com/questions/263400
        /// </remarks>
        public const int HashStart = 17;
        private const int HashPrime = 397;

        /// <summary>
        /// Adds a hash of an object to a running hash value.
        /// </summary>
        /// <param name="hash">A running hash value.</param>
        /// <param name="obj">The object to hash and incorporate into the running hash.</param>
        public static int HashObject(this int hash, object obj)
        {
            unchecked { return hash * HashPrime ^ (ReferenceEquals(null, obj) ? 0 : obj.GetHashCode()); }
        }

        /// <summary>
        /// Adds a hash of a struct to a running hash value.
        /// </summary>
        /// <param name="hash">A running hash value.</param>
        /// <param name="value">The struct to hash and incorporate into the running hash.</param>
        public static int HashValue<T>(this int hash, T value) where T : struct
        {
            unchecked { return hash * HashPrime ^ value.GetHashCode(); }
        }
    }
}
