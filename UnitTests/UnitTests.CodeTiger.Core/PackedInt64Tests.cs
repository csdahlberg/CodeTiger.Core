using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CodeTiger;
using Xunit;

namespace UnitTests.CodeTiger
{
    public class PackedInt64Tests
    {
        public class LowerInt32
        {
            [Theory]
            [MemberData(nameof(GetInt32ValuesForEdgeCases))]
            public void GetterReturnsLowerInt32PassedInToConstructor(int lowerValue, int upperValue)
            {
                var target = new PackedInt64(lowerValue, upperValue);

                Assert.Equal(lowerValue, target.LowerInt32);
            }

            public static IEnumerable<object[]> GetInt32ValuesForEdgeCases()
            {
                int[] edgeCaseValues = new int[]
                {
                    int.MinValue, int.MinValue + 1, -1, 0, 1, int.MaxValue - 1, int.MaxValue
                };

                for (int i = 0; i < edgeCaseValues.Length; i++)
                {
                    for (int j = 0; j < edgeCaseValues.Length; j++)
                    {
                        yield return new object[] { edgeCaseValues[i], edgeCaseValues[j] };
                    }
                }
            }
        }

        public class UpperInt32
        {
            [Theory]
            [MemberData(nameof(GetInt32ValuesForEdgeCases))]
            public void GetterReturnsUpperInt32PassedInToConstructor(int lowerValue, int upperValue)
            {
                var target = new PackedInt64(lowerValue, upperValue);

                Assert.Equal(upperValue, target.UpperInt32);
            }

            public static IEnumerable<object[]> GetInt32ValuesForEdgeCases()
            {
                int[] edgeCaseValues = new int[]
                {
                    int.MinValue, int.MinValue + 1, -1, 0, 1, int.MaxValue - 1, int.MaxValue
                };

                for (int i = 0; i < edgeCaseValues.Length; i++)
                {
                    for (int j = 0; j < edgeCaseValues.Length; j++)
                    {
                        yield return new object[] { edgeCaseValues[i], edgeCaseValues[j] };
                    }
                }
            }
        }

        public class GetInt32Values
        {
            [Theory]
            [MemberData(nameof(GetInt32ValuesForEdgeCases))]
            public void GetterReturnsArrayWithValuesPassedInToConstructor(int lowerValue, int upperValue)
            {
                var target = new PackedInt64(lowerValue, upperValue);

                int[] actual = target.GetInt32Values();

                Assert.Equal(lowerValue, actual[0]);
                Assert.Equal(upperValue, actual[1]);
            }

            public static IEnumerable<object[]> GetInt32ValuesForEdgeCases()
            {
                int[] edgeCaseValues = new int[]
                {
                    int.MinValue, int.MinValue + 1, -1, 0, 1, int.MaxValue - 1, int.MaxValue
                };

                for (int i = 0; i < edgeCaseValues.Length; i++)
                {
                    for (int j = 0; j < edgeCaseValues.Length; j++)
                    {
                        yield return new object[] { edgeCaseValues[i], edgeCaseValues[j] };
                    }
                }
            }
        }

        public class GetInt16Values
        {
            [Theory]
            [MemberData(nameof(GetInt16ValuesForEdgeCases))]
            public void GetterReturnsArrayWithValuesPassedInToConstructor(short value0, short value1, short value2,
                short value3)
            {
                var target = new PackedInt64(new[] { value0, value1, value2, value3 });

                short[] actual = target.GetInt16Values();

                Assert.Equal(value0, actual[0]);
                Assert.Equal(value1, actual[1]);
                Assert.Equal(value2, actual[2]);
                Assert.Equal(value3, actual[3]);
            }

            public static IEnumerable<object[]> GetInt16ValuesForEdgeCases()
            {
                short[] edgeCaseValues = new short[]
                {
                    short.MinValue, short.MinValue + 1, -1, 0, 1, short.MaxValue - 1, short.MaxValue
                };

                for (int i = 0; i < edgeCaseValues.Length; i++)
                {
                    for (int j = 0; j < edgeCaseValues.Length; j++)
                    {
                        for (int k = 0; k < edgeCaseValues.Length; k++)
                        {
                            for (int l = 0; l < edgeCaseValues.Length; l++)
                            {
                                yield return new object[]
                                {
                                    edgeCaseValues[i], edgeCaseValues[j], edgeCaseValues[k], edgeCaseValues[l]
                                };
                            }
                        }
                    }
                }
            }
        }

