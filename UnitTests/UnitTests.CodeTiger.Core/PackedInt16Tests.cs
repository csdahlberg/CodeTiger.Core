using System;
using System.Collections.Generic;
using CodeTiger;
using Xunit;

namespace UnitTests.CodeTiger
{
    public static class PackedInt16Tests
    {
        public class LowerByte
        {
            [Theory]
            [MemberData(nameof(GetByteValuesForEdgeCases))]
            public void GetterReturnsLowerBytePassedInToConstructor(byte lowerValue, byte upperValue)
            {
                var target = new PackedInt16(lowerValue, upperValue);

                Assert.Equal(lowerValue, target.LowerByte);
            }

            public static IEnumerable<object[]> GetByteValuesForEdgeCases()
            {
                byte[] edgeCaseValues = new byte[] { byte.MinValue, 1, 254, byte.MaxValue };

                for (int i = 0; i < edgeCaseValues.Length; i++)
                {
                    for (int j = 0; j < edgeCaseValues.Length; j++)
                    {
                        yield return new object[] { edgeCaseValues[i], edgeCaseValues[j] };
                    }
                }
            }
        }

        public class UpperByte
        {
            [Theory]
            [MemberData(nameof(GetByteValuesForEdgeCases))]
            public void GetterReturnsUpperBytePassedInToConstructor(byte lowerValue, byte upperValue)
            {
                var target = new PackedInt16(lowerValue, upperValue);

                Assert.Equal(upperValue, target.UpperByte);
            }

            public static IEnumerable<object[]> GetByteValuesForEdgeCases()
            {
                byte[] edgeCaseValues = new byte[] { byte.MinValue, 1, 254, byte.MaxValue };

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
            public void GetterReturnsArrayWithValuesPassedInToConstructor(byte lowerValue, byte upperValue)
            {
                var target = new PackedInt16(lowerValue, upperValue);

                byte[] actual = target.GetByteValues();

                Assert.Equal(lowerValue, actual[0]);
                Assert.Equal(upperValue, actual[1]);
            }

            public static IEnumerable<object[]> GetByteValuesForEdgeCases()
            {
                byte[] edgeCaseValues = new byte[] { byte.MinValue, 1, 254, byte.MaxValue };

                for (int i = 0; i < edgeCaseValues.Length; i++)
                {
                    for (int j = 0; j < edgeCaseValues.Length; j++)
                    {
                        yield return new object[] { edgeCaseValues[i], edgeCaseValues[j] };
                    }
                }
            }
        }

        public class Constructor
        {
            [Fact]
            public void SetsValueToZero()
            {
                var target = new PackedInt16();

                Assert.Equal(0, target.Value);
            }
        }

        public class Constructor_Int16
        {
            [Theory]
            [InlineData(short.MinValue)]
            [InlineData(-32767)]
            [InlineData(-1)]
            [InlineData(0)]
            [InlineData(1)]
            [InlineData(32766)]
            [InlineData(short.MaxValue)]
            public void SetsValueToValuePassedIn(short initialValue)
            {
                var target = new PackedInt16(initialValue);

                Assert.Equal(initialValue, target.Value);
            }
        }

        public class Constructor_Byte_Byte
        {
            [Theory]
            [InlineData((byte)0, (byte)128, short.MinValue)]
            [InlineData((byte)1, (byte)128, (short)-32767)]
            [InlineData((byte)254, (byte)255, (short)-2)]
            [InlineData((byte)255, (byte)255, (short)-1)]
            [InlineData((byte)0, (byte)0, (short)0)]
            [InlineData((byte)1, (byte)0, (short)1)]
            [InlineData((byte)2, (byte)0, (short)2)]
            [InlineData((byte)254, (byte)127, (short)32766)]
            [InlineData((byte)255, (byte)127, short.MaxValue)]
            public void SetsValueToCombinationOfValuesPassedIn(byte lowerValue, byte upperValue, short expectedValue)
            {
                var target = new PackedInt16(lowerValue, upperValue);
                
                Assert.Equal(expectedValue, target.Value);
            }
        }

