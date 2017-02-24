using System;

namespace CodeTiger
{
    public static partial class Guard
    {
        /// <summary>
        /// Ensures that the value of a <see cref="byte"/> argument is within a specified range, throwing an
        /// exception if it is outside of that range.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <param name="minimumValue">The minimum allowable value of <paramref name="value"/>.</param>
        /// <param name="maximumValue">The maximum allowable value of <paramref name="value"/>.</param>
        public static void ArgumentIsWithinRange(string name, byte value, byte minimumValue, byte maximumValue)
        {
            if (value < minimumValue || value > maximumValue)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Ensures that the value of a <see cref="short"/> argument is within a specified range, throwing an
        /// exception if it is outside of that range.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <param name="minimumValue">The minimum allowable value of <paramref name="value"/>.</param>
        /// <param name="maximumValue">The maximum allowable value of <paramref name="value"/>.</param>
        public static void ArgumentIsWithinRange(string name, short value, short minimumValue, short maximumValue)
        {
            if (value < minimumValue || value > maximumValue)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Ensures that the value of a <see cref="int"/> argument is within a specified range, throwing an
        /// exception if it is outside of that range.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <param name="minimumValue">The minimum allowable value of <paramref name="value"/>.</param>
        /// <param name="maximumValue">The maximum allowable value of <paramref name="value"/>.</param>
        public static void ArgumentIsWithinRange(string name, int value, int minimumValue, int maximumValue)
        {
            if (value < minimumValue || value > maximumValue)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Ensures that the value of a <see cref="long"/> argument is within a specified range, throwing an
        /// exception if it is outside of that range.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <param name="minimumValue">The minimum allowable value of <paramref name="value"/>.</param>
        /// <param name="maximumValue">The maximum allowable value of <paramref name="value"/>.</param>
        public static void ArgumentIsWithinRange(string name, long value, long minimumValue, long maximumValue)
        {
            if (value < minimumValue || value > maximumValue)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Ensures that the value of a <see cref="float"/> argument is within a specified range, throwing an
        /// exception if it is outside of that range.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <param name="minimumValue">The minimum allowable value of <paramref name="value"/>.</param>
        /// <param name="maximumValue">The maximum allowable value of <paramref name="value"/>.</param>
        public static void ArgumentIsWithinRange(string name, float value, float minimumValue, float maximumValue)
        {
            if (value < minimumValue || value > maximumValue)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Ensures that the value of a <see cref="double"/> argument is within a specified range, throwing an
        /// exception if it is outside of that range.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <param name="minimumValue">The minimum allowable value of <paramref name="value"/>.</param>
        /// <param name="maximumValue">The maximum allowable value of <paramref name="value"/>.</param>
        public static void ArgumentIsWithinRange(string name, double value, double minimumValue, double maximumValue)
        {
            if (value < minimumValue || value > maximumValue)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Ensures that the value of a <see cref="decimal"/> argument is within a specified range, throwing an
        /// exception if it is outside of that range.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <param name="minimumValue">The minimum allowable value of <paramref name="value"/>.</param>
        /// <param name="maximumValue">The maximum allowable value of <paramref name="value"/>.</param>
        public static void ArgumentIsWithinRange(string name, decimal value, decimal minimumValue, decimal maximumValue)
        {
            if (value < minimumValue || value > maximumValue)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Ensures that the value of an argument is within a valid range, throwing an exception if it is outside
        /// of that range.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="isWithinRange"><c>true</c> if the argument is valid, <c>false</c> otherwise.</param>
        public static void ArgumentIsWithinRange(string name, bool isWithinRange)
        {
            if (!isWithinRange)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }
    }
}
