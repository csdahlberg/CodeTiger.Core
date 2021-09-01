using System;
using System.Collections.Generic;
using CodeTiger;
using Xunit;

namespace UnitTests.CodeTiger
{
    public partial class GuardTests
    {
        public class ArgumentIsNotNegative_String_Int16
        {
            [Theory]
            [InlineData(short.MinValue)]
            [InlineData((short)-32767)]
            [InlineData((short)-1)]
            public void ThrowsArgumentOutOfRangeExceptionWhenArgumentIsNegative(short argumentValue)
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsNotNegative("DummyArgumentName", argumentValue));
            }

            [Theory]
            [InlineData((short)0)]
            [InlineData((short)1)]
            [InlineData((short)32766)]
            [InlineData(short.MaxValue)]
            public void DoesNotThrowExceptionWhenArgumentIsNotNegative(short argumentValue)
            {
                short actual = Guard.ArgumentIsNotNegative("DummyArgumentName", argumentValue);

                Assert.Equal(argumentValue, actual);
            }
        }

        public class ArgumentIsNotNegative_String_Int32
        {
            [Theory]
            [InlineData(int.MinValue)]
            [InlineData((int)-2147483647)]
            [InlineData((int)-1)]
            public void ThrowsArgumentOutOfRangeExceptionWhenArgumentIsNegative(int argumentValue)
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsNotNegative("DummyArgumentName", argumentValue));
            }

            [Theory]
            [InlineData((int)0)]
            [InlineData((int)1)]
            [InlineData((int)2147483646)]
            [InlineData(int.MaxValue)]
            public void DoesNotThrowExceptionWhenArgumentIsNotNegative(int argumentValue)
            {
                int actual = Guard.ArgumentIsNotNegative("DummyArgumentName", argumentValue);

                Assert.Equal(argumentValue, actual);
            }
        }

        public class ArgumentIsNotNegative_String_Int64
        {
            [Theory]
            [InlineData(long.MinValue)]
            [InlineData((long)-9223372036854775807)]
            [InlineData((long)-1)]
            public void ThrowsArgumentOutOfRangeExceptionWhenArgumentIsNegative(long argumentValue)
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsNotNegative("DummyArgumentName", argumentValue));
            }

            [Theory]
            [InlineData((long)0)]
            [InlineData((long)1)]
            [InlineData((long)9223372036854775806)]
            [InlineData(long.MaxValue)]
            public void DoesNotThrowExceptionWhenArgumentIsNotNegative(long argumentValue)
            {
                long actual = Guard.ArgumentIsNotNegative("DummyArgumentName", argumentValue);

                Assert.Equal(argumentValue, actual);
            }
        }

        public class ArgumentIsNotNegative_String_Single
        {
            [Theory]
            [InlineData(float.NegativeInfinity)]
            [InlineData(float.MinValue)]
            [InlineData((float)-3.40281E+38F)]
            [InlineData((float)-1)]
            [InlineData((float)-float.Epsilon)]
            public void ThrowsArgumentOutOfRangeExceptionWhenArgumentIsNegative(float argumentValue)
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsNotNegative("DummyArgumentName", argumentValue));
            }

            [Theory]
            [InlineData((float)0)]
            [InlineData(float.Epsilon)]
            [InlineData((float)1)]
            [InlineData((float)3.40281e+038f)]
            [InlineData(float.MaxValue)]
            [InlineData(float.PositiveInfinity)]
            public void DoesNotThrowExceptionWhenArgumentIsNotNegative(float argumentValue)
            {
                float actual = Guard.ArgumentIsNotNegative("DummyArgumentName", argumentValue);

                Assert.Equal(argumentValue, actual);
            }
        }

        public class ArgumentIsNotNegative_String_Double
        {
            [Theory]
            [InlineData(double.NegativeInfinity)]
            // TODO: Handle this without xunit crashing: [InlineData(double.MinValue)]
            [InlineData((double)-1.79768e+308)]
            [InlineData((double)-1)]
            [InlineData((double)-double.Epsilon)]
            public void ThrowsArgumentOutOfRangeExceptionWhenArgumentIsNegative(double argumentValue)
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsNotNegative("DummyArgumentName", argumentValue));
            }

            [Theory]
            [InlineData((double)0)]
            [InlineData(double.Epsilon)]
            [InlineData((double)1)]
            [InlineData((double)1.79768e+308)]
            // TODO: Handle this without xunit crashing: [InlineData(double.MaxValue)]
            [InlineData(double.PositiveInfinity)]
            public void DoesNotThrowExceptionWhenArgumentIsNotNegative(double argumentValue)
            {
                double actual = Guard.ArgumentIsNotNegative("DummyArgumentName", argumentValue);

                Assert.Equal(argumentValue, actual);
            }
        }

        public class ArgumentIsNotNegative_String_Decimal
        {
            [Theory]
            [MemberData(nameof(GetNegativeDecimalValuesForEdgeCases))]
            public void ThrowsArgumentOutOfRangeExceptionWhenArgumentIsNegative(decimal argumentValue)
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsNotNegative("DummyArgumentName", argumentValue));
            }

            [Theory]
            [MemberData(nameof(GetNonNegativeDecimalValuesForEdgeCases))]
            public void DoesNotThrowExceptionWhenArgumentIsNotNegative(decimal argumentValue)
            {
                decimal actual = Guard.ArgumentIsNotNegative("DummyArgumentName", argumentValue);

                Assert.Equal(argumentValue, actual);
            }

            private static IEnumerable<object[]> GetNegativeDecimalValuesForEdgeCases()
            {
                return new object[][]
                {
                    new object[] { decimal.MinValue },
                    new object[] { -79228162514264337593543950334m },
                    new object[] { decimal.MinusOne },
                };
            }

            private static IEnumerable<object[]> GetNonNegativeDecimalValuesForEdgeCases()
            {
                return new object[][]
                {
                    new object[] { decimal.Zero },
                    new object[] { decimal.One },
                    new object[] { 79228162514264337593543950334m },
                    new object[] { decimal.MaxValue },
                };
            }
        }
    }
}