        public class Constructor_ByteArray
        {
            [Theory]
            [InlineData((byte)0, (byte)128, short.MinValue)]
            [InlineData((byte)1, (byte)128, (short)-32767)]
            [InlineData((byte)254, (byte)255, (short)-2)]
            [InlineData((byte)255, (byte)255, (short)-1)]
            [InlineData((byte)0, (byte)0, (short)0)]
            [InlineData((byte)1, (byte)0, (short)1)]
            [InlineData((byte)2, (byte)0, (short)2)]
            [InlineData((byte)254, (byte)127, (short)32766)]
            [InlineData((byte)255, (byte)127, short.MaxValue)]
            public void SetsValueToCombinationOfValuesPassedIn(byte lowerValue, byte upperValue, short expectedValue)
            {
                var target = new PackedInt16(new[] { lowerValue, upperValue });

                Assert.Equal(expectedValue, target.Value);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenValueIsNull()
            {
                Assert.Throws<ArgumentNullException>(() => new PackedInt16(null));
            }

            [Theory]
            [InlineData(0)]
            [InlineData(1)]
            [InlineData(3)]
            [InlineData(4)]
            public void ThrowsArgumentOutOfRangeExceptionExceptionWhenValueHasIncorrectNumberOfElements(
                int numberOfElements)
            {
                byte[] values = new byte[numberOfElements];

                Assert.Throws<ArgumentException>(() => new PackedInt16(values));
            }
        }

        public class WithLowerByte
        {
            [Theory]
            [InlineData(short.MinValue, byte.MinValue, short.MinValue)]
            [InlineData(short.MinValue, (byte)1, (short)-32767)]
            [InlineData(short.MinValue, (byte)254, (short)-32514)]
            [InlineData(short.MinValue, byte.MaxValue, (short)-32513)]
            [InlineData((short)-32767, byte.MinValue, short.MinValue)]
            [InlineData((short)-32767, (byte)1, (short)-32767)]
            [InlineData((short)-32767, (byte)254, (short)-32514)]
            [InlineData((short)-32767, byte.MaxValue, (short)-32513)]
            [InlineData((short)-1, byte.MinValue, (short)-256)]
            [InlineData((short)-1, (byte)1, (short)-255)]
            [InlineData((short)-1, (byte)254, (short)-2)]
            [InlineData((short)-1, byte.MaxValue, (short)-1)]
            [InlineData((short)0, byte.MinValue, (short)0)]
            [InlineData((short)0, (byte)1, (short)1)]
            [InlineData((short)0, (byte)254, (short)254)]
            [InlineData((short)0, byte.MaxValue, (short)255)]
            [InlineData((short)1, byte.MinValue, (short)0)]
            [InlineData((short)1, (byte)1, (short)1)]
            [InlineData((short)1, (byte)254, (short)254)]
            [InlineData((short)1, byte.MaxValue, (short)255)]
            [InlineData((short)32766, byte.MinValue, (short)32512)]
            [InlineData((short)32766, (byte)1, (short)32513)]
            [InlineData((short)32766, (byte)254, (short)32766)]
            [InlineData((short)32766, byte.MaxValue, (short)32767)]
            [InlineData(short.MaxValue, byte.MinValue, (short)32512)]
            [InlineData(short.MaxValue, (byte)1, (short)32513)]
            [InlineData(short.MaxValue, (byte)254, (short)32766)]
            [InlineData(short.MaxValue, byte.MaxValue, short.MaxValue)]
            public void ReturnsPackedInt16WithCorrectValue(short originalValue, byte newLowerByte, short newValue)
            {
                var target = new PackedInt16(originalValue);

                var actual = target.WithLowerByte(newLowerByte);

                Assert.Equal(newValue, actual.Value);
            }
        }

