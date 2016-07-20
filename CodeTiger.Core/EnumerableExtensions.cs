using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeTiger
{
    /// <summary>
    /// Contains extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Determines whether any element of a sequence does not satisfy a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> that contains the elements to apply the predicate
        /// to.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns><c>true</c> if any elements in the source sequence do not pass the test in the specified
        /// predicate; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is
        /// <c>null</c>.</exception>
        public static bool AnyNot<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            Guard.ArgumentIsNotNull(nameof(source), source);
            Guard.ArgumentIsNotNull(nameof(predicate), predicate);

            return source.Any(element => !predicate(element));
        }
    }
}
