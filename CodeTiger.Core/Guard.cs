using System;
using System.Globalization;
using CodeTiger.Resources;

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
        /// <returns><paramref name="value"/> if it is not null.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        public static T ArgumentIsNotNull<T>(string name, [ValidatedNotNull] T? value)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }

            return value;
        }

        /// <summary>
        /// Ensures that a string argument is not null or empty, throwing an exception if it is either.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <returns><paramref name="value"/> if it is not null or empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is empty.</exception>
        public static string ArgumentIsNotNullOrEmpty(string name, [ValidatedNotNull] string? value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }

            if (value.Length == 0)
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, Strings.ArgumentCannotBeEmptyMessageFormat,
                        Environment.NewLine, name),
                    name);
            }

            return value;
        }

        /// <summary>
        /// Ensures that a string argument is not null, empty, or whitespace, throwing an exception if it is.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <returns><paramref name="value"/> if it is not null, empty, or whitespace.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is empty or contains only
        /// whitespace characters.</exception>
        public static string ArgumentIsNotNullOrWhiteSpace(string name, [ValidatedNotNull] string? value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }

            if (value.Length == 0)
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, Strings.ArgumentCannotBeEmptyMessageFormat,
                        Environment.NewLine, name),
                    name);
            }

            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                {
                    return value;
                }
            }

            throw new ArgumentException(
                string.Format(CultureInfo.CurrentCulture, Strings.ArgumentCannotBeOnlyWhitespaceMessageFormat,
                    Environment.NewLine, name),
                name);
        }

        /// <summary>
        /// Ensures that an argument is valid, throwing an exception if it is not valid.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="condition"><c>true</c> if the argument is valid, <c>false</c> otherwise.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="condition"/> is <c>false</c>.
        /// </exception>
        public static void ArgumentIsValid(string name, bool condition)
        {
            if (!condition)
            {
                throw new ArgumentException(Strings.ArgumentOutOfRangeMessage, name);
            }
        }

        /// <summary>
        /// Ensures that an argument is valid, throwing an exception if it is not valid.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="condition"><c>true</c> if the argument is valid, <c>false</c> otherwise.</param>
        /// <param name="value">The value of the argument.</param>
        /// <returns><paramref name="value"/> if <paramref name="condition"/> is <c>true</c>.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="condition"/> is <c>false</c>.
        /// </exception>
        public static T ArgumentIsValid<T>(string name, bool condition, [ValidatedNotNull] T value)
        {
            if (!condition)
            {
                throw new ArgumentException(Strings.ArgumentOutOfRangeMessage, name);
            }

            return value;
        }

        /// <summary>
        /// Ensures that an object has not been disposed, throwing an exception if it has been disposed.
        /// </summary>
        /// <param name="disposableObject">The object that may have been disposed.</param>
        /// <param name="hasObjectBeenDisposed">Indicates whether the object has been disposed.</param>
        /// <returns><paramref name="disposableObject"/> if <paramref name="hasObjectBeenDisposed"/> is
        /// <c>true</c>.</returns>
        /// <exception cref="ObjectDisposedException">Thrown when <paramref name="hasObjectBeenDisposed"/> is
        /// <c>true</c>.</exception>
        public static T ObjectHasNotBeenDisposed<T>(T disposableObject, bool hasObjectBeenDisposed)
            where T : IDisposable
        {
            if (hasObjectBeenDisposed)
            {
                // Try to get the actual type of the object, but fall back to to compile-time generic type argument
                // if the object is null.
                string? objectTypeName = disposableObject?.GetType().FullName ?? typeof(T).FullName;

                throw new ObjectDisposedException(objectTypeName);
            }

            return disposableObject;
        }

        /// <summary>
        /// Ensures that an operation is valid, throwing an exception if it is not valid.
        /// </summary>
        /// <param name="condition"><c>true</c> if the operation is valid, <c>false</c> otherwise.</param>
        /// <exception cref="InvalidOperationException">Thrown when <paramref name="condition"/> is false.
        /// </exception>
        public static void OperationIsValid(bool condition)
        {
            if (!condition)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