        public class GetByteValues
        {
            [Theory]
            [MemberData(nameof(GetByteValuesForEdgeCases))]
            [SuppressMessage("CodeTiger.Design", "CT1003:Methods should not exceed seven parameters.",
                Justification = "This test requires more than seven generated arguments.")]
            public void GetterReturnsArrayWithValuesPassedInToConstructor(byte value0, byte value1, byte value2,
                byte value3, byte value4, byte value5, byte value6, byte value7)
            {
                var target = new PackedInt64(new[]
                {
                    value0, value1, value2, value3, value4, value5, value6, value7
                });

                byte[] actual = target.GetByteValues();

                Assert.Equal(value0, actual[0]);
                Assert.Equal(value1, actual[1]);
                Assert.Equal(value2, actual[2]);
                Assert.Equal(value3, actual[3]);
                Assert.Equal(value4, actual[4]);
                Assert.Equal(value5, actual[5]);
                Assert.Equal(value6, actual[6]);
                Assert.Equal(value7, actual[7]);
            }

            public static IEnumerable<object[]> GetByteValuesForEdgeCases()
            {
                byte[] edgeCaseValues = new byte[] { byte.MinValue, 1, 254, byte.MaxValue };

                for (int i = 0; i < edgeCaseValues.Length; i++)
                {
                    for (int j = 0; j < edgeCaseValues.Length; j++)
                    {
                        for (int k = 0; k < edgeCaseValues.Length; k++)
                        {
                            for (int l = 0; l < edgeCaseValues.Length; l++)
                            {
                                yield return new object[]
                                {
                                    edgeCaseValues[i],
                                    edgeCaseValues[j],
                                    edgeCaseValues[k],
                                    edgeCaseValues[l],
                                    edgeCaseValues[i],
                                    edgeCaseValues[j],
                                    edgeCaseValues[k],
                                    edgeCaseValues[l]
                                };
                            }
                        }
                    }
                }
            }
        }

        public class Constructor
        {
            [Fact]
            public void SetsValueToZero()
            {
                var target = new PackedInt64();

                Assert.Equal(0, target.Value);
            }
        }

        public class Constructor_Int64
        {
            [Theory]
            [InlineData(long.MinValue)]
            [InlineData(-9223372036854775807)]
            [InlineData(-1)]
            [InlineData(0)]
            [InlineData(1)]
            [InlineData(9223372036854775806)]
            [InlineData(long.MaxValue)]
            public void SetsValueToValuePassedIn(long initialValue)
            {
                var target = new PackedInt64(initialValue);

                Assert.Equal(initialValue, target.Value);
            }
        }

        public class Constructor_Int32_Int32
        {
            [Theory]
            [InlineData((int)0, (int)-2147483648, long.MinValue)]
            [InlineData((int)1, (int)-2147483648, -9223372036854775807)]
            [InlineData((int)-2, (int)-1, -2)]
            [InlineData((int)-1, (int)-1, -1)]
            [InlineData((int)0, (int)0, 0)]
            [InlineData((int)1, (int)0, 1)]
            [InlineData((int)2, (int)0, 2)]
            [InlineData((int)-2, (int)2147483647, 9223372036854775806)]
            [InlineData((int)-1, (int)2147483647, long.MaxValue)]
            public void SetsValueToCombinationOfValuesPassedIn(int lowerValue, int upperValue,
                long expectedValue)
            {
                var target = new PackedInt64(lowerValue, upperValue);

                Assert.Equal(expectedValue, target.Value);
            }
        }

        public class Constructor_Int32Array
        {
            [Theory]
            [InlineData((int)0, (int)-2147483648, long.MinValue)]
            [InlineData((int)1, (int)-2147483648, -9223372036854775807)]
            [InlineData((int)-2, (int)-1, -2)]
            [InlineData((int)-1, (int)-1, -1)]
            [InlineData((int)0, (int)0, 0)]
            [InlineData((int)1, (int)0, 1)]
            [InlineData((int)2, (int)0, 2)]
            [InlineData((int)-2, (int)2147483647, 9223372036854775806)]
            [InlineData((int)-1, (int)2147483647, long.MaxValue)]
            public void SetsValueToCombinationOfValuesPassedIn(int lowerValue, int upperValue,
                long expectedValue)
            {
                var target = new PackedInt64(new int[] { lowerValue, upperValue });

                Assert.Equal(expectedValue, target.Value);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenValueIsNull()
            {
                Assert.Throws<ArgumentNullException>(() => new PackedInt64((int[])null));
            }

            [Theory]
            [InlineData(0)]
            [InlineData(1)]
            [InlineData(3)]
            [InlineData(4)]
            public void ThrowsArgumentOutOfRangeExceptionExceptionWhenValueHasIncorrectNumberOfElements(
                int numberOfElements)
            {
                int[] values = new int[numberOfElements];

                Assert.Throws<ArgumentException>(() => new PackedInt64(values));
            }
        }

