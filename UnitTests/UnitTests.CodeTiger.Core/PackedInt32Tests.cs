using System;
using System.Collections.Generic;
using CodeTiger;
using Xunit;

namespace UnitTests.CodeTiger
{
    public static class PackedInt32Tests
    {
        public class LowerInt16
        {
            [Theory]
            [MemberData(nameof(GetInt16ValuesForEdgeCases))]
            public void GetterReturnsLowerInt16PassedInToConstructor(short lowerValue, short upperValue)
            {
                var target = new PackedInt32(lowerValue, upperValue);

                Assert.Equal(lowerValue, target.LowerInt16);
            }

            private static IEnumerable<object[]> GetInt16ValuesForEdgeCases()
            {
                short[] edgeCaseValues = new short[]
                {
                    short.MinValue, short.MinValue + 1, -1, 0, 1, short.MaxValue - 1, short.MaxValue
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

        public class UpperInt16
        {
            [Theory]
            [MemberData(nameof(GetInt16ValuesForEdgeCases))]
            public void GetterReturnsUpperInt16PassedInToConstructor(short lowerValue, short upperValue)
            {
                var target = new PackedInt32(lowerValue, upperValue);

                Assert.Equal(upperValue, target.UpperInt16);
            }

            private static IEnumerable<object[]> GetInt16ValuesForEdgeCases()
            {
                short[] edgeCaseValues = new short[]
                {
                    short.MinValue, short.MinValue + 1, -1, 0, 1, short.MaxValue - 1, short.MaxValue
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
            public void GetterReturnsArrayWithValuesPassedInToConstructor(short lowerValue, short upperValue)
            {
                var target = new PackedInt32(lowerValue, upperValue);

                short[] actual = target.GetInt16Values();

                Assert.Equal(lowerValue, actual[0]);
                Assert.Equal(upperValue, actual[1]);
            }

            private static IEnumerable<object[]> GetInt16ValuesForEdgeCases()
            {
                short[] edgeCaseValues = new short[]
                {
                    short.MinValue, short.MinValue + 1, -1, 0, 1, short.MaxValue - 1, short.MaxValue
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

        public class GetByteValues
        {
            [Theory]
            [MemberData(nameof(GetByteValuesForEdgeCases))]
            public void GetterReturnsArrayWithValuesPassedInToConstructor(byte value0, byte value1, byte value2,
                byte value3)
            {
                var target = new PackedInt32(new[] { value0, value1, value2, value3 });

                byte[] actual = target.GetByteValues();

                Assert.Equal(value0, actual[0]);
                Assert.Equal(value1, actual[1]);
                Assert.Equal(value2, actual[2]);
                Assert.Equal(value3, actual[3]);
            }

            private static IEnumerable<object[]> GetByteValuesForEdgeCases()
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
                                    edgeCaseValues[i], edgeCaseValues[j], edgeCaseValues[k], edgeCaseValues[l]
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
                var target = new PackedInt32();

                Assert.Equal(0, target.Value);
            }
        }

        public class Constructor_Int32
        {
            [Theory]
            [InlineData(int.MinValue)]
            [InlineData(-2147483647)]
            [InlineData(-1)]
            [InlineData(0)]
            [InlineData(1)]
            [InlineData(2147483646)]
            [InlineData(int.MaxValue)]
            public void SetsValueToValuePassedIn(int initialValue)
            {
                var target = new PackedInt32(initialValue);

                Assert.Equal(initialValue, target.Value);
            }
        }

        public class Constructor_Int16_Int16
        {
            [Theory]
            [InlineData((short)0, (short)-32768, int.MinValue)]
            [InlineData((short)1, (short)-32768, -2147483647)]
            [InlineData((short)-2, (short)-1, -2)]
            [InlineData((short)-1, (short)-1, -1)]
            [InlineData((short)0, (short)0, 0)]
            [InlineData((short)1, (short)0, 1)]
            [InlineData((short)2, (short)0, 2)]
            [InlineData((short)-2, (short)32767, 2147483646)]
            [InlineData((short)-1, (short)32767, int.MaxValue)]
            public void SetsValueToCombinationOfValuesPassedIn(short lowerValue, short upperValue,
                int expectedValue)
            {
                var target = new PackedInt32(lowerValue, upperValue);

                Assert.Equal(expectedValue, target.Value);
            }
        }

        public class Constructor_Int16Array
        {
            [Theory]
            [InlineData((short)0, (short)-32768, int.MinValue)]
            [InlineData((short)1, (short)-32768, -2147483647)]
            [InlineData((short)-2, (short)-1, -2)]
            [InlineData((short)-1, (short)-1, -1)]
            [InlineData((short)0, (short)0, 0)]
            [InlineData((short)1, (short)0, 1)]
            [InlineData((short)2, (short)0, 2)]
            [InlineData((short)-2, (short)32767, 2147483646)]
            [InlineData((short)-1, (short)32767, int.MaxValue)]
            public void SetsValueToCombinationOfValuesPassedIn(short lowerValue, short upperValue,
                int expectedValue)
            {
                var target = new PackedInt32(new[] { lowerValue, upperValue });

                Assert.Equal(expectedValue, target.Value);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenValueIsNull()
            {
                Assert.Throws<ArgumentNullException>(() => new PackedInt32((short[])null!));
            }

            [Theory]
            [InlineData(0)]
            [InlineData(1)]
            [InlineData(3)]
            [InlineData(4)]
            public void ThrowsArgumentOutOfRangeExceptionExceptionWhenValueHasIncorrectNumberOfElements(
                int numberOfElements)
            {
                short[] values = new short[numberOfElements];

                Assert.Throws<ArgumentException>(() => new PackedInt32(values));
            }
        }

        public class Constructor_ByteArray
        {
            [Theory]
            [InlineData((byte)0, (byte)0, (byte)0, (byte)128, int.MinValue)]
            [InlineData((byte)1, (byte)0, (byte)0, (byte)128, -2147483647)]
            [InlineData((byte)254, (byte)255, (byte)255, (byte)255, -2)]
            [InlineData((byte)255, (byte)255, (byte)255, (byte)255, -1)]
            [InlineData((byte)0, (byte)0, (byte)0, (byte)0, 0)]
            [InlineData((byte)1, (byte)0, (byte)0, (byte)0, 1)]
            [InlineData((byte)2, (byte)0, (byte)0, (byte)0, 2)]
            [InlineData((byte)254, (byte)255, (byte)255, (byte)127, 2147483646)]
            [InlineData((byte)255, (byte)255, (byte)255, (byte)127, int.MaxValue)]
            public void SetsValueToCombinationOfValuesPassedIn(byte value0, byte value1,
                byte value2, byte value3, int expectedValue)
            {
                var target = new PackedInt32(new[] { value0, value1, value2, value3 });

                Assert.Equal(expectedValue, target.Value);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenValueIsNull()
            {
                Assert.Throws<ArgumentNullException>(() => new PackedInt32((byte[])null!));
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
                byte[] values = new byte[numberOfElements];

                Assert.Throws<ArgumentException>(() => new PackedInt32(values));
            }
        }

        public class WithLowerInt16
        {
            [Theory]
            [InlineData(int.MinValue, short.MinValue, (int)-2147450880)]
            [InlineData(int.MinValue, (short)-32767, (int)-2147450879)]
            [InlineData(int.MinValue, (short)-1, (int)-2147418113)]
            [InlineData(int.MinValue, (short)0, (int)-2147483648)]
            [InlineData(int.MinValue, (short)1, (int)-2147483647)]
            [InlineData(int.MinValue, (short)32766, (int)-2147450882)]
            [InlineData(int.MinValue, short.MaxValue, (int)-2147450881)]
            [InlineData((int)-2147483647, short.MinValue, (int)-2147450880)]
            [InlineData((int)-2147483647, (short)-32767, (int)-2147450879)]
            [InlineData((int)-2147483647, (short)-1, (int)-2147418113)]
            [InlineData((int)-2147483647, (short)0, (int)-2147483648)]
            [InlineData((int)-2147483647, (short)1, (int)-2147483647)]
            [InlineData((int)-2147483647, (short)32766, (int)-2147450882)]
            [InlineData((int)-2147483647, short.MaxValue, (int)-2147450881)]
            [InlineData((int)-1, short.MinValue, (int)-32768)]
            [InlineData((int)-1, (short)-32767, (int)-32767)]
            [InlineData((int)-1, (short)-1, (int)-1)]
            [InlineData((int)-1, (short)0, (int)-65536)]
            [InlineData((int)-1, (short)1, (int)-65535)]
            [InlineData((int)-1, (short)32766, (int)-32770)]
            [InlineData((int)-1, short.MaxValue, (int)-32769)]
            [InlineData((int)0, short.MinValue, (int)32768)]
            [InlineData((int)0, (short)-32767, (int)32769)]
            [InlineData((int)0, (short)-1, (int)65535)]
            [InlineData((int)0, (short)0, (int)0)]
            [InlineData((int)0, (short)1, (int)1)]
            [InlineData((int)0, (short)32766, (int)32766)]
            [InlineData((int)0, short.MaxValue, (int)32767)]
            [InlineData((int)1, short.MinValue, (int)32768)]
            [InlineData((int)1, (short)-32767, (int)32769)]
            [InlineData((int)1, (short)-1, (int)65535)]
            [InlineData((int)1, (short)0, (int)0)]
            [InlineData((int)1, (short)1, (int)1)]
            [InlineData((int)1, (short)32766, (int)32766)]
            [InlineData((int)1, short.MaxValue, (int)32767)]
            [InlineData((int)2147483646, short.MinValue, (int)2147450880)]
            [InlineData((int)2147483646, (short)-32767, (int)2147450881)]
            [InlineData((int)2147483646, (short)-1, (int)2147483647)]
            [InlineData((int)2147483646, (short)0, (int)2147418112)]
            [InlineData((int)2147483646, (short)1, (int)2147418113)]
            [InlineData((int)2147483646, (short)32766, (int)2147450878)]
            [InlineData((int)2147483646, short.MaxValue, (int)2147450879)]
            [InlineData(int.MaxValue, short.MinValue, (int)2147450880)]
            [InlineData(int.MaxValue, (short)-32767, (int)2147450881)]
            [InlineData(int.MaxValue, (short)-1, (int)2147483647)]
            [InlineData(int.MaxValue, (short)0, (int)2147418112)]
            [InlineData(int.MaxValue, (short)1, (int)2147418113)]
            [InlineData(int.MaxValue, (short)32766, (int)2147450878)]
            [InlineData(int.MaxValue, short.MaxValue, (int)2147450879)]
            public void ReturnsPackedInt16WithCorrectValue(int originalValue, short newLowerInt16, int newValue)
            {
                var target = new PackedInt32(originalValue);

                var actual = target.WithLowerInt16(newLowerInt16);

                Assert.Equal(newValue, actual.Value);
            }
        }

        public class WithUpperInt16
        {
            [Theory]
            [InlineData(int.MinValue, short.MinValue, (int)-2147483648)]
            [InlineData(int.MinValue, (short)-32767, (int)-2147418112)]
            [InlineData(int.MinValue, (short)-1, (int)-65536)]
            [InlineData(int.MinValue, (short)0, (int)0)]
            [InlineData(int.MinValue, (short)1, (int)65536)]
            [InlineData(int.MinValue, (short)32766, (int)2147352576)]
            [InlineData(int.MinValue, short.MaxValue, (int)2147418112)]
            [InlineData((int)-2147483647, short.MinValue, (int)-2147483647)]
            [InlineData((int)-2147483647, (short)-32767, (int)-2147418111)]
            [InlineData((int)-2147483647, (short)-1, (int)-65535)]
            [InlineData((int)-2147483647, (short)0, (int)1)]
            [InlineData((int)-2147483647, (short)1, (int)65537)]
            [InlineData((int)-2147483647, (short)32766, (int)2147352577)]
            [InlineData((int)-2147483647, short.MaxValue, (int)2147418113)]
            [InlineData((int)-1, short.MinValue, (int)-2147418113)]
            [InlineData((int)-1, (short)-32767, (int)-2147352577)]
            [InlineData((int)-1, (short)-1, (int)-1)]
            [InlineData((int)-1, (short)0, (int)65535)]
            [InlineData((int)-1, (short)1, (int)131071)]
            [InlineData((int)-1, (short)32766, (int)2147418111)]
            [InlineData((int)-1, short.MaxValue, (int)2147483647)]
            [InlineData((int)0, short.MinValue, (int)-2147483648)]
            [InlineData((int)0, (short)-32767, (int)-2147418112)]
            [InlineData((int)0, (short)-1, (int)-65536)]
            [InlineData((int)0, (short)0, (int)0)]
            [InlineData((int)0, (short)1, (int)65536)]
            [InlineData((int)0, (short)32766, (int)2147352576)]
            [InlineData((int)0, short.MaxValue, (int)2147418112)]
            [InlineData((int)1, short.MinValue, (int)-2147483647)]
            [InlineData((int)1, (short)-32767, (int)-2147418111)]
            [InlineData((int)1, (short)-1, (int)-65535)]
            [InlineData((int)1, (short)0, (int)1)]
            [InlineData((int)1, (short)1, (int)65537)]
            [InlineData((int)1, (short)32766, (int)2147352577)]
            [InlineData((int)1, short.MaxValue, (int)2147418113)]
            [InlineData((int)2147483646, short.MinValue, (int)-2147418114)]
            [InlineData((int)2147483646, (short)-32767, (int)-2147352578)]
            [InlineData((int)2147483646, (short)-1, (int)-2)]
            [InlineData((int)2147483646, (short)0, (int)65534)]
            [InlineData((int)2147483646, (short)1, (int)131070)]
            [InlineData((int)2147483646, (short)32766, (int)2147418110)]
            [InlineData((int)2147483646, short.MaxValue, (int)2147483646)]
            [InlineData(int.MaxValue, short.MinValue, (int)-2147418113)]
            [InlineData(int.MaxValue, (short)-32767, (int)-2147352577)]
            [InlineData(int.MaxValue, (short)-1, (int)-1)]
            [InlineData(int.MaxValue, (short)0, (int)65535)]
            [InlineData(int.MaxValue, (short)1, (int)131071)]
            [InlineData(int.MaxValue, (short)32766, (int)2147418111)]
            [InlineData(int.MaxValue, short.MaxValue, (int)2147483647)]
            public void ReturnsPackedInt16WithCorrectValue(int originalValue, short newUpperInt16, int newValue)
            {
                var target = new PackedInt32(originalValue);

                var actual = target.WithUpperInt16(newUpperInt16);

                Assert.Equal(newValue, actual.Value);
            }
        }

        public class OperatorFromInt32
        {
            [Theory]
            [InlineData(int.MinValue)]
            [InlineData((int)-2147483647)]
            [InlineData((int)-1)]
            [InlineData((int)0)]
            [InlineData((int)1)]
            [InlineData((int)2147483646)]
            [InlineData(int.MaxValue)]
            public void CreatesPackedInt32WithSameValue(int value)
            {
                PackedInt32 target = value;

                Assert.Equal(value, target.Value);
            }
        }

        public class OperatorToInt32
        {
            [Theory]
            [InlineData(int.MinValue)]
            [InlineData((int)-2147483647)]
            [InlineData((int)-1)]
            [InlineData((int)0)]
            [InlineData((int)1)]
            [InlineData((int)2147483646)]
            [InlineData(int.MaxValue)]
            public void CreatesInt32WithSameValue(int value)
            {
                var packedInt32 = new PackedInt32(value);

                int actual = packedInt32;

                Assert.Equal(value, actual);
            }
        }

        public class FromInt32
        {
            [Theory]
            [InlineData(int.MinValue)]
            [InlineData((int)-2147483647)]
            [InlineData((int)-1)]
            [InlineData((int)0)]
            [InlineData((int)1)]
            [InlineData((int)2147483646)]
            [InlineData(int.MaxValue)]
            public void CreatesPackedInt32WithSameValue(int value)
            {
                var target = PackedInt32.FromInt32(value);

                Assert.Equal(value, target.Value);
            }
        }

        public class ToInt32
        {
            [Theory]
            [InlineData(int.MinValue)]
            [InlineData((int)-2147483647)]
            [InlineData((int)-1)]
            [InlineData((int)0)]
            [InlineData((int)1)]
            [InlineData((int)2147483646)]
            [InlineData(int.MaxValue)]
            public void CreatesInt32WithSameValue(int value)
            {
                var packedInt32 = new PackedInt32(value);

                int actual = packedInt32.ToInt32();

                Assert.Equal(value, actual);
            }
        }
    }
}
