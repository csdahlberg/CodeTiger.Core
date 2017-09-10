using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace CodeTiger
{
    /// <summary>
    /// Represents a 32-bit signed integer value composed of smaller integer values.
    /// </summary>
    public struct PackedInt32
    {
        private readonly int _value;

        /// <summary>
        /// Gets the underlying <see cref="int"/> value.
        /// </summary>
        public int Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Gets the <see cref="short"/> value from the lower 16 bits of this 32-bit value.
        /// </summary>
        public short LowerInt16
        {
            get { return unchecked((short)(_value & Bitmask.Int32Lower16)); }
        }

        /// <summary>
        /// Gets the <see cref="short"/> value from the upper 16 bits of this 32-bit value.
        /// </summary>
        public short UpperInt16
        {
            get { return (short)(_value >> 16); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackedInt32"/> structure to a specified packed value.
        /// </summary>
        /// <param name="value">The packed <see cref="int"/> value.</param>
        public PackedInt32(int value)
        {
            _value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackedInt32"/> structure to the packed value of two
        /// <see cref="short"/> values.
        /// </summary>
        /// <param name="lowerValue">The <see cref="short"/> value to be stored in the lower 16 bits.</param>
        /// <param name="upperValue">The <see cref="short"/> value to be stored in the upper 16 bits.</param>
        public PackedInt32(short lowerValue, short upperValue)
        {
            _value = (upperValue << 16) | ((int)lowerValue & Bitmask.Int32Lower16);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackedInt32"/> structure to the packed value of an array
        /// of two <see cref="short"/> values.
        /// </summary>
        /// <param name="values">The array of two <see cref="short"/> values that will be packed.</param>
        public PackedInt32(short[] values)
        {
            Guard.ArgumentIsNotNull(nameof(values), values);
            Guard.ArgumentIsValid(nameof(values), values.Length == 2);

            _value = (values[1] << 16) | ((int)values[0] & Bitmask.Int32Lower16);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackedInt32"/> structure to the packed value of an array
        /// of four <see cref="byte"/> values.
        /// </summary>
        /// <param name="values">The array of four <see cref="byte"/> values that will be packed.</param>
        public PackedInt32(byte[] values)
        {
            Guard.ArgumentIsNotNull(nameof(values), values);
            Guard.ArgumentIsValid(nameof(values), values.Length == 4);

            _value = (values[3] << 24)
                | (values[2] << 16)
                | (values[1] << 8)
                | values[0];
        }

        /// <summary>
        /// Gets the individual <see cref="short"/> values that make up this 32-bit value.
        /// </summary>
        /// <returns>The individual <see cref="short"/> values that make up this 32-bit value.</returns>
        public short[] GetInt16Values()
        {
            // Make a local copy of _value to avoid threading issues due to multiple reads.
            int value = _value;

            return unchecked(new[] { (short)value, (short)(value >> 16) });
        }

        /// <summary>
        /// Gets the individual <see cref="byte"/> values that make up this 32-bit value.
        /// </summary>
        /// <returns>The individual <see cref="byte"/> values that make up this 32-bit value.</returns>
        public byte[] GetByteValues()
        {
            // Make a local copy of _value to avoid threading issues due to multiple reads.
            int value = _value;

            return new[]
            {
                unchecked((byte)value),
                unchecked((byte)(value >> 8)),
                unchecked((byte)(value >> 16)),
                unchecked((byte)(value >> 24))
            };
        }

        /// <summary>
        /// Returns a new <see cref="PackedInt32"/> value equivalent to this instance with its lower 16 bits set to
        /// a given <see cref="short"/> value.
        /// </summary>
        /// <param name="newLowerValue">The <see cref="short"/> value to use as the lower 8 bits of the new
        /// <see cref="PackedInt32"/> value.</param>
        /// <returns>A new <see cref="PackedInt32"/> value equivalent to this instance with its lower 16 bits set
        /// to <paramref name="newLowerValue"/>.</returns>
        public PackedInt32 WithLowerInt16(short newLowerValue)
        {
            return new PackedInt32(
                unchecked((_value & Bitmask.Int32Upper16) | ((int)newLowerValue & Bitmask.Int32Lower16)));
        }

        /// <summary>
        /// Returns a new <see cref="PackedInt32"/> value equivalent to this instance with its upper 16 bits set to
        /// a given <see cref="short"/> value.
        /// </summary>
        /// <param name="newUpperValue">The <see cref="short"/> value to use as the upper 16 bits of the new
        /// <see cref="PackedInt32"/> value.</param>
        /// <returns>A new <see cref="PackedInt32"/> value equivalent to this instance with its upper 16 bits set
        /// to <paramref name="newUpperValue"/>.</returns>
        public PackedInt32 WithUpperInt16(short newUpperValue)
        {
            return new PackedInt32(((int)newUpperValue << 16) | (_value & Bitmask.Int32Lower16));
        }

        /// <summary>
        /// Converts the value of this <see cref="PackedInt32"/> value to an equivalent <see cref="int"/> value.
        /// </summary>
        /// <returns>An <see cref="int"/> value equivalent to this <see cref="PackedInt32"/> value.</returns>
        public int ToInt32()
        {
            return _value;
        }

        /// <summary>
        /// Converts this <see cref="PackedInt32"/> value to its equivalent string representation.
        /// </summary>
        /// <returns>The string representation of this <see cref="PackedInt32"/> value, consisting of a minus sign
        /// if the value is negative, and a sequence of digits ranging from 0 to 9 with no leading zeroes.
        /// </returns>
        public override string ToString()
        {
            return _value.ToString(NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts this <see cref="PackedInt32"/> value to its equivalent string representation using a specified
        /// format.
        /// </summary>
        /// <param name="format">The numeric format string to use.</param>
        /// <returns>The string representation of this <see cref="PackedInt32"/> value as specified by
        /// <paramref name="format"/>.</returns>
        public string ToString(string format)
        {
            return _value.ToString(format, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts this <see cref="PackedInt32"/> value to its equivalent string representation using provided
        /// culture-specific format information.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting
        /// information.</param>
        /// <returns>The string representation of this <see cref="PackedInt32"/> value as specified by
        /// <paramref name="provider"/>.</returns>
        public string ToString(IFormatProvider provider)
        {
            return _value.ToString(provider);
        }

        /// <summary>
        /// Converts this <see cref="PackedInt32"/> value to its equivalent string representation using a specified
        /// format and provided culture-specific format information.
        /// </summary>
        /// <param name="format">The numeric format string to use.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting
        /// information.</param>
        /// <returns>The string representation of this <see cref="PackedInt32"/> value as specified by
        /// <paramref name="format"/> and <paramref name="provider"/>.</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            return _value.ToString(format, provider);
        }

        /// <summary>
        /// Determines whether this <see cref="PackedInt32"/> value is equal to another <see cref="PackedInt32"/>
        /// value.
        /// </summary>
        /// <param name="other">The <see cref="PackedInt32"/> value to compare to this value.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to this value, <c>false</c> otherwise.
        /// </returns>
        public bool Equals(PackedInt32 other)
        {
            return other == this;
        }

        /// <summary>
        /// Determines whether this <see cref="PackedInt32"/> value is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare to this value.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is equal to this value, <c>false</c> otherwise.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is PackedInt32 && (PackedInt32)obj == this;
        }

        /// <summary>
        /// Returns a hash code for this value.
        /// </summary>
        /// <returns>A hash code for this value.</returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// Determines whether two <see cref="PackedInt32"/> values are equal.
        /// </summary>
        /// <param name="first">The first <see cref="PackedInt32"/> value to compare.</param>
        /// <param name="second">The second <see cref="PackedInt32"/> value to compare.</param>
        /// <returns><c>true</c> if <paramref name="first"/> and <paramref name="second"/> are equal, <c>false</c>
        /// otherwise.</returns>
        public static bool operator ==(PackedInt32 first, PackedInt32 second)
        {
            return first.Value == second.Value;
        }

        /// <summary>
        /// Determines whether two <see cref="PackedInt32"/> values are not equal.
        /// </summary>
        /// <param name="first">The first <see cref="PackedInt32"/> value to compare.</param>
        /// <param name="second">The second <see cref="PackedInt32"/> value to compare.</param>
        /// <returns><c>true</c> if <paramref name="first"/> and <paramref name="second"/> are not equal,
        /// <c>false</c> otherwise.</returns>
        public static bool operator !=(PackedInt32 first, PackedInt32 second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Defines an implicit conversion of an <see cref="int"/> value to a <see cref="PackedInt32"/> value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static implicit operator PackedInt32(int value)
        {
            return new PackedInt32(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a <see cref="PackedInt32"/> value to an <see cref="int"/> value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static implicit operator int(PackedInt32 value)
        {
            return value._value;
        }
    }
}