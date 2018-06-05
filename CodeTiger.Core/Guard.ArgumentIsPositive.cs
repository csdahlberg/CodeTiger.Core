using System;

namespace CodeTiger
{
    public static partial class Guard
    {
        /// <summary>
        /// Ensures that the value of a <see cref="short"/> argument is positive, throwing an exception if it is
        /// negative or zero.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <returns><paramref name="value"/> if it is positive.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative or
        /// zero.</exception>
        public static short ArgumentIsPositive(string name, short value)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }

            return value;
        }

        /// <summary>
        /// Ensures that the value of a <see cref="int"/> argument is positive, throwing an exception if it is
        /// negative or zero.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <returns><paramref name="value"/> if it is positive.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative or
        /// zero.</exception>
        public static int ArgumentIsPositive(string name, int value)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }

            return value;
        }

        /// <summary>
        /// Ensures that the value of a <see cref="long"/> argument is positive, throwing an exception if it is
        /// negative or zero.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <returns><paramref name="value"/> if it is positive.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative or
        /// zero.</exception>
        public static long ArgumentIsPositive(string name, long value)
        {
            if (value <= 0L)
            {
                throw new ArgumentOutOfRangeException(name);
            }

            return value;
        }

        /// <summary>
        /// Ensures that the value of a <see cref="float"/> argument is positive, throwing an exception if it is
        /// negative or zero.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <returns><paramref name="value"/> if it is positive.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative or
        /// zero.</exception>
        public static float ArgumentIsPositive(string name, float value)
        {
            if (value <= 0.0f)
            {
                throw new ArgumentOutOfRangeException(name);
            }

            return value;
        }

        /// <summary>
        /// Ensures that the value of a <see cref="double"/> argument is positive, throwing an exception if it is
        /// negative or zero.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <returns><paramref name="value"/> if it is positive.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative or
        /// zero.</exception>
        public static double ArgumentIsPositive(string name, double value)
        {
            if (value <= 0.0d)
            {
                throw new ArgumentOutOfRangeException(name);
            }

            return value;
        }

        /// <summary>
        /// Ensures that the value of a <see cref="decimal"/> argument is positive, throwing an exception if it is
        /// negative or zero.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <returns><paramref name="value"/> if it is positive.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative or
        /// zero.</exception>
        public static decimal ArgumentIsPositive(string name, decimal value)
        {
            if (value <= 0m)
            {
                throw new ArgumentOutOfRangeException(name);
            }

            return value;
        }
    }
}