        public class Constructor_Int16Array
        {
            [Theory]
            [InlineData((short)0, (short)0, (short)0, (short)-32768, long.MinValue)]
            [InlineData((short)1, (short)0, (short)0, (short)-32768, -9223372036854775807)]
            [InlineData((short)-2, (short)-1, (short)-1, (short)-1, -2)]
            [InlineData((short)-1, (short)-1, (short)-1, (short)-1, -1)]
            [InlineData((short)0, (short)0, (short)0, (short)0, 0)]
            [InlineData((short)1, (short)0, (short)0, (short)0, 1)]
            [InlineData((short)2, (short)0, (short)0, (short)0, 2)]
            [InlineData((short)-2, (short)-1, (short)-1, (short)32767, 9223372036854775806)]
            [InlineData((short)-1, (short)-1, (short)-1, (short)32767, long.MaxValue)]
            public void SetsValueToCombinationOfValuesPassedIn(short value0, short value1,
                short value2, short value3, long expectedValue)
            {
                var target = new PackedInt64(new[] { value0, value1, value2, value3 });

                Assert.Equal(expectedValue, target.Value);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenValueIsNull()
            {
                Assert.Throws<ArgumentNullException>(() => new PackedInt64((short[])null));
            }

            [Theory]
            [InlineData(0)]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(3)]
            [InlineData(5)]
            [InlineData(6)]
            [InlineData(7)]
            [InlineData(8)]
            public void ThrowsArgumentOutOfRangeExceptionExceptionWhenValueHasIncorrectNumberOfElements(
                int numberOfElements)
            {
                short[] values = new short[numberOfElements];

                Assert.Throws<ArgumentException>(() => new PackedInt64(values));
            }
        }

        public class Constructor_ByteArray
        {
            [Theory]
            [InlineData((byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)128, long.MinValue)]
            [InlineData((byte)1, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)128,
                -9223372036854775807)]
            [InlineData((byte)254, (byte)255, (byte)255, (byte)255, (byte)255, (byte)255, (byte)255, (byte)255,
                -2)]
            [InlineData((byte)255, (byte)255, (byte)255, (byte)255, (byte)255, (byte)255, (byte)255, (byte)255,
                -1)]
            [InlineData((byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, 0)]
            [InlineData((byte)1, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, 1)]
            [InlineData((byte)2, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, 2)]
            [InlineData((byte)254, (byte)255, (byte)255, (byte)255, (byte)255, (byte)255, (byte)255, (byte)127,
                9223372036854775806)]
            [InlineData((byte)255, (byte)255, (byte)255, (byte)255, (byte)255, (byte)255, (byte)255, (byte)127,
                long.MaxValue)]
            [SuppressMessage("CodeTiger.Design", "CT1003:Methods should not exceed seven parameters.",
                Justification = "This test requires more than seven generated arguments.")]
            public void SetsValueToCombinationOfValuesPassedIn(byte value0, byte value1, byte value2,
                byte value3, byte value4, byte value5, byte value6, byte value7, long expectedValue)
            {
                var target = new PackedInt64(new[]
                {
                    value0, value1, value2, value3, value4, value5, value6, value7
                });

                Assert.Equal(expectedValue, target.Value);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenValueIsNull()
            {
                Assert.Throws<ArgumentNullException>(() => new PackedInt64((byte[])null));
            }

            [Theory]
            [InlineData(0)]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(3)]
            [InlineData(4)]
            [InlineData(5)]
            [InlineData(6)]
            [InlineData(7)]
            [InlineData(9)]
            [InlineData(10)]
            [InlineData(11)]
            [InlineData(12)]
            [InlineData(13)]
            [InlineData(14)]
            [InlineData(15)]
            [InlineData(16)]
            public void ThrowsArgumentOutOfRangeExceptionExceptionWhenValueHasIncorrectNumberOfElements(
                int numberOfElements)
            {
                byte[] values = new byte[numberOfElements];

                Assert.Throws<ArgumentException>(() => new PackedInt64(values));
            }
        }

