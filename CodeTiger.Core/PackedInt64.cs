using System;
using System.Globalization;

namespace CodeTiger
{
    /// <summary>
    /// Represents a 64-bit signed integer value composed of smaller integer values.
    /// </summary>
    public struct PackedInt64 : IEquatable<PackedInt64>
    {
        /// <summary>
        /// Gets the underlying <see cref="long"/> value.
        /// </summary>
        public long Value { get; }

        /// <summary>
        /// Gets the <see cref="int"/> value from the lower 32 bits of this 64-bit value.
        /// </summary>
        public int LowerInt32 => unchecked((int)(Value & Bitmask.Int64Lower32));

        /// <summary>
        /// Gets the <see cref="int"/> value from the upper 32 bits of this 64-bit value.
        /// </summary>
        public int UpperInt32 => (int)(Value >> 32);

        /// <summary>
        /// Initializes a new instance of the <see cref="PackedInt64"/> structure to a specified packed value.
        /// </summary>
        /// <param name="value">The packed <see cref="long"/> value.</param>
        public PackedInt64(long value)
        {
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackedInt64"/> structure to the packed value of two
        /// <see cref="int"/> values.
        /// </summary>
        /// <param name="lowerValue">The <see cref="int"/> value to be stored in the lower 32 bits.</param>
        /// <param name="upperValue">The <see cref="int"/> value to be stored in the upper 32 bits.</param>
        public PackedInt64(int lowerValue, int upperValue)
        {
            Value = ((long)upperValue << 32) | ((long)lowerValue & Bitmask.Int64Lower32);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackedInt64"/> structure to the packed value of an array
        /// of two <see cref="int"/> values.
        /// </summary>
        /// <param name="values">The array of two <see cref="int"/> values that will be packed.</param>
        public PackedInt64(int[] values)
        {
            Guard.ArgumentIsNotNull(nameof(values), values);
            Guard.ArgumentIsValid(nameof(values), values.Length == 2);

            Value = ((long)values[1] << 32) | ((long)values[0] & Bitmask.Int64Lower32);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackedInt64"/> structure to the packed value of an array
        /// of four <see cref="short"/> values.
        /// </summary>
        /// <param name="values">The array of four <see cref="short"/> values that will be packed.</param>
        public PackedInt64(short[] values)
        {
            Guard.ArgumentIsNotNull(nameof(values), values);
            Guard.ArgumentIsValid(nameof(values), values.Length == 4);

            Value = ((long)values[3] << 48)
                | (((long)values[2] << 32) & Bitmask.Int64Bits32To47)
                | (((long)values[1] << 16) & Bitmask.Int64Bits16To31)
                | ((long)values[0] & Bitmask.Int64Bits0To15);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackedInt64"/> structure to the packed value of an array
        /// of four <see cref="byte"/> values.
        /// </summary>
        /// <param name="values">The array of eight <see cref="byte"/> values that will be packed.</param>
        public PackedInt64(byte[] values)
        {
            Guard.ArgumentIsNotNull(nameof(values), values);
            Guard.ArgumentIsValid(nameof(values), values.Length == 8);

            Value = ((long)values[7] << 56)
                | ((long)values[6] << 48)
                | ((long)values[5] << 40)
                | ((long)values[4] << 32)
                | ((long)values[3] << 24)
                | ((long)values[2] << 16)
                | ((long)values[1] << 8)
                | (long)values[0];
        }

        /// <summary>
        /// Gets the individual <see cref="int"/> values that make up this 64-bit value.
        /// </summary>
        /// <returns>The individual <see cref="int"/> values that make up this 64-bit value.</returns>
        public int[] GetInt32Values()
        {
            // Make a local copy of _value to avoid threading issues due to multiple reads.
            long value = Value;

            return unchecked(new[] { (int)value, (int)(value >> 32) });
        }

        /// <summary>
        /// Gets the individual <see cref="short"/> values that make up this 64-bit value.
        /// </summary>
        /// <returns>The individual <see cref="short"/> values that make up this 64-bit value.</returns>
        public short[] GetInt16Values()
        {
            // Make a local copy of _value to avoid threading issues due to multiple reads.
            long value = Value;

            return new[]
            {
                unchecked((short)value),
                unchecked((short)(value >> 16)),
                unchecked((short)(value >> 32)),
                unchecked((short)(value >> 48))
            };
        }

        /// <summary>
        /// Gets the individual <see cref="byte"/> values that make up this 64-bit value.
        /// </summary>
        /// <returns>The individual <see cref="byte"/> values that make up this 64-bit value.</returns>
        public byte[] GetByteValues()
        {
            // Make a local copy of _value to avoid threading issues due to multiple reads.
            long value = Value;

            return unchecked(new[]
            {
                (byte)value,
                (byte)(value >> 8),
                (byte)(value >> 16),
                (byte)(value >> 24),
                (byte)(value >> 32),
                (byte)(value >> 40),
                (byte)(value >> 48),
                (byte)(value >> 56)
            });
        }

        /// <summary>
        /// Returns a new <see cref="PackedInt64"/> value equivalent to this instance with its lower 32 bits set to
        /// a given <see cref="int"/> value.
        /// </summary>
        /// <param name="newLowerValue">The <see cref="int"/> value to use as the lower 32 bits of the new
        /// <see cref="PackedInt64"/> value.</param>
        /// <returns>A new <see cref="PackedInt64"/> value equivalent to this instance with its lower 32 bits set
        /// to <paramref name="newLowerValue"/>.</returns>
        public PackedInt64 WithLowerInt32(int newLowerValue)
        {
            return new PackedInt64((Value & Bitmask.Int64Upper32) | ((long)newLowerValue & Bitmask.Int64Lower32));
        }

        /// <summary>
        /// Returns a new <see cref="PackedInt64"/> value equivalent to this instance with its upper 32 bits set to
        /// a given <see cref="int"/> value.
        /// </summary>
        /// <param name="newUpperValue">The <see cref="int"/> value to use as the upper 32 bits of the new
        /// <see cref="PackedInt64"/> value.</param>
        /// <returns>A new <see cref="PackedInt64"/> value equivalent to this instance with its upper 32 bits set
        /// to <paramref name="newUpperValue"/>.</returns>
        public PackedInt64 WithUpperInt32(int newUpperValue)
        {
            return new PackedInt64(((long)newUpperValue << 32) | (Value & Bitmask.Int64Lower32));
        }

        /// <summary>
        /// Converts the value of this <see cref="PackedInt64"/> value to an equivalent <see cref="long"/> value.
        /// </summary>
        /// <returns>An <see cref="long"/> value equivalent to this <see cref="PackedInt64"/> value.</returns>
        public long ToInt64()
        {
            return Value;
        }

        /// <summary>
        /// Converts this <see cref="PackedInt64"/> value to its equivalent string representation.
        /// </summary>
        /// <returns>The string representation of this <see cref="PackedInt64"/> value, consisting of a minus sign
        /// if the value is negative, and a sequence of digits ranging from 0 to 9 with no leading zeroes.
        /// </returns>
        public override string ToString()
        {
            return Value.ToString(NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts this <see cref="PackedInt64"/> value to its equivalent string representation using a specified
        /// format.
        /// </summary>
        /// <param name="format">The numeric format string to use.</param>
        /// <returns>The string representation of this <see cref="PackedInt64"/> value as specified by
        /// <paramref name="format"/>.</returns>
        public string ToString(string format)
        {
            return Value.ToString(format, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts this <see cref="PackedInt64"/> value to its equivalent string representation using provided
        /// culture-specific format information.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting
        /// information.</param>
        /// <returns>The string representation of this <see cref="PackedInt64"/> value as specified by
        /// <paramref name="provider"/>.</returns>
        public string ToString(IFormatProvider provider)
        {
            return Value.ToString(provider);
        }

        /// <summary>
        /// Converts this <see cref="PackedInt64"/> value to its equivalent string representation using a specified
        /// format and provided culture-specific format information.
        /// </summary>
        /// <param name="format">The numeric format string to use.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting
        /// information.</param>
        /// <returns>The string representation of this <see cref="PackedInt64"/> value as specified by
        /// <paramref name="format"/> and <paramref name="provider"/>.</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            return Value.ToString(format, provider);
        }

        /// <summary>
        /// Determines whether this <see cref="PackedInt64"/> value is equal to another <see cref="PackedInt64"/>
        /// value.
        /// </summary>
        /// <param name="other">The <see cref="PackedInt64"/> value to compare to this value.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to this value, <c>false</c> otherwise.
        /// </returns>
        public bool Equals(PackedInt64 other)
        {
            return other == this;
        }

        /// <summary>
        /// Determines whether this <see cref="PackedInt64"/> value is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare to this value.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is equal to this value, <c>false</c> otherwise.
        /// </returns>
        public override bool Equals(object? obj)
        {
            return obj is PackedInt64 other && other == this;
        }

        /// <summary>
        /// Returns a hash code for this value.
        /// </summary>
        /// <returns>A hash code for this value.</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Creates a <see cref="PackedInt64"/> value from a given <see cref="long"/> value.
        /// </summary>
        /// <param name="value">The <see cref="long"/> value to create a <see cref="PackedInt64"/> value from.
        /// </param>
        /// <returns>A <see cref="PackedInt64"/> value equivalent to <paramref name="value"/>.</returns>
        public static PackedInt64 FromInt64(long value)
        {
            return new PackedInt64(value);
        }

        /// <summary>
        /// Determines whether two <see cref="PackedInt64"/> values are equal.
        /// </summary>
        /// <param name="first">The first <see cref="PackedInt64"/> value to compare.</param>
        /// <param name="second">The second <see cref="PackedInt64"/> value to compare.</param>
        /// <returns><c>true</c> if <paramref name="first"/> and <paramref name="second"/> are equal, <c>false</c>
        /// otherwise.</returns>
        public static bool operator ==(PackedInt64 first, PackedInt64 second)
        {
            return first.Value == second.Value;
        }

        /// <summary>
        /// Determines whether two <see cref="PackedInt64"/> values are not equal.
        /// </summary>
        /// <param name="first">The first <see cref="PackedInt64"/> value to compare.</param>
        /// <param name="second">The second <see cref="PackedInt64"/> value to compare.</param>
        /// <returns><c>true</c> if <paramref name="first"/> and <paramref name="second"/> are not equal,
        /// <c>false</c> otherwise.</returns>
        public static bool operator !=(PackedInt64 first, PackedInt64 second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Defines an implicit conversion of an <see cref="long"/> value to a <see cref="PackedInt64"/> value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static implicit operator PackedInt64(long value)
        {
            return new PackedInt64(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a <see cref="PackedInt64"/> value to a <see cref="long"/> value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static implicit operator long (PackedInt64 value)
        {
            return value.Value;
        }
    }
}
