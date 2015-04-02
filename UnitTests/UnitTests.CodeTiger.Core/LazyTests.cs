using System.Threading;
using CodeTiger;
using Xunit;

namespace UnitTests.CodeTiger
{
    public class LazyTests
    {
        public class Create1
        {
            [Fact]
            public void SetsIsValueCreatedToFalse()
            {
                var target = Lazy.Create<object>();

                Assert.False(target.IsValueCreated);
            }

            [Fact]
            public void ReturnsDefaultValueOfObject()
            {
                var target = Lazy.Create<object>();

                Assert.NotNull(target.Value);
                Assert.Equal(typeof(object), target.Value.GetType());
            }

            [Fact]
            public void ReturnsDefaultValueOfBoolean()
            {
                var target = Lazy.Create<bool>();

                Assert.Equal(new bool(), target.Value);
            }

            [Fact]
            public void ReturnsDefaultValueOfDecimal()
            {
                var target = Lazy.Create<decimal>();

                Assert.Equal(new decimal(), target.Value);
            }
        }

        public class Create1_Boolean
        {
            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void SetsIsValueCreatedToFalse(bool isThreadSafe)
            {
                var target = Lazy.Create<object>(isThreadSafe);

                Assert.False(target.IsValueCreated);
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void ReturnsDefaultValueOfObject(bool isThreadSafe)
            {
                var target = Lazy.Create<object>(isThreadSafe);

                Assert.NotNull(target.Value);
                Assert.Equal(typeof(object), target.Value.GetType());
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void ReturnsDefaultValueOfBoolean(bool isThreadSafe)
            {
                var target = Lazy.Create<bool>(isThreadSafe);

                Assert.Equal(new bool(), target.Value);
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void ReturnsDefaultValueOfDecimal(bool isThreadSafe)
            {
                var target = Lazy.Create<decimal>(isThreadSafe);

                Assert.Equal(new decimal(), target.Value);
            }
        }

        public class Create1_LazyThreadSafetyMode
        {
            [Theory]
            [InlineData(LazyThreadSafetyMode.None)]
            [InlineData(LazyThreadSafetyMode.PublicationOnly)]
            [InlineData(LazyThreadSafetyMode.ExecutionAndPublication)]
            public void SetsIsValueCreatedToFalse(LazyThreadSafetyMode mode)
            {
                var target = Lazy.Create<object>(mode);

                Assert.False(target.IsValueCreated);
            }

            [Theory]
            [InlineData(LazyThreadSafetyMode.None)]
            [InlineData(LazyThreadSafetyMode.PublicationOnly)]
            [InlineData(LazyThreadSafetyMode.ExecutionAndPublication)]
            public void CreatesTaskWhichReturnsDefaultValueOfObject(LazyThreadSafetyMode mode)
            {
                var target = Lazy.Create<object>(mode);

                Assert.NotNull(target.Value);
                Assert.Equal(typeof(object), target.Value.GetType());
            }

            [Theory]
            [InlineData(LazyThreadSafetyMode.None)]
            [InlineData(LazyThreadSafetyMode.PublicationOnly)]
            [InlineData(LazyThreadSafetyMode.ExecutionAndPublication)]
            public void CreatesTaskWhichReturnsDefaultValueOfBoolean(LazyThreadSafetyMode mode)
            {
                var target = Lazy.Create<bool>(mode);

                Assert.Equal(new bool(), target.Value);
            }

            [Theory]
            [InlineData(LazyThreadSafetyMode.None)]
            [InlineData(LazyThreadSafetyMode.PublicationOnly)]
            [InlineData(LazyThreadSafetyMode.ExecutionAndPublication)]
            public void CreatesTaskWhichReturnsDefaultValueOfDecimal(LazyThreadSafetyMode mode)
            {
                var target = Lazy.Create<decimal>(mode);

                Assert.Equal(new decimal(), target.Value);
            }
        }

        public class Create1_FuncOfTaskOfT1
        {
            [Fact]
            public void SetsIsValueCreatedToFalse()
            {
                object expected = new object();

                var target = Lazy.Create(() => expected);

                Assert.False(target.IsValueCreated);
            }

            [Fact]
            public void SetsValueToProvidedObject()
            {
                object expected = new object();

                var target = Lazy.Create(() => expected);

                Assert.Same(expected, target.Value);
            }
        }

        public class Create1_FuncOfTaskOfT1_Boolean
        {
            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void SetsIsValueCreatedToFalse(bool isThreadSafe)
            {
                object expected = new object();

                var target = Lazy.Create(() => expected, isThreadSafe);

                Assert.False(target.IsValueCreated);
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void SetsValueToProvidedTask(bool isThreadSafe)
            {
                object expected = new object();

                var target = Lazy.Create(() => expected, isThreadSafe);

                Assert.Same(expected, target.Value);
            }
        }

        public class Create1_FuncOfTaskOfT1_LazyThreadSafetyMode
        {
            [Theory]
            [InlineData(LazyThreadSafetyMode.None)]
            [InlineData(LazyThreadSafetyMode.PublicationOnly)]
            [InlineData(LazyThreadSafetyMode.ExecutionAndPublication)]
            public void SetsIsValueCreatedToFalse(LazyThreadSafetyMode mode)
            {
                object expected = new object();

                var target = Lazy.Create(() => expected, mode);

                Assert.False(target.IsValueCreated);
            }

            [Theory]
            [InlineData(LazyThreadSafetyMode.None)]
            [InlineData(LazyThreadSafetyMode.PublicationOnly)]
            [InlineData(LazyThreadSafetyMode.ExecutionAndPublication)]
            public void SetsValueToProvidedTask(LazyThreadSafetyMode mode)
            {
                object expected = new object();

                var target = Lazy.Create(() => expected, mode);

                Assert.Same(expected, target.Value);
            }
        }
    }
}