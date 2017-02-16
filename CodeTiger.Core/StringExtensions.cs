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
    }
}