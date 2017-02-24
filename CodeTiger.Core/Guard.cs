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
        /// Ensures that a string argument is not null or empty, throwing an exception if it is either.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is empty.</exception>
        public static void ArgumentIsNotNullOrEmpty(string name, [ValidatedNotNull] string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }

            if (value.Length == 0)
            {
                throw new ArgumentException(
                    $"Value cannot be empty.{Environment.NewLine}Parameter name: {name}", name);
            }
        }

        /// <summary>
        /// Ensures that a string argument is not null or empty, throwing an exception if it is either.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is empty or contains only
        /// whitespace characters.</exception>
        public static void ArgumentIsNotNullOrWhiteSpace(string name, [ValidatedNotNull] string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }

            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                {
                    return;
                }
            }

            if (value.Length == 0)
            {
                throw new ArgumentException(
                    $"Value cannot be empty.{Environment.NewLine}Parameter name: {name}", name);
            }

            throw new ArgumentException(
                $"Value cannot consist only of whitespace characters.{Environment.NewLine}Parameter name: {name}",
                name);
        }

        /// <summary>
        /// Ensures that an argument is valid, throwing an exception if it is not valid.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="condition"><c>true</c> if the argument is valid, <c>false</c> otherwise.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="condition"/> is false.</exception>
        public static void ArgumentIsValid(string name, bool condition)
        {
            if (!condition)
            {
                throw new ArgumentException("Value does not fall within the expected range.", name);
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