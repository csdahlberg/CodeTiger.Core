using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeTiger.Collections.Generic
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

        /// <summary>
        /// Determines whether a sequence contains no elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to check for emptiness.</param>
        /// <returns><c>true</c> if the source sequence does not contain any elements; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        public static bool None<TSource>(this IEnumerable<TSource> source)
        {
            Guard.ArgumentIsNotNull(nameof(source), source);

            return !source.Any();
        }

        /// <summary>
        /// Determines whether a sequence contains no elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to check for emptiness.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns><c>true</c> if the source sequence does not contain any elements; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is
        /// <c>null</c>.</exception>
        public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Guard.ArgumentIsNotNull(nameof(source), source);
            Guard.ArgumentIsNotNull(nameof(predicate), predicate);

            return !source.Any(predicate);
        }

        /// <summary>
        /// Bypasses elements in a sequence until a specified condition is true and then returns the remaining
        /// elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to return elements from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains the elements from the input sequence
        /// starting at the first element in the linear series that passes the test specified by
        /// <paramref name="predicate"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is
        /// <c>null</c>.</exception>
        public static IEnumerable<TSource> SkipUntil<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            Guard.ArgumentIsNotNull(nameof(source), source);
            Guard.ArgumentIsNotNull(nameof(predicate), predicate);

            return source.SkipWhile(element => !predicate(element));
        }

        /// <summary>
        /// Bypasses elements in a sequence until a specified condition is true and then returns the remaining
        /// elements. The element's index is used in the logic of the predicate function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to return elements from.</param>
        /// <param name="predicate">A function to test each element for a condition; the second parameter of the
        /// function represents the index of the source element.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains the elements from the input sequence
        /// starting at the first element in the linear series that passes the test specified by
        /// <paramref name="predicate"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is
        /// <c>null</c>.</exception>
        public static IEnumerable<TSource> SkipUntil<TSource>(this IEnumerable<TSource> source,
            Func<TSource, int, bool> predicate)
        {
            Guard.ArgumentIsNotNull(nameof(source), source);
            Guard.ArgumentIsNotNull(nameof(predicate), predicate);

            return source.SkipWhile((element, index) => !predicate(element, index));
        }

        /// <summary>
        /// Returns elements from a sequence as long as a specified condition is false.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence to return elements from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains the elements from the input sequence that occur
        /// before the element at which the test first passes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is
        /// <c>null</c>.</exception>
        public static IEnumerable<TSource> TakeUntil<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            Guard.ArgumentIsNotNull(nameof(source), source);
            Guard.ArgumentIsNotNull(nameof(predicate), predicate);

            return source.TakeWhile(element => !predicate(element));
        }

        /// <summary>
        /// Returns elements from a sequence as long as a specified condition is false. The element's index is
        /// used in the logic of the predicate function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence to return elements from.</param>
        /// <param name="predicate">A function to test each element for a condition; the second parameter of the
        /// function represents the index of the source element.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains the elements from the input sequence that occur
        /// before the element at which the test first passes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is
        /// <c>null</c>.</exception>
        public static IEnumerable<TSource> TakeUntil<TSource>(this IEnumerable<TSource> source,
            Func<TSource, int, bool> predicate)
        {
            Guard.ArgumentIsNotNull(nameof(source), source);
            Guard.ArgumentIsNotNull(nameof(predicate), predicate);

            return source.TakeWhile((element, index) => !predicate(element, index));
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to filter.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that do not
        /// satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is
        /// <c>null</c>.</exception>
        public static IEnumerable<TSource> WhereNot<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            Guard.ArgumentIsNotNull(nameof(source), source);
            Guard.ArgumentIsNotNull(nameof(predicate), predicate);

            return source.Where(element => !predicate(element));
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate. Each element's index is used in the logic of the
        /// predicate function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to filter.</param>
        /// <param name="predicate">A function to test each element for a condition; the second parameter of the
        /// function represents the index of the source element.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that do not
        /// satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is
        /// <c>null</c>.</exception>
        public static IEnumerable<TSource> WhereNot<TSource>(this IEnumerable<TSource> source,
            Func<TSource, int, bool> predicate)
        {
            Guard.ArgumentIsNotNull(nameof(source), source);
            Guard.ArgumentIsNotNull(nameof(predicate), predicate);

            return source.Where((element, index) => !predicate(element, index));
        }
    }
}
