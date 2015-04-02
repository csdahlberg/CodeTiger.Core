using System;

namespace CodeTiger
{
    /// <summary>
    /// Contains methods for ensuring method calls and arguments passed in to them are valid.
    /// </summary>
    public static class Guard
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

        #region ArgumentIsNotNegative Overloads

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

        #endregion

        #region ArgumentIsPositive Overloads

        /// <summary>
        /// Ensures that the value of a <see cref="short"/> argument is positive, throwing an exception if it is
        /// negative or zero.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative or
        /// zero.</exception>
        public static void ArgumentIsPositive(string name, short value)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Ensures that the value of a <see cref="int"/> argument is positive, throwing an exception if it is
        /// negative or zero.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative or
        /// zero.</exception>
        public static void ArgumentIsPositive(string name, int value)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Ensures that the value of a <see cref="long"/> argument is positive, throwing an exception if it is
        /// negative or zero.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative or
        /// zero.</exception>
        public static void ArgumentIsPositive(string name, long value)
        {
            if (value <= 0L)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Ensures that the value of a <see cref="float"/> argument is positive, throwing an exception if it is
        /// negative or zero.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative or
        /// zero.</exception>
        public static void ArgumentIsPositive(string name, float value)
        {
            if (value <= 0.0f)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Ensures that the value of a <see cref="double"/> argument is positive, throwing an exception if it is
        /// negative or zero.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative or
        /// zero.</exception>
        public static void ArgumentIsPositive(string name, double value)
        {
            if (value <= 0.0d)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        /// <summary>
        /// Ensures that the value of a <see cref="decimal"/> argument is positive, throwing an exception if it is
        /// negative or zero.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative or
        /// zero.</exception>
        public static void ArgumentIsPositive(string name, decimal value)
        {
            if (value <= 0m)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        #endregion

        #region ArgumentIsWithinRange Overloads

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

        #endregion

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