﻿using System;
using CodeTiger;
using Xunit;

namespace UnitTests.CodeTiger
{
    /// <summary>
    /// Contains unit tests for the <see cref="Guard"/> class.
    /// </summary>
    public partial class GuardTests
    {
        public class ArgumentIsNotNull_String_T1
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenObjectArgumentIsNull()
            {
                object argumentValue = null!;

                Assert.Throws<ArgumentNullException>("DummyArgumentName",
                    () => Guard.ArgumentIsNotNull("DummyArgumentName", argumentValue));
            }

            [Fact]
            public void DoesNotThrowExceptionWhenObjectArgumentIsNotNull()
            {
                object argumentValue = new object();

                object actual = Guard.ArgumentIsNotNull("DummyArgumentName", argumentValue);

                Assert.Same(argumentValue, actual);
            }
        }

        public class ArgumentIsNotNullOrEmpty_String_String
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenArgumentIsNull()
            {
                Assert.Throws<ArgumentNullException>("DummyArgumentName",
                    () => Guard.ArgumentIsNotNullOrEmpty("DummyArgumentName", null!));
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenArgumentIsEmptyString()
            {
                Assert.Throws<ArgumentException>("DummyArgumentName",
                    () => Guard.ArgumentIsNotNullOrEmpty("DummyArgumentName", ""));
            }

            [Theory]
            [InlineData(" ")]
            [InlineData("\t")]
            [InlineData("x")]
            [InlineData(" x")]
            [InlineData("x ")]
            [InlineData(" x ")]
            [InlineData("Testing")]
            public void DoesNotThrowExceptionWhenArgumentIsNotNullOrEmpty(string argumentValue)
            {
                string actual = Guard.ArgumentIsNotNullOrEmpty("DummyArgumentName", argumentValue);

                Assert.Equal(argumentValue, actual);
            }
        }

        public class ArgumentIsNotNullOrWhiteSpace_String_String
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenArgumentIsNull()
            {
                Assert.Throws<ArgumentNullException>("DummyArgumentName",
                    () => Guard.ArgumentIsNotNullOrWhiteSpace("DummyArgumentName", null!));
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenArgumentIsEmptyString()
            {
                Assert.Throws<ArgumentException>("DummyArgumentName",
                    () => Guard.ArgumentIsNotNullOrWhiteSpace("DummyArgumentName", ""));
            }

            [Theory]
            [InlineData(" ")]
            [InlineData("\t")]
            public void ThrowsArgumentExceptionWhenArgumentIsWhiteSpace(string argumentValue)
            {
                Assert.Throws<ArgumentException>("DummyArgumentName",
                    () => Guard.ArgumentIsNotNullOrWhiteSpace("DummyArgumentName", argumentValue));
            }

            [Theory]
            [InlineData("x")]
            [InlineData(" x")]
            [InlineData("x ")]
            [InlineData(" x ")]
            [InlineData("Testing")]
            public void DoesNotThrowExceptionWhenArgumentIsNotNullOrWhiteSpace(string argumentValue)
            {
                string actual = Guard.ArgumentIsNotNullOrWhiteSpace("DummyArgumentName", argumentValue);

                Assert.Equal(argumentValue, actual);
            }
        }

        public class ArgumentIsNull_String_T1
        {
            [Fact]
            public void ThrowsArgumentExceptionWhenObjectArgumentIsNotNull()
            {
                object? argumentValue = new object();

                Assert.Throws<ArgumentException>("DummyArgumentName",
                    () => Guard.ArgumentIsNull("DummyArgumentName", argumentValue));
            }

            [Fact]
            public void DoesNotThrowExceptionWhenObjectArgumentIsNull()
            {
                object? argumentValue = null;

                Guard.ArgumentIsNull("DummyArgumentName", argumentValue);
            }
        }

        public class ArgumentIsNullOrEmpty_String_String
        {
            [Theory]
            [InlineData(" ")]
            [InlineData("\t")]
            [InlineData("x")]
            [InlineData(" x")]
            [InlineData("x ")]
            [InlineData(" x ")]
            [InlineData("Testing")]
            public void ThrowsArgumentExceptionWhenArgumentIsNotNullOrEmpty(string argumentValue)
            {
                Assert.Throws<ArgumentException>("DummyArgumentName",
                    () => Guard.ArgumentIsNullOrEmpty("DummyArgumentName", argumentValue));
            }

            [Fact]
            public void DoesNotThrowExceptionWhenArgumentIsEmptyString()
            {
                Guard.ArgumentIsNullOrEmpty("DummyArgumentName", "");
            }

            [Fact]
            public void DoesNotThrowExceptionWhenArgumentIsNull()
            {
                Guard.ArgumentIsNullOrEmpty("DummyArgumentName", null);
            }
        }

        public class ArgumentIsNullOrWhiteSpace_String_String
        {
            [Theory]
            [InlineData("x")]
            [InlineData(" x")]
            [InlineData("x ")]
            [InlineData(" x ")]
            [InlineData("Testing")]
            public void ThrowsArgumentExceptionWhenArgumentIsNotNullOrWhiteSpace(string argumentValue)
            {
                Assert.Throws<ArgumentException>("DummyArgumentName",
                    () => Guard.ArgumentIsNullOrWhiteSpace("DummyArgumentName", argumentValue));
            }

            [Fact]
            public void DoesNotThrowExceptionWhenArgumentIsNull()
            {
                string? actual = Guard.ArgumentIsNullOrWhiteSpace("DummyArgumentName", null);

                Assert.Null(actual);
            }

            [Fact]
            public void DoesNotThrowExceptionWhenArgumentIsEmptyString()
            {
                string? actual = Guard.ArgumentIsNullOrWhiteSpace("DummyArgumentName", "");

                Assert.Equal("", actual);
            }

            [Theory]
            [InlineData(" ")]
            [InlineData("\t")]
            public void DoesNotThrowExceptionWhenArgumentIsWhiteSpace(string argumentValue)
            {
                string? actual = Guard.ArgumentIsNullOrWhiteSpace("DummyArgumentName", argumentValue);

                Assert.Equal(argumentValue, actual);
            }
        }

        public class ArgumentIsValid_String_Boolean
        {
            [Fact]
            public void ThrowsArgumentExceptionWhenConditionIsFalse()
            {
                Assert.Throws<ArgumentException>("DummyArgumentName",
                    () => Guard.ArgumentIsValid("DummyArgumentName", false));
            }

            [Fact]
            public void DoesNotThrowExceptionWhenConditionIsTrue()
            {
                Guard.ArgumentIsValid("DummyArgumentName", true);
            }
        }

        public class ObjectHasNotBeenDisposed1_T1_Boolean
        {
            [Fact]
            public void ThrowsCorrectObjectDisposedExceptionForDisposableClassWhenHasObjectBeenDisposedIsTrue()
            {
                var actual = Assert.Throws<ObjectDisposedException>(
                    () => Guard.ObjectHasNotBeenDisposed(new DisposableClass(), true));
                Assert.Equal(typeof(DisposableClass).FullName, actual.ObjectName);
            }

            [Fact]
            public void ThrowsCorrectObjectDisposedExceptionForNullDisposableClassWhenHasObjectBeenDisposedIsTrue()
            {
                DisposableClass objectValue = null!;

                var actual = Assert.Throws<ObjectDisposedException>(
                    () => Guard.ObjectHasNotBeenDisposed(objectValue, true));
                Assert.Equal(typeof(DisposableClass).FullName, actual.ObjectName);
            }

            [Fact]
            public void ThrowsCorrectObjectDisposedExceptionForDisposableStructWhenHasObjectBeenDisposedIsTrue()
            {
                var actual = Assert.Throws<ObjectDisposedException>(
                    () => Guard.ObjectHasNotBeenDisposed(new DisposableStruct(), true));
                Assert.Equal(typeof(DisposableStruct).FullName, actual.ObjectName);
            }

            [Fact]
            public void ThrowsCorrectObjectDisposedExceptionForIDisposableWhenHasObjectBeenDisposedIsTrue()
            {
                var actual = Assert.Throws<ObjectDisposedException>(
                    () => Guard.ObjectHasNotBeenDisposed<IDisposable>(new DisposableClass(), true));
                Assert.Equal(typeof(DisposableClass).FullName, actual.ObjectName);
            }

            [Fact]
            public void ThrowsCorrectObjectDisposedExceptionForNullIDisposableWhenHasObjectBeenDisposedIsTrue()
            {
                var actual = Assert.Throws<ObjectDisposedException>(
                    () => Guard.ObjectHasNotBeenDisposed<IDisposable>(null!, true));
                Assert.Equal(typeof(IDisposable).FullName, actual.ObjectName);
            }

            [Fact]
            public void DoesNotThrowExceptionForDisposableClassWhenHasObjectBeenDisposedIsFalse()
            {
                using (var argumentValue = new DisposableClass())
                {
                    var actual = Guard.ObjectHasNotBeenDisposed(argumentValue, false);

                    Assert.Same(argumentValue, actual);
                }
            }

            [Fact]
            public void DoesNotThrowExceptionForDisposableStructWhenHasObjectBeenDisposedIsFalse()
            {
                using (var argumentValue = new DisposableStruct())
                {
                    var actual = Guard.ObjectHasNotBeenDisposed(argumentValue, false);

                    Assert.Equal(argumentValue, actual);
                }
            }

            private class DisposableClass : IDisposable
            {
                public void Dispose()
                {
                }
            }

            private struct DisposableStruct : IDisposable
            {
                public void Dispose()
                {
                }
            }
        }

        public class OperationIsValid_Boolean
        {
            [Fact]
            public void ThrowsInvalidOperationExceptionWhenConditionIsFalse()
            {
                Assert.Throws<InvalidOperationException>(() => Guard.OperationIsValid(false));
            }

            [Fact]
            public void DoesNotThrowExceptionWhenConditionIsTrue()
            {
                Guard.OperationIsValid(true);
            }
        }
    }
}
