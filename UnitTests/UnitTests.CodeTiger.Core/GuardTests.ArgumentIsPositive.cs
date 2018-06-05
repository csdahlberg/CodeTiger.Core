using System;
using System.Collections.Generic;
using CodeTiger;
using Xunit;

namespace UnitTests.CodeTiger
{
    public partial class GuardTests
    {
        public class ArgumentIsPositive_String_Int16
        {
            [Theory]
            [InlineData(short.MinValue)]
            [InlineData((short)-32767)]
            [InlineData((short)-1)]
            [InlineData((short)0)]
            public void ThrowsArgumentOutOfRangeExceptionWhenArgumentIsNotPositive(short argumentValue)
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsPositive("DummyArgumentName", argumentValue));
            }

            [Theory]
            [InlineData((short)1)]
            [InlineData((short)32766)]
            [InlineData(short.MaxValue)]
            public void DoesNotThrowExceptionWhenArgumentIsPositive(short argumentValue)
            {
                short actual = Guard.ArgumentIsPositive("DummyArgumentName", argumentValue);

                Assert.Equal(argumentValue, actual);
            }
        }

        public class ArgumentIsPositive_String_Int32
        {
            [Theory]
            [InlineData(int.MinValue)]
            [InlineData((int)-2147483647)]
            [InlineData((int)-1)]
            [InlineData((int)0)]
            public void ThrowsArgumentOutOfRangeExceptionWhenArgumentIsNotPositive(int argumentValue)
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsPositive("DummyArgumentName", argumentValue));
            }

            [Theory]
            [InlineData((int)1)]
            [InlineData((int)2147483646)]
            [InlineData(int.MaxValue)]
            public void DoesNotThrowExceptionWhenArgumentIsPositive(int argumentValue)
            {
                int actual = Guard.ArgumentIsPositive("DummyArgumentName", argumentValue);

                Assert.Equal(argumentValue, actual);
            }
        }

        public class ArgumentIsPositive_String_Int64
        {
            [Theory]
            [InlineData(long.MinValue)]
            [InlineData((long)-9223372036854775807)]
            [InlineData((long)-1)]
            [InlineData((long)0)]
            public void ThrowsArgumentOutOfRangeExceptionWhenArgumentIsNotPositive(long argumentValue)
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsPositive("DummyArgumentName", argumentValue));
            }

            [Theory]
            [InlineData((long)1)]
            [InlineData((long)9223372036854775806)]
            [InlineData(long.MaxValue)]
            public void DoesNotThrowExceptionWhenArgumentIsPositive(long argumentValue)
            {
                long actual = Guard.ArgumentIsPositive("DummyArgumentName", argumentValue);

                Assert.Equal(argumentValue, actual);
            }
        }

        public class ArgumentIsPositive_String_Single
        {
            [Theory]
            [InlineData(float.NegativeInfinity)]
            [InlineData(float.MinValue)]
            [InlineData((float)-3.40281E+38F)]
            [InlineData((float)-1)]
            [InlineData((float)-float.Epsilon)]
            [InlineData((float)0)]
            public void ThrowsArgumentOutOfRangeExceptionWhenArgumentIsNotPositive(float argumentValue)
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsPositive("DummyArgumentName", argumentValue));
            }

            [Theory]
            [InlineData(float.Epsilon)]
            [InlineData((float)1)]
            [InlineData((float)3.40281e+038f)]
            [InlineData(float.MaxValue)]
            [InlineData(float.PositiveInfinity)]
            public void DoesNotThrowExceptionWhenArgumentIsPositive(float argumentValue)
            {
                float actual = Guard.ArgumentIsPositive("DummyArgumentName", argumentValue);

                Assert.Equal(argumentValue, actual);
            }
        }

        public class ArgumentIsPositive_String_Double
        {
            [Theory]
            [InlineData(double.NegativeInfinity)]
            // TODO: Handle this without xunit crashing: [InlineData(double.MinValue)]
            [InlineData((double)-1.79768e+308)]
            [InlineData((double)-1)]
            [InlineData((double)-double.Epsilon)]
            [InlineData((double)0)]
            public void ThrowsArgumentOutOfRangeExceptionWhenArgumentIsNotPositive(double argumentValue)
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsPositive("DummyArgumentName", argumentValue));
            }

            [Theory]
            [InlineData(double.Epsilon)]
            [InlineData((double)1)]
            [InlineData((double)1.79768e+308)]
            // TODO: Handle this without xunit crashing: [InlineData(double.MaxValue)]
            [InlineData(double.PositiveInfinity)]
            public void DoesNotThrowExceptionWhenArgumentIsPositive(double argumentValue)
            {
                double actual = Guard.ArgumentIsPositive("DummyArgumentName", argumentValue);

                Assert.Equal(argumentValue, actual);
            }
        }

        public class ArgumentIsPositive_String_Decimal
        {
            [Theory]
            [MemberData(nameof(GetNonPositiveDecimalValuesForEdgeCases))]
            public void ThrowsArgumentOutOfRangeExceptionWhenArgumentIsNotPositive(decimal argumentValue)
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsPositive("DummyArgumentName", argumentValue));
            }

            [Theory]
            [MemberData(nameof(GetPositiveDecimalValuesForEdgeCases))]
            public void DoesNotThrowExceptionWhenArgumentIsPositive(decimal argumentValue)
            {
                decimal actual = Guard.ArgumentIsPositive("DummyArgumentName", argumentValue);

                Assert.Equal(argumentValue, actual);
            }

            public static IEnumerable<object[]> GetNonPositiveDecimalValuesForEdgeCases()
            {
                return new object[][]
                {
                    new object[] { decimal.MinValue },
                    new object[] { (decimal)-79228162514264337593543950334m },
                    new object[] { decimal.MinusOne },
                    new object[] { decimal.Zero },
                };
            }

            public static IEnumerable<object[]> GetPositiveDecimalValuesForEdgeCases()
            {
                return new object[][]
                {
                    new object[] { decimal.One },
                    new object[] { (decimal)79228162514264337593543950334m },
                    new object[] { decimal.MaxValue },
                };
            }
        }
    }
}
