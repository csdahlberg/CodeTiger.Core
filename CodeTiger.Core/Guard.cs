using System;

namespace CodeTiger
{
    /// <summary>
    /// Contains methods for ensuring method calls and arguments passed in to them are valid.
    /// </summary>
    public static partial class Guard
    {
        /// <summary>
        /// Ensures that an argument is not null, throwing an exception if it is null.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        public static void ArgumentIsNotNull<T>(string name, [ValidatedNotNull] T value)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Ensures that an argument is valid, throwing an exception if it is not valid.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="condition"><c>true</c> if the argument is valid, <c>false</c> otherwise.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="condition"/> is false.</exception>
        public static void ArgumentIsValid(string name, bool condition)
        {
            if (!condition)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Ensures that an operation is valid, throwing an exception if it is not valid.
        /// </summary>
        /// <param name="condition"><c>true</c> if the operation is valid, <c>false</c> otherwise.</param>
        /// <exception cref="InvalidOperationException">Thrown when <paramref name="condition"/> is false.</exception>
        public static void OperationIsValid(bool condition)
        {
            if (!condition)
            {
                throw new InvalidOperationException();
            }
        }
    }
}