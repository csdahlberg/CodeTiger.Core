using System.Threading;
using System.Threading.Tasks;
using CodeTiger;
using Xunit;

namespace UnitTests.CodeTiger
{
    /// <summary>
    /// Contains unit tests for the <see cref="AsyncLazy"/> class.
    /// </summary>
    public static class AsyncLazyTests
    {
        public class Create1
        {
            [Fact]
            public void SetsIsValueCreatedToFalse()
            {
                var target = AsyncLazy.Create<object>();

                Assert.False(target.IsValueCreated);
            }

            [Fact]
            public void CreatesTaskWhichIsCompleted()
            {
                var target = AsyncLazy.Create<object>();

                Assert.True(target.Value.IsCompleted);
            }

            [Fact]
            public void CreatesTaskWhichReturnsDefaultValueOfObject()
            {
                var target = AsyncLazy.Create<object>();

                Assert.NotNull(target.Value.GetAwaiter().GetResult());
                Assert.Equal(typeof(object), target.Value.GetAwaiter().GetResult().GetType());
            }

            [Fact]
            public void CreatesTaskWhichReturnsDefaultValueOfBoolean()
            {
                var target = AsyncLazy.Create<bool>();

                Assert.Equal(default(bool), target.Value.GetAwaiter().GetResult());
            }

            [Fact]
            public void CreatesTaskWhichReturnsDefaultValueOfDecimal()
            {
                var target = AsyncLazy.Create<decimal>();

                Assert.Equal(default(decimal), target.Value.GetAwaiter().GetResult());
            }
        }

        public class Create1_Boolean
        {
            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void SetsIsValueCreatedToFalse(bool isThreadSafe)
            {
                var target = AsyncLazy.Create<object>(isThreadSafe);

                Assert.False(target.IsValueCreated);
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void CreatesTaskWhichIsCompleted(bool isThreadSafe)
            {
                var target = AsyncLazy.Create<object>(isThreadSafe);

                Assert.True(target.Value.IsCompleted);
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void CreatesTaskWhichReturnsDefaultValueOfObject(bool isThreadSafe)
            {
                var target = AsyncLazy.Create<object>(isThreadSafe);

                Assert.NotNull(target.Value.GetAwaiter().GetResult());
                Assert.Equal(typeof(object), target.Value.GetAwaiter().GetResult().GetType());
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void CreatesTaskWhichReturnsDefaultValueOfBoolean(bool isThreadSafe)
            {
                var target = AsyncLazy.Create<bool>(isThreadSafe);

                Assert.Equal(new bool(), target.Value.Result);
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void CreatesTaskWhichReturnsDefaultValueOfDecimal(bool isThreadSafe)
            {
                var target = AsyncLazy.Create<decimal>(isThreadSafe);

                Assert.Equal(new decimal(), target.Value.Result);
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
                var target = AsyncLazy.Create<object>(mode);

                Assert.False(target.IsValueCreated);
            }

            [Theory]
            [InlineData(LazyThreadSafetyMode.None)]
            [InlineData(LazyThreadSafetyMode.PublicationOnly)]
            [InlineData(LazyThreadSafetyMode.ExecutionAndPublication)]
            public void CreatesTaskWhichIsCompleted(LazyThreadSafetyMode mode)
            {
                var target = AsyncLazy.Create<object>(mode);

                Assert.True(target.Value.IsCompleted);
            }

            [Theory]
            [InlineData(LazyThreadSafetyMode.None)]
            [InlineData(LazyThreadSafetyMode.PublicationOnly)]
            [InlineData(LazyThreadSafetyMode.ExecutionAndPublication)]
            public void CreatesTaskWhichReturnsDefaultValueOfObject(LazyThreadSafetyMode mode)
            {
                var target = AsyncLazy.Create<object>(mode);

                Assert.NotNull(target.Value.GetAwaiter().GetResult());
                Assert.Equal(typeof(object), target.Value.GetAwaiter().GetResult().GetType());
            }

            [Theory]
            [InlineData(LazyThreadSafetyMode.None)]
            [InlineData(LazyThreadSafetyMode.PublicationOnly)]
            [InlineData(LazyThreadSafetyMode.ExecutionAndPublication)]
            public void CreatesTaskWhichReturnsDefaultValueOfBoolean(LazyThreadSafetyMode mode)
            {
                var target = AsyncLazy.Create<bool>(mode);

                Assert.Equal(new bool(), target.Value.GetAwaiter().GetResult());
            }

            [Theory]
            [InlineData(LazyThreadSafetyMode.None)]
            [InlineData(LazyThreadSafetyMode.PublicationOnly)]
            [InlineData(LazyThreadSafetyMode.ExecutionAndPublication)]
            public void CreatesTaskWhichReturnsDefaultValueOfDecimal(LazyThreadSafetyMode mode)
            {
                var target = AsyncLazy.Create<decimal>(mode);

                Assert.Equal(new decimal(), target.Value.GetAwaiter().GetResult());
            }
        }

        public class Create1_FuncOfTaskOfT1
        {
            [Fact]
            public void SetsIsValueCreatedToFalse()
            {
                var task = Task.FromResult(new object());

                var target = AsyncLazy.Create(() => task);

                Assert.False(target.IsValueCreated);
            }

            [Fact]
            public void SetsValueToProvidedTask()
            {
                var task = Task.FromResult(new object());

                var target = AsyncLazy.Create(() => task);

                Assert.Same(task, target.Value);
            }
        }

        public class Create1_FuncOfTaskOfT1_Boolean
        {
            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void SetsIsValueCreatedToFalse(bool isThreadSafe)
            {
                var task = Task.FromResult(new object());

                var target = AsyncLazy.Create(() => task, isThreadSafe);

                Assert.False(target.IsValueCreated);
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void SetsValueToProvidedTask(bool isThreadSafe)
            {
                var task = Task.FromResult(new object());

                var target = AsyncLazy.Create(() => task, isThreadSafe);

                Assert.Same(task, target.Value);
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
                var task = Task.FromResult(new object());

                var target = AsyncLazy.Create(() => task, mode);

                Assert.False(target.IsValueCreated);
            }

            [Theory]
            [InlineData(LazyThreadSafetyMode.None)]
            [InlineData(LazyThreadSafetyMode.PublicationOnly)]
            [InlineData(LazyThreadSafetyMode.ExecutionAndPublication)]
            public void SetsValueToProvidedTask(LazyThreadSafetyMode mode)
            {
                var task = Task.FromResult(new object());

                var target = AsyncLazy.Create(() => task, mode);

                Assert.Same(task, target.Value);
            }
        }
    }
}