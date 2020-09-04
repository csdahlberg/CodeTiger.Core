using System;
using System.Globalization;

namespace CodeTiger
{
    /// <summary>
    /// Represents a 16-bit signed integer value composed of smaller integer values.
    /// </summary>
    public struct PackedInt16 : IEquatable<PackedInt16>
    {
        /// <summary>
        /// Gets the underlying <see cref="short"/> value.
        /// </summary>
        public short Value { get; }

        /// <summary>
        /// Gets the <see cref="byte"/> value from the lower 8 bits of this 16-bit value.
        /// </summary>
        public byte LowerByte => (byte)(Value & Bitmask.Int16Lower8);

        /// <summary>
        /// Gets the <see cref="byte"/> value from the upper 8 bits of this 16-bit value.
        /// </summary>
        public byte UpperByte => unchecked((byte)(Value >> 8));

        /// <summary>
        /// Initializes a new instance of the <see cref="PackedInt16"/> structure to a specified packed value.
        /// </summary>
        /// <param name="value">The packed <see cref="short"/> value.</param>
        public PackedInt16(short value)
        {
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackedInt16"/> structure to the packed value of two
        /// <see cref="byte"/> values.
        /// </summary>
        /// <param name="lowerValue">The <see cref="byte"/> value to be stored in the lower 8 bits.</param>
        /// <param name="upperValue">The <see cref="byte"/> value to be stored in the upper 8 bits.</param>
        public PackedInt16(byte lowerValue, byte upperValue)
        {
            Value = unchecked((short)((upperValue << 8) | lowerValue));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackedInt16"/> structure to the packed value of an array
        /// of two <see cref="byte"/> values.
        /// </summary>
        /// <param name="values">The array of two <see cref="byte"/> values that will be packed.</param>
        public PackedInt16(byte[] values)
        {
            Guard.ArgumentIsNotNull(nameof(values), values);
            Guard.ArgumentIsValid(nameof(values), values.Length == 2);

            Value = unchecked((short)(((short)values[1] << 8) | values[0]));
        }

        /// <summary>
        /// Gets the individual <see cref="byte"/> values that make up this 16-bit value.
        /// </summary>
        /// <returns>The individual <see cref="byte"/> values that make up this 16-bit value.</returns>
        public byte[] GetByteValues()
        {
            // Make a local copy of _value to avoid threading issues due to multiple reads.
            short value = Value;

            return unchecked(new[] { (byte)value, (byte)(value >> 8) });
        }

        /// <summary>
        /// Returns a new <see cref="PackedInt16"/> value equivalent to this instance with its lower 8 bits set to
        /// a given <see cref="byte"/> value.
        /// </summary>
        /// <param name="newLowerValue">The <see cref="byte"/> value to use as the lower 8 bits of the new
        /// <see cref="PackedInt16"/> value.</param>
        /// <returns>A new <see cref="PackedInt16"/> value equivalent to this instance with its lower 8 bits set to
        /// <paramref name="newLowerValue"/>.</returns>
        public PackedInt16 WithLowerByte(byte newLowerValue)
        {
            return new PackedInt16((short)((Value & Bitmask.Int16Upper8) | newLowerValue));
        }

        /// <summary>
        /// Returns a new <see cref="PackedInt16"/> value equivalent to this instance with its upper 8 bits set to
        /// a given <see cref="byte"/> value.
        /// </summary>
        /// <param name="newUpperValue">The <see cref="byte"/> value to use as the lower 8 bits of the new
        /// <see cref="PackedInt16"/> value.</param>
        /// <returns>A new <see cref="PackedInt16"/> value equivalent to this instance with its lower 8 bits set to
        /// <paramref name="newUpperValue"/>.</returns>
        public PackedInt16 WithUpperByte(byte newUpperValue)
        {
            return new PackedInt16(unchecked((short)((newUpperValue << 8) | (Value & Bitmask.Int16Lower8))));
        }

        /// <summary>
        /// Converts the value of this <see cref="PackedInt16"/> value to an equivalent <see cref="short"/> value.
        /// </summary>
        /// <returns>An <see cref="short"/> value equivalent to this <see cref="PackedInt16"/> value.</returns>
        public short ToInt16()
        {
            return Value;
        }

        /// <summary>
        /// Converts this <see cref="PackedInt16"/> value to its equivalent string representation.
        /// </summary>
        /// <returns>The string representation of this <see cref="PackedInt16"/> value, consisting of a minus sign
        /// if the value is negative, and a sequence of digits ranging from 0 to 9 with no leading zeroes.
        /// </returns>
        public override string ToString()
        {
            return Value.ToString(NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts this <see cref="PackedInt16"/> value to its equivalent string representation using a specified
        /// format.
        /// </summary>
        /// <param name="format">The numeric format string to use.</param>
        /// <returns>The string representation of this <see cref="PackedInt16"/> value as specified by
        /// <paramref name="format"/>.</returns>
        public string ToString(string format)
        {
            return Value.ToString(format, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts this <see cref="PackedInt16"/> value to its equivalent string representation using provided
        /// culture-specific format information.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting
        /// information.</param>
        /// <returns>The string representation of this <see cref="PackedInt16"/> value as specified by
        /// <paramref name="provider"/>.</returns>
        public string ToString(IFormatProvider provider)
        {
            return Value.ToString(provider);
        }

        /// <summary>
        /// Converts this <see cref="PackedInt16"/> value to its equivalent string representation using a specified
        /// format and provided culture-specific format information.
        /// </summary>
        /// <param name="format">The numeric format string to use.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting
        /// information.</param>
        /// <returns>The string representation of this <see cref="PackedInt16"/> value as specified by
        /// <paramref name="format"/> and <paramref name="provider"/>.</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            return Value.ToString(format, provider);
        }

        /// <summary>
        /// Determines whether this <see cref="PackedInt16"/> value is equal to another <see cref="PackedInt16"/>
        /// value.
        /// </summary>
        /// <param name="other">The <see cref="PackedInt16"/> value to compare to this value.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to this value, <c>false</c> otherwise.
        /// </returns>
        public bool Equals(PackedInt16 other)
        {
            return other == this;
        }

        /// <summary>
        /// Determines whether this <see cref="PackedInt16"/> value is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare to this value.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is equal to this value, <c>false</c> otherwise.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is PackedInt16 && (PackedInt16)obj == this;
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
        /// Creates a <see cref="PackedInt16"/> value from a given <see cref="short"/> value.
        /// </summary>
        /// <param name="value">The <see cref="short"/> value to create a <see cref="PackedInt16"/> value from.
        /// </param>
        /// <returns>A <see cref="PackedInt16"/> value equivalent to <paramref name="value"/>.</returns>
        public static PackedInt16 FromInt16(short value)
        {
            return new PackedInt16(value);
        }

        /// <summary>
        /// Determines whether two <see cref="PackedInt16"/> values are equal.
        /// </summary>
        /// <param name="first">The first <see cref="PackedInt16"/> value to compare.</param>
        /// <param name="second">The second <see cref="PackedInt16"/> value to compare.</param>
        /// <returns><c>true</c> if <paramref name="first"/> and <paramref name="second"/> are equal, <c>false</c>
        /// otherwise.</returns>
        public static bool operator ==(PackedInt16 first, PackedInt16 second)
        {
            return first.Value == second.Value;
        }

        /// <summary>
        /// Determines whether two <see cref="PackedInt16"/> values are not equal.
        /// </summary>
        /// <param name="first">The first <see cref="PackedInt16"/> value to compare.</param>
        /// <param name="second">The second <see cref="PackedInt16"/> value to compare.</param>
        /// <returns><c>true</c> if <paramref name="first"/> and <paramref name="second"/> are not equal,
        /// <c>false</c> otherwise.</returns>
        public static bool operator !=(PackedInt16 first, PackedInt16 second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Defines an implicit conversion of an <see cref="short"/> value to a <see cref="PackedInt16"/> value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static implicit operator PackedInt16(short value)
        {
            return new PackedInt16(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a <see cref="PackedInt16"/> value to an <see cref="short"/> value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static implicit operator short(PackedInt16 value)
        {
            return value.Value;
        }
    }
}