        public class WithUpperByte
        {
            [Theory]
            [InlineData(short.MinValue, byte.MinValue, 0)]
            [InlineData(short.MinValue, (byte)1, (short)256)]
            [InlineData(short.MinValue, (byte)254, (short)-512)]
            [InlineData(short.MinValue, byte.MaxValue, (short)-256)]
            [InlineData((short)-32767, byte.MinValue, (short)1)]
            [InlineData((short)-32767, (byte)1, (short)257)]
            [InlineData((short)-32767, (byte)254, (short)-511)]
            [InlineData((short)-32767, byte.MaxValue, (short)-255)]
            [InlineData((short)-1, byte.MinValue, (short)255)]
            [InlineData((short)-1, (byte)1, (short)511)]
            [InlineData((short)-1, (byte)254, (short)-257)]
            [InlineData((short)-1, byte.MaxValue, (short)-1)]
            [InlineData((short)0, byte.MinValue, (short)0)]
            [InlineData((short)0, (byte)1, (short)256)]
            [InlineData((short)0, (byte)254, (short)-512)]
            [InlineData((short)0, byte.MaxValue, (short)-256)]
            [InlineData((short)1, byte.MinValue, (short)1)]
            [InlineData((short)1, (byte)1, (short)257)]
            [InlineData((short)1, (byte)254, (short)-511)]
            [InlineData((short)1, byte.MaxValue, (short)-255)]
            [InlineData((short)32766, byte.MinValue, (short)254)]
            [InlineData((short)32766, (byte)1, (short)510)]
            [InlineData((short)32766, (byte)254, (short)-258)]
            [InlineData((short)32766, byte.MaxValue, (short)-2)]
            [InlineData(short.MaxValue, byte.MinValue, (short)255)]
            [InlineData(short.MaxValue, (byte)1, (short)511)]
            [InlineData(short.MaxValue, (byte)254, (short)-257)]
            [InlineData(short.MaxValue, byte.MaxValue, (short)-1)]
            public void ReturnsPackedInt16WithCorrectValue(short originalValue, byte newUpperByte, short newValue)
            {
                var target = new PackedInt16(originalValue);

                var actual = target.WithUpperByte(newUpperByte);

                Assert.Equal(newValue, actual.Value);
            }
        }

        public class OperatorFromInt16
        {
            [Theory]
            [InlineData(short.MinValue)]
            [InlineData((short)-32767)]
            [InlineData((short)-1)]
            [InlineData((short)0)]
            [InlineData((short)1)]
            [InlineData((short)32766)]
            [InlineData(short.MaxValue)]
            public void CreatesPackedInt16WithSameValue(short value)
            {
                PackedInt16 target = value;

                Assert.Equal(value, target.Value);
            }
        }

        public class OperatorToInt16
        {
            [Theory]
            [InlineData(short.MinValue)]
            [InlineData((short)-32767)]
            [InlineData((short)-1)]
            [InlineData((short)0)]
            [InlineData((short)1)]
            [InlineData((short)32766)]
            [InlineData(short.MaxValue)]
            public void CreatesInt16WithSameValue(short value)
            {
                var packedInt16 = new PackedInt16(value);

                short actual = packedInt16;

                Assert.Equal(value, actual);
            }
        }

        public class FromInt16
        {
            [Theory]
            [InlineData(short.MinValue)]
            [InlineData((short)-32767)]
            [InlineData((short)-1)]
            [InlineData((short)0)]
            [InlineData((short)1)]
            [InlineData((short)32766)]
            [InlineData(short.MaxValue)]
            public void CreatesPackedInt16WithSameValue(short value)
            {
                var target = PackedInt16.FromInt16(value);

                Assert.Equal(value, target.Value);
            }
        }

        public class ToInt16
        {
            [Theory]
            [InlineData(short.MinValue)]
            [InlineData((short)-32767)]
            [InlineData((short)-1)]
            [InlineData((short)0)]
            [InlineData((short)1)]
            [InlineData((short)32766)]
            [InlineData(short.MaxValue)]
            public void CreatesInt16WithSameValue(short value)
            {
                var packedInt16 = new PackedInt16(value);

                short actual = packedInt16.ToInt16();

                Assert.Equal(value, actual);
            }
        }
    }
}
