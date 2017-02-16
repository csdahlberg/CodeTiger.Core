using System;
using System.Collections.Generic;
using CodeTiger;
using Xunit;

namespace UnitTests.CodeTiger
{
    public partial class GuardTests
    {
        public class ArgumentIsWithinRange_String_Byte_Byte_Byte
        {
            [Theory]
            [InlineData(byte.MinValue, (byte)1, (byte)254)]
            [InlineData(byte.MaxValue, (byte)1, (byte)254)]
            [InlineData(byte.MinValue, (byte)128, (byte)128)]
            [InlineData((byte)127, (byte)128, (byte)128)]
            [InlineData((byte)129, (byte)128, (byte)128)]
            [InlineData(byte.MaxValue, (byte)128, (byte)128)]
            public void ThrowsArgumentOutOfRangeExceptionWhenArgumentIsOutsideOfRange(byte argumentValue,
                byte minimumValue, byte maximumValue)
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsWithinRange("DummyArgumentName", argumentValue, minimumValue,
                        maximumValue));
            }

            [Theory]
            [InlineData(byte.MinValue, byte.MinValue, byte.MaxValue)]
            [InlineData((byte)1, byte.MinValue, byte.MaxValue)]
            [InlineData((byte)128, byte.MinValue, byte.MaxValue)]
            [InlineData((byte)254, byte.MinValue, byte.MaxValue)]
            [InlineData(byte.MaxValue, byte.MinValue, byte.MaxValue)]
            [InlineData((byte)1, 1, 254)]
            [InlineData((byte)2, 1, 254)]
            [InlineData((byte)128, 1, 254)]
            [InlineData((byte)253, 1, 254)]
            [InlineData((byte)254, 1, 254)]
            [InlineData((byte)128, 128, 128)]
            public void DoesNotThrowExceptionWhenArgumentIsWithinRange(byte argumentValue,
                byte minimumValue, byte maximumValue)
            {
                Guard.ArgumentIsWithinRange("DummyArgumentName", argumentValue, minimumValue,
                        maximumValue);
            }
        }

        public class ArgumentIsWithinRange_String_Int16_Int16_Int16
        {
            [Theory]
            [InlineData(short.MinValue, (short)-32767, (short)32766)]
            [InlineData(short.MaxValue, (short)-32767, (short)32766)]
            [InlineData(short.MinValue, (short)0, (short)0)]
            [InlineData((short)-1, (short)0, (short)0)]
            [InlineData((short)1, (short)0, (short)0)]
            [InlineData(short.MaxValue, (short)0, (short)0)]
            public void ThrowsArgumentOutOfRangeExceptionWhenArgumentIsOutsideOfRange(short argumentValue,
                short minimumValue, short maximumValue)
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsWithinRange("DummyArgumentName", argumentValue, minimumValue,
                        maximumValue));
            }

            [Theory]
            [InlineData(short.MinValue, short.MinValue, short.MaxValue)]
            [InlineData((short)-32767, short.MinValue, short.MaxValue)]
            [InlineData((short)-1, short.MinValue, short.MaxValue)]
            [InlineData((short)0, short.MinValue, short.MaxValue)]
            [InlineData((short)1, short.MinValue, short.MaxValue)]
            [InlineData((short)32766, short.MinValue, short.MaxValue)]
            [InlineData(short.MaxValue, short.MinValue, short.MaxValue)]
            [InlineData((short)-32767, (short)-32767, 32766)]
            [InlineData((short)-32766, (short)-32767, 32766)]
            [InlineData((short)-1, (short)-32767, 32766)]
            [InlineData((short)0, (short)-32767, 32766)]
            [InlineData((short)1, (short)-32767, 32766)]
            [InlineData((short)32765, (short)-32767, 32766)]
            [InlineData((short)32766, (short)-32767, 32766)]
            [InlineData((short)0, 0, 0)]
            public void DoesNotThrowExceptionWhenArgumentIsWithinRange(short argumentValue,
                short minimumValue, short maximumValue)
            {
                Guard.ArgumentIsWithinRange("DummyArgumentName", argumentValue, minimumValue,
                        maximumValue);
            }
        }

        public class ArgumentIsWithinRange_String_Int32_Int32_Int32
        {
            [Theory]
            [InlineData(int.MinValue, (int)-2147483647, (int)2147483646)]
            [InlineData(int.MaxValue, (int)-2147483647, (int)2147483646)]
            [InlineData(int.MinValue, (int)0, (int)0)]
            [InlineData((int)-1, (int)0, (int)0)]
            [InlineData((int)1, (int)0, (int)0)]
            [InlineData(int.MaxValue, (int)0, (int)0)]
            public void ThrowsArgumentOutOfRangeExceptionWhenArgumentIsOutsideOfRange(int argumentValue,
                int minimumValue, int maximumValue)
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsWithinRange("DummyArgumentName", argumentValue, minimumValue,
                        maximumValue));
            }

            [Theory]
            [InlineData(int.MinValue, int.MinValue, int.MaxValue)]
            [InlineData((int)-2147483647, int.MinValue, int.MaxValue)]
            [InlineData((int)-1, int.MinValue, int.MaxValue)]
            [InlineData((int)0, int.MinValue, int.MaxValue)]
            [InlineData((int)1, int.MinValue, int.MaxValue)]
            [InlineData((int)2147483646, int.MinValue, int.MaxValue)]
            [InlineData(int.MaxValue, int.MinValue, int.MaxValue)]
            [InlineData((int)-2147483647, (int)-2147483647, 2147483646)]
            [InlineData((int)-2147483646, (int)-2147483647, 2147483646)]
            [InlineData((int)-1, (int)-2147483647, 2147483646)]
            [InlineData((int)0, (int)-2147483647, 2147483646)]
            [InlineData((int)1, (int)-2147483647, 2147483646)]
            [InlineData((int)2147483645, (int)-2147483647, 2147483646)]
            [InlineData((int)2147483646, (int)-2147483647, 2147483646)]
            [InlineData((int)0, 0, 0)]
            public void DoesNotThrowExceptionWhenArgumentIsWithinRange(int argumentValue,
                int minimumValue, int maximumValue)
            {
                Guard.ArgumentIsWithinRange("DummyArgumentName", argumentValue, minimumValue,
                        maximumValue);
            }
        }

        public class ArgumentIsWithinRange_String_Int64_Int64_Int64
        {
            [Theory]
            [InlineData(long.MinValue, (long)-9223372036854775807, (long)9223372036854775806)]
            [InlineData(long.MaxValue, (long)-9223372036854775807, (long)9223372036854775806)]
            [InlineData(long.MinValue, (long)0, (long)0)]
            [InlineData((long)-1, (long)0, (long)0)]
            [InlineData((long)1, (long)0, (long)0)]
            [InlineData(long.MaxValue, (long)0, (long)0)]
            public void ThrowsArgumentOutOfRangeExceptionWhenArgumentIsOutsideOfRange(long argumentValue,
                long minimumValue, long maximumValue)
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsWithinRange("DummyArgumentName", argumentValue, minimumValue,
                        maximumValue));
            }

            [Theory]
            [InlineData(long.MinValue, long.MinValue, long.MaxValue)]
            [InlineData((long)-9223372036854775807, long.MinValue, long.MaxValue)]
            [InlineData((long)-1, long.MinValue, long.MaxValue)]
            [InlineData((long)0, long.MinValue, long.MaxValue)]
            [InlineData((long)1, long.MinValue, long.MaxValue)]
            [InlineData((long)9223372036854775806, long.MinValue, long.MaxValue)]
            [InlineData(long.MaxValue, long.MinValue, long.MaxValue)]
            [InlineData((long)-9223372036854775807, (long)-9223372036854775807, 9223372036854775806)]
            [InlineData((long)-9223372036854775806, (long)-9223372036854775807, 9223372036854775806)]
            [InlineData((long)-1, (long)-9223372036854775807, 9223372036854775806)]
            [InlineData((long)0, (long)-9223372036854775807, 9223372036854775806)]
            [InlineData((long)1, (long)-9223372036854775807, 9223372036854775806)]
            [InlineData((long)9223372036854775805, (long)-9223372036854775807, 9223372036854775806)]
            [InlineData((long)9223372036854775806, (long)-9223372036854775807, 9223372036854775806)]
            [InlineData((long)0, 0, 0)]
            public void DoesNotThrowExceptionWhenArgumentIsWithinRange(long argumentValue,
                long minimumValue, long maximumValue)
            {
                Guard.ArgumentIsWithinRange("DummyArgumentName", argumentValue, minimumValue,
                        maximumValue);
            }
        }

        public class ArgumentIsWithinRange_String_Single_Single_Single
        {
            [Theory]
            [InlineData(float.MinValue, (float)-3.40281e+038f, (float)3.40281e+038f)]
            [InlineData(float.MaxValue, (float)-3.40281e+038f, (float)3.40281e+038f)]
            [InlineData(float.MinValue, (float)0, (float)0)]
            [InlineData((float)-1, (float)0, (float)0)]
            [InlineData((float)1, (float)0, (float)0)]
            [InlineData(float.MaxValue, (float)0, (float)0)]
            public void ThrowsArgumentOutOfRangeExceptionWhenArgumentIsOutsideOfRange(float argumentValue,
                float minimumValue, float maximumValue)
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsWithinRange("DummyArgumentName", argumentValue, minimumValue,
                        maximumValue));
            }

            [Theory]
            [InlineData(float.MinValue, float.MinValue, float.MaxValue)]
            [InlineData((float)-3.40281e+038f, float.MinValue, float.MaxValue)]
            [InlineData((float)-1, float.MinValue, float.MaxValue)]
            [InlineData((float)0, float.MinValue, float.MaxValue)]
            [InlineData((float)1, float.MinValue, float.MaxValue)]
            [InlineData((float)3.40281e+038f, float.MinValue, float.MaxValue)]
            [InlineData(float.MaxValue, float.MinValue, float.MaxValue)]
            [InlineData((float)-3.40281e+038f, (float)-3.40281e+038f, 3.40281e+038f)]
            [InlineData((float)-3.40280e+038f, (float)-3.40281e+038f, 3.40281e+038f)]
            [InlineData((float)-1, (float)-3.40281e+038f, 3.40281e+038f)]
            [InlineData((float)0, (float)-3.40281e+038f, 3.40281e+038f)]
            [InlineData((float)1, (float)-3.40281e+038f, 3.40281e+038f)]
            [InlineData((float)3.40280e+038f, (float)-3.40281e+038f, 3.40281e+038f)]
            [InlineData((float)3.40281e+038f, (float)-3.40281e+038f, 3.40281e+038f)]
            [InlineData((float)0, (float)0, (float)0)]
            public void DoesNotThrowExceptionWhenArgumentIsWithinRange(float argumentValue,
                float minimumValue, float maximumValue)
            {
                Guard.ArgumentIsWithinRange("DummyArgumentName", argumentValue, minimumValue,
                        maximumValue);
            }
        }

        public class ArgumentIsWithinRange_String_Double_Double_Double
        {
            [Theory]
            // TODO: Handle this without xunit crashing: [InlineData(double.MinValue, (double)-1.79768e+308, (double)1.79768e+308)]
            // TODO: Handle this without xunit crashing: [InlineData(double.MaxValue, (double)-1.79768e+308, (double)1.79768e+308)]
            // TODO: Handle this without xunit crashing: [InlineData(double.MinValue, (double)0, (double)0)]
            [InlineData((double)-1, (double)0, (double)0)]
            [InlineData((double)1, (double)0, (double)0)]
            // TODO: Handle this without xunit crashing: [InlineData(double.MaxValue, (double)0, (double)0)]
            public void ThrowsArgumentOutOfRangeExceptionWhenArgumentIsOutsideOfRange(double argumentValue,
                double minimumValue, double maximumValue)
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsWithinRange("DummyArgumentName", argumentValue, minimumValue,
                        maximumValue));
            }

            [Theory]
            // TODO: Handle this without xunit crashing: [InlineData(double.MinValue, double.MinValue, double.MaxValue)]
            // TODO: Handle this without xunit crashing: [InlineData((double)-1.79768e+308, double.MinValue, double.MaxValue)]
            // TODO: Handle this without xunit crashing: [InlineData((double)-1, double.MinValue, double.MaxValue)]
            // TODO: Handle this without xunit crashing: [InlineData((double)0, double.MinValue, double.MaxValue)]
            // TODO: Handle this without xunit crashing: [InlineData((double)1, double.MinValue, double.MaxValue)]
            // TODO: Handle this without xunit crashing: [InlineData((double)1.79768e+308, double.MinValue, double.MaxValue)]
            // TODO: Handle this without xunit crashing: [InlineData(double.MaxValue, double.MinValue, double.MaxValue)]
            [InlineData((double)-1.79768e+308, (double)-1.79768e+308, 1.79768e+308)]
            [InlineData((double)-1.79767e+308, (double)-1.79768e+308, 1.79768e+308)]
            [InlineData((double)-1, (double)-1.79768e+308, 1.79768e+308)]
            [InlineData((double)0, (double)-1.79768e+308, 1.79768e+308)]
            [InlineData((double)1, (double)-1.79768e+308, 1.79768e+308)]
            [InlineData((double)1.79767e+308, (double)-1.79768e+308, 1.79768e+308)]
            [InlineData((double)1.79768e+308, (double)-1.79768e+308, 1.79768e+308)]
            [InlineData((double)0, (double)0, (double)0)]
            public void DoesNotThrowExceptionWhenArgumentIsWithinRange(double argumentValue,
                double minimumValue, double maximumValue)
            {
                Guard.ArgumentIsWithinRange("DummyArgumentName", argumentValue, minimumValue,
                        maximumValue);
            }
        }

        public class ArgumentIsWithinRange_String_Decimal_Decimal_Decimal
        {
            [Theory]
            [MemberData("GetDecimalValuesForOutOfRangeEdgeCases")]
            public void ThrowsArgumentOutOfRangeExceptionWhenArgumentIsOutsideOfRange(decimal argumentValue,
                decimal minimumValue, decimal maximumValue)
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsWithinRange("DummyArgumentName", argumentValue, minimumValue,
                        maximumValue));
            }

            [Theory]
            [MemberData("GetDecimalValuesForWithinRangeEdgeCases")]
            public void DoesNotThrowExceptionWhenArgumentIsWithinRange(decimal argumentValue,
                decimal minimumValue, decimal maximumValue)
            {
                Guard.ArgumentIsWithinRange("DummyArgumentName", argumentValue, minimumValue,
                        maximumValue);
            }

            public static IEnumerable<object[]> GetDecimalValuesForOutOfRangeEdgeCases()
            {
                return new object[][]
                {
                    new object[]
                    {
                        decimal.MinValue,
                        -79228162514264337593543950334m,
                        79228162514264337593543950334m
                    },
                    new object[]
                    {
                        decimal.MaxValue,
                        -79228162514264337593543950334m,
                        79228162514264337593543950334m
                    },
                    new object[] { decimal.MinValue, decimal.Zero, decimal.Zero },
                    new object[] { decimal.MinusOne, decimal.Zero, decimal.Zero },
                    new object[] { decimal.One, decimal.Zero, decimal.Zero },
                    new object[] { decimal.MaxValue, decimal.Zero, decimal.Zero }
                };
            }

            public static IEnumerable<object[]> GetDecimalValuesForWithinRangeEdgeCases()
            {
                return new object[][]
                {
                    new object[] { decimal.MinValue, decimal.MinValue, decimal.MaxValue },
                    new object[] { -79228162514264337593543950334m, decimal.MinValue, decimal.MaxValue },
                    new object[] { decimal.MinusOne, decimal.MinValue, decimal.MaxValue },
                    new object[] { decimal.Zero, decimal.MinValue, decimal.MaxValue },
                    new object[] { decimal.One, decimal.MinValue, decimal.MaxValue },
                    new object[] { 79228162514264337593543950334m, decimal.MinValue, decimal.MaxValue },
                    new object[] { decimal.MaxValue, decimal.MinValue, decimal.MaxValue },
                    new object[]
                    {
                        -79228162514264337593543950334m,
                        -79228162514264337593543950334m,
                        79228162514264337593543950334m
                    },
                    new object[]
                    {
                        -79228162514264337593543950333m,
                        -79228162514264337593543950334m,
                        79228162514264337593543950334m
                    },
                    new object[]
                    {
                        decimal.MinusOne, -79228162514264337593543950334m, 79228162514264337593543950334m
                    },
                    new object[]
                    {
                        decimal.Zero, -79228162514264337593543950334m, 79228162514264337593543950334m
                    },
                    new object[]
                    {
                        decimal.One, -79228162514264337593543950334m, 79228162514264337593543950334m
                    },
                    new object[]
                    {
                        79228162514264337593543950333m,
                        -79228162514264337593543950334m,
                        79228162514264337593543950334m
                    },
                    new object[]
                    {
                        79228162514264337593543950334m,
                        -79228162514264337593543950334m,
                        79228162514264337593543950334m
                    },
                    new object[] { decimal.Zero, (decimal)0, (decimal)0 }
                };
            }
        }
    }
}