        public class WithLowerInt32
        {
            [Theory]
            [InlineData(long.MinValue, int.MinValue, (long)-9223372034707292160)]
            [InlineData(long.MinValue, (int)-2147483647, (long)-9223372034707292159)]
            [InlineData(long.MinValue, (int)-1, (long)-9223372032559808513)]
            [InlineData(long.MinValue, (int)0, (long)-9223372036854775808)]
            [InlineData(long.MinValue, (int)1, (long)-9223372036854775807)]
            [InlineData(long.MinValue, (int)2147483646, (long)-9223372034707292162)]
            [InlineData(long.MinValue, int.MaxValue, (long)-9223372034707292161)]
            [InlineData((long)-9223372036854775807, int.MinValue, (long)-9223372034707292160)]
            [InlineData((long)-9223372036854775807, (int)-2147483647, (long)-9223372034707292159)]
            [InlineData((long)-9223372036854775807, (int)-1, (long)-9223372032559808513)]
            [InlineData((long)-9223372036854775807, (int)0, (long)-9223372036854775808)]
            [InlineData((long)-9223372036854775807, (int)1, (long)-9223372036854775807)]
            [InlineData((long)-9223372036854775807, (int)2147483646, (long)-9223372034707292162)]
            [InlineData((long)-9223372036854775807, int.MaxValue, (long)-9223372034707292161)]
            [InlineData((long)-1, int.MinValue, (long)-2147483648)]
            [InlineData((long)-1, (int)-2147483647, (long)-2147483647)]
            [InlineData((long)-1, (int)-1, (long)-1)]
            [InlineData((long)-1, (int)0, (long)-4294967296)]
            [InlineData((long)-1, (int)1, (long)-4294967295)]
            [InlineData((long)-1, (int)2147483646, (long)-2147483650)]
            [InlineData((long)-1, int.MaxValue, (long)-2147483649)]
            [InlineData((long)0, int.MinValue, (long)2147483648)]
            [InlineData((long)0, (int)-2147483647, (long)2147483649)]
            [InlineData((long)0, (int)-1, (long)4294967295)]
            [InlineData((long)0, (int)0, (long)0)]
            [InlineData((long)0, (int)1, (long)1)]
            [InlineData((long)0, (int)2147483646, (long)2147483646)]
            [InlineData((long)0, int.MaxValue, (long)2147483647)]
            [InlineData((long)1, int.MinValue, (long)2147483648)]
            [InlineData((long)1, (int)-2147483647, (long)2147483649)]
            [InlineData((long)1, (int)-1, (long)4294967295)]
            [InlineData((long)1, (int)0, (long)0)]
            [InlineData((long)1, (int)1, (long)1)]
            [InlineData((long)1, (int)2147483646, (long)2147483646)]
            [InlineData((long)1, int.MaxValue, (long)2147483647)]
            [InlineData((long)9223372036854775806, int.MinValue, (long)9223372034707292160)]
            [InlineData((long)9223372036854775806, (int)-2147483647, (long)9223372034707292161)]
            [InlineData((long)9223372036854775806, (int)-1, (long)9223372036854775807)]
            [InlineData((long)9223372036854775806, (int)0, (long)9223372032559808512)]
            [InlineData((long)9223372036854775806, (int)1, (long)9223372032559808513)]
            [InlineData((long)9223372036854775806, (int)2147483646, (long)9223372034707292158)]
            [InlineData((long)9223372036854775806, int.MaxValue, (long)9223372034707292159)]
            [InlineData(long.MaxValue, int.MinValue, (long)9223372034707292160)]
            [InlineData(long.MaxValue, (int)-2147483647, (long)9223372034707292161)]
            [InlineData(long.MaxValue, (int)-1, (long)9223372036854775807)]
            [InlineData(long.MaxValue, (int)0, (long)9223372032559808512)]
            [InlineData(long.MaxValue, (int)1, (long)9223372032559808513)]
            [InlineData(long.MaxValue, (int)2147483646, (long)9223372034707292158)]
            [InlineData(long.MaxValue, int.MaxValue, (long)9223372034707292159)]
            public void ReturnsPackedInt64WithCorrectValue(long originalValue, int newLowerInt32, long newValue)
            {
                var target = new PackedInt64(originalValue);

                var actual = target.WithLowerInt32(newLowerInt32);

                Assert.Equal(newValue, actual.Value);
            }
        }

