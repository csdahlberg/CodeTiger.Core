using System;

namespace CodeTiger
{
    public static partial class Guard
    {
        /// <summary>
        /// Ensures that the value of a <see cref="short"/> argument is not negative, throwing an exception if it
        /// is negative.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative.
        /// </exception>
        public static void ArgumentIsNotNegative(string name, short value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Ensures that the value of a <see cref="int"/> argument is not negative, throwing an exception if it
        /// is negative.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative.
        /// </exception>
        public static void ArgumentIsNotNegative(string name, int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Ensures that the value of a <see cref="long"/> argument is not negative, throwing an exception if it
        /// is negative.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative.
        /// </exception>
        public static void ArgumentIsNotNegative(string name, long value)
        {
            if (value < 0L)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Ensures that the value of a <see cref="float"/> argument is not negative, throwing an exception if it
        /// is negative.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative.
        /// </exception>
        public static void ArgumentIsNotNegative(string name, float value)
        {
            if (value < 0.0f)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Ensures that the value of a <see cref="double"/> argument is not negative, throwing an exception if it
        /// is negative.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative.
        /// </exception>
        public static void ArgumentIsNotNegative(string name, double value)
        {
            if (value < 0.0d)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Ensures that the value of a <see cref="decimal"/> argument is not negative, throwing an exception if
        /// it is negative.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative.
        /// </exception>
        public static void ArgumentIsNotNegative(string name, decimal value)
        {
            if (value < 0m)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }
    }
}
