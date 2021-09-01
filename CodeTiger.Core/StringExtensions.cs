using System;

namespace CodeTiger
{
    /// <summary>
    /// Contains extension methods for the <see cref="string"/> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a string array that contains substrings in this instance that are separated after
        /// <paramref name="splitLocation"/> characters.
        /// </summary>
        /// <param name="original">The <see cref="string"/> to split.</param>
        /// <param name="splitLocation">The location to split <paramref name="original"/>.</param>
        /// <returns>A string array consisting of two strings from <paramref name="original"/>.</returns>
        public static string[] SplitAt(this string original, int splitLocation)
        {
            Guard.ArgumentIsNotNull(nameof(original), original);
            Guard.ArgumentIsWithinRange(nameof(splitLocation), splitLocation, 0, original.Length);

            return new string[]
                {
                    original.Substring(0, splitLocation),
                    original.Substring(splitLocation)
                };
        }

        /// <summary>
        /// Determines whether a specified substring occurs within this string when using the specified comparison
        /// option.
        /// </summary>
        /// <param name="source">The original string.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="comparisonType">One of the enumeration values that determines how this string and value
        /// are compared.</param>
        /// <returns><c>true</c> if <paramref name="value"/> occurs within <paramref name="source"/>, or if
        /// <paramref name="value"/> is the empty string; otherwise, <c>false</c>.</returns>
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            Guard.ArgumentIsNotNull(nameof(source), source);
            Guard.ArgumentIsNotNull(nameof(value), value);

#if NET5_0_OR_GREATER
            return source.Contains(value, comparisonType);
#else
            return source.IndexOf(value, comparisonType) >= 0;
#endif
        }
    }
}