        public class WithUpperInt32
        {
            [Theory]
            [InlineData(long.MinValue, int.MinValue, (long)-9223372036854775808)]
            [InlineData(long.MinValue, (int)-2147483647, (long)-9223372032559808512)]
            [InlineData(long.MinValue, (int)-1, (long)-4294967296)]
            [InlineData(long.MinValue, (int)0, (long)0)]
            [InlineData(long.MinValue, (int)1, (long)4294967296)]
            [InlineData(long.MinValue, (int)2147483646, (long)9223372028264841216)]
            [InlineData(long.MinValue, int.MaxValue, (long)9223372032559808512)]
            [InlineData((long)-9223372036854775807, int.MinValue, (long)-9223372036854775807)]
            [InlineData((long)-9223372036854775807, (int)-2147483647, (long)-9223372032559808511)]
            [InlineData((long)-9223372036854775807, (int)-1, (long)-4294967295)]
            [InlineData((long)-9223372036854775807, (int)0, (long)1)]
            [InlineData((long)-9223372036854775807, (int)1, (long)4294967297)]
            [InlineData((long)-9223372036854775807, (int)2147483646, (long)9223372028264841217)]
            [InlineData((long)-9223372036854775807, int.MaxValue, (long)9223372032559808513)]
            [InlineData((long)-1, int.MinValue, (long)-9223372032559808513)]
            [InlineData((long)-1, (int)-2147483647, (long)-9223372028264841217)]
            [InlineData((long)-1, (int)-1, (long)-1)]
            [InlineData((long)-1, (int)0, (long)4294967295)]
            [InlineData((long)-1, (int)1, (long)8589934591)]
            [InlineData((long)-1, (int)2147483646, (long)9223372032559808511)]
            [InlineData((long)-1, int.MaxValue, (long)9223372036854775807)]
            [InlineData((long)0, int.MinValue, (long)-9223372036854775808)]
            [InlineData((long)0, (int)-2147483647, (long)-9223372032559808512)]
            [InlineData((long)0, (int)-1, (long)-4294967296)]
            [InlineData((long)0, (int)0, (long)0)]
            [InlineData((long)0, (int)1, (long)4294967296)]
            [InlineData((long)0, (int)2147483646, (long)9223372028264841216)]
            [InlineData((long)0, int.MaxValue, (long)9223372032559808512)]
            [InlineData((long)1, int.MinValue, (long)-9223372036854775807)]
            [InlineData((long)1, (int)-2147483647, (long)-9223372032559808511)]
            [InlineData((long)1, (int)-1, (long)-4294967295)]
            [InlineData((long)1, (int)0, (long)1)]
            [InlineData((long)1, (int)1, (long)4294967297)]
            [InlineData((long)1, (int)2147483646, (long)9223372028264841217)]
            [InlineData((long)1, int.MaxValue, (long)9223372032559808513)]
            [InlineData((long)9223372036854775806, int.MinValue, (long)-9223372032559808514)]
            [InlineData((long)9223372036854775806, (int)-2147483647, (long)-9223372028264841218)]
            [InlineData((long)9223372036854775806, (int)-1, (long)-2)]
            [InlineData((long)9223372036854775806, (int)0, (long)4294967294)]
            [InlineData((long)9223372036854775806, (int)1, (long)8589934590)]
            [InlineData((long)9223372036854775806, (int)2147483646, (long)9223372032559808510)]
            [InlineData((long)9223372036854775806, int.MaxValue, (long)9223372036854775806)]
            [InlineData(long.MaxValue, int.MinValue, (long)-9223372032559808513)]
            [InlineData(long.MaxValue, (int)-2147483647, (long)-9223372028264841217)]
            [InlineData(long.MaxValue, (int)-1, (long)-1)]
            [InlineData(long.MaxValue, (int)0, (long)4294967295)]
            [InlineData(long.MaxValue, (int)1, (long)8589934591)]
            [InlineData(long.MaxValue, (int)2147483646, (long)9223372032559808511)]
            [InlineData(long.MaxValue, int.MaxValue, (long)9223372036854775807)]
            public void ReturnsPackedInt64WithCorrectValue(long originalValue, int newUpperInt32, long newValue)
            {
                var target = new PackedInt64(originalValue);

                var actual = target.WithUpperInt32(newUpperInt32);

                Assert.Equal(newValue, actual.Value);
            }
        }

        public class OperatorFromInt64
        {
            [Theory]
            [InlineData(long.MinValue)]
            [InlineData((long)-9223372036854775807)]
            [InlineData((long)-1)]
            [InlineData((long)0)]
            [InlineData((long)1)]
            [InlineData((long)9223372036854775806)]
            [InlineData(long.MaxValue)]
            public void CreatesPackedInt64WithSameValue(long value)
            {
                PackedInt64 target = value;

                Assert.Equal(value, target.Value);
            }
        }

        public class OperatorToInt64
        {
            [Theory]
            [InlineData(long.MinValue)]
            [InlineData((long)-9223372036854775807)]
            [InlineData((long)-1)]
            [InlineData((long)0)]
            [InlineData((long)1)]
            [InlineData((long)9223372036854775806)]
            [InlineData(long.MaxValue)]
            public void CreatesInt64WithSameValue(long value)
            {
                var packedInt64 = new PackedInt64(value);

                long actual = packedInt64;

                Assert.Equal(value, actual);
            }
        }
    }
}