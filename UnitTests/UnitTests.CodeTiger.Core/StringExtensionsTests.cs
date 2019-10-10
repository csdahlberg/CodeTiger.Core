using System;
using CodeTiger;
using Xunit;

namespace UnitTests.CodeTiger
{
    public static class StringExtensionsTests
    {
        public class SplitAt_Int32
        {
            [Fact]
            public void ThrowsArgumentNullExceptionForNullString()
            {
                Assert.Throws<ArgumentNullException>(() => ((string)null).SplitAt(0));
            }

            [Theory]
            [InlineData("", int.MinValue)]
            [InlineData("", -1)]
            [InlineData("", 1)]
            [InlineData("", int.MaxValue)]
            [InlineData("x", int.MinValue)]
            [InlineData("x", -1)]
            [InlineData("x", 2)]
            [InlineData("x", int.MaxValue)]
            [InlineData("xy", int.MinValue)]
            [InlineData("xy", -1)]
            [InlineData("xy", 3)]
            [InlineData("xy", int.MaxValue)]
            public void ThrowsArgumentOutOfRangeExceptionForInvalidOffsets(string original, int offset)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => original.SplitAt(offset));
            }

            [Theory]
            [InlineData("", 0, "", "")]
            [InlineData("x", 0, "", "x")]
            [InlineData("x", 1, "x", "")]
            [InlineData("xy", 0, "", "xy")]
            [InlineData("xy", 1, "x", "y")]
            [InlineData("xy", 2, "xy", "")]
            public void ReturnsCorrectStringsForValidOffsets(string original, int offset, string expected1,
                string expected2)
            {
                string[] actual = original.SplitAt(offset);

                Assert.NotNull(actual);
                Assert.Equal(2, actual.Length);
                Assert.Equal(expected1, actual[0]);
                Assert.Equal(expected2, actual[1]);
            }
        }

        public class Contains_String_StringComparison
        {
            [Fact]
            public void ThrowsArgumentNullExceptionForNullSourceString()
            {
                Assert.Throws<ArgumentNullException>(
                    () => ((string)null).Contains("foo", StringComparison.Ordinal));
            }

            [Fact]
            public void ThrowsArgumentNullExceptionForNullValueString()
            {
                Assert.Throws<ArgumentNullException>(() => "".Contains(null, StringComparison.Ordinal));
            }

            [Theory]
            [InlineData("", "")]
            [InlineData("Test", "")]
            public void ReturnsTrueWhenValueIsEmptyString(string source, string value)
            {
                Assert.True(source.Contains(value, StringComparison.Ordinal));
                Assert.True(source.Contains(value, StringComparison.OrdinalIgnoreCase));
            }

            [Theory]
            [InlineData("Test", "Test")]
            [InlineData("This is a test", "This is ")]
            [InlineData("This is a test", " is a")]
            [InlineData("This is a test", "a test")]
            public void ReturnsTrueWhenSourceContainsValueWithSameCasing(string source, string value)
            {
                Assert.True(source.Contains(value, StringComparison.Ordinal));
                Assert.True(source.Contains(value, StringComparison.OrdinalIgnoreCase));
            }

            [Theory]
            [InlineData("Test", "test")]
            [InlineData("test", "Test")]
            [InlineData("This is a test", "this is ")]
            [InlineData("This is a test", " IS a")]
            [InlineData("This is a test", "a TEST")]
            public void ReturnsTrueWhenSourceContainsValueWithDifferentCasing(string source, string value)
            {
                Assert.True(source.Contains(value, StringComparison.OrdinalIgnoreCase));
            }

            [Theory]
            [InlineData("", "a test")]
            [InlineData("Test", "a test")]
            public void ReturnsFalseWhenSourceDoesNotContainValue(string source, string value)
            {
                Assert.False(source.Contains(value, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}
