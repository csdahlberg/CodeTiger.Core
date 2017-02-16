using System;
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

        public class ArgumentIsValid_String_Boolean
        {
            [Fact]
            public void ThrowsArgumentOutOfRangeExceptionWhenConditionIsFalse()
            {
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => Guard.ArgumentIsValid("DummyArgumentName", false));
            }

            [Fact]
            public void DoesNotThrowExceptionWhenConditionIsTrue()
            {
                Guard.ArgumentIsValid("DummyArgumentName", true);
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