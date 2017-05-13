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
        public class ArgumentIsNotNull1_String_T1
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenObjectArgumentIsNull()
            {
                Assert.Throws<ArgumentNullException>("DummyArgumentName",
                    () => Guard.ArgumentIsNotNull<object>("DummyArgumentName", (object)null));
            }

            [Fact]
            public void DoesNotThrowExceptionWhenObjectArgumentIsNotNull()
            {
                Guard.ArgumentIsNotNull<object>("DummyArgumentName", new object());
            }
        }

        public class ArgumentIsNotNullOrEmpty
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenArgumentIsNull()
            {
                Assert.Throws<ArgumentNullException>("DummyArgumentName",
                    () => Guard.ArgumentIsNotNullOrEmpty("DummyArgumentName", null));
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
                Guard.ArgumentIsNotNullOrEmpty("DummyArgumentName", argumentValue);
            }
        }

        public class ArgumentIsNotNullOrWhiteSpace
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenArgumentIsNull()
            {
                Assert.Throws<ArgumentNullException>("DummyArgumentName",
                    () => Guard.ArgumentIsNotNullOrWhiteSpace("DummyArgumentName", null));
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
                Guard.ArgumentIsNotNullOrWhiteSpace("DummyArgumentName", argumentValue);
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
                var actual = Assert.Throws<ObjectDisposedException>(
                    () => Guard.ObjectHasNotBeenDisposed<DisposableClass>(null, true));
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
                    () => Guard.ObjectHasNotBeenDisposed<IDisposable>(null, true));
                Assert.Equal(typeof(IDisposable).FullName, actual.ObjectName);
            }

            [Fact]
            public void DoesNotThrowExceptionForDisposableClassWhenHasObjectBeenDisposedIsFalse()
            {
                Guard.ObjectHasNotBeenDisposed(new DisposableClass(), false);
            }

            [Fact]
            public void DoesNotThrowExceptionForDisposableStructWhenHasObjectBeenDisposedIsFalse()
            {
                Guard.ObjectHasNotBeenDisposed(new DisposableStruct(), false);
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