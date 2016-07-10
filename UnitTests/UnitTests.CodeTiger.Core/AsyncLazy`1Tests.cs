using System.Threading;
using System.Threading.Tasks;
using CodeTiger;
using Xunit;

namespace UnitTests.CodeTiger
{
    /// <summary>
    /// Contains unit tests for the <see cref="AsyncLazy{T}"/> class.
    /// </summary>
    public class AsyncLazy1Tests
    {
        public class Constructor
        {
            [Fact]
            public void SetsIsValueCreatedToFalse()
            {
                var target = new AsyncLazy<object>();
                
                Assert.False(target.IsValueCreated);
            }

            [Fact]
            public void CreatesTaskWhichIsCompleted()
            {
                var target = new AsyncLazy<object>();

                Assert.True(target.Value.IsCompleted);
            }

            [Fact]
            public void CreatesTaskWhichReturnsDefaultValueOfObject()
            {
                var target = new AsyncLazy<object>();

                var actual = target.Value.ConfigureAwait(false).GetAwaiter().GetResult();

                Assert.NotNull(actual);
                Assert.Equal(typeof(object), actual.GetType());
            }

            [Fact]
            public void CreatesTaskWhichReturnsDefaultValueOfBoolean()
            {
                var target = new AsyncLazy<bool>();

                Assert.Equal(new bool(), target.Value.Result);
            }

            [Fact]
            public void CreatesTaskWhichReturnsDefaultValueOfDecimal()
            {
                var target = new AsyncLazy<decimal>();

                Assert.Equal(new decimal(), target.Value.Result);
            }
        }

        public class Constructor_Boolean
        {
            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void SetsIsValueCreatedToFalse(bool isThreadSafe)
            {
                var target = new AsyncLazy<object>(isThreadSafe);

                Assert.False(target.IsValueCreated);
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void CreatesTaskWhichIsCompleted(bool isThreadSafe)
            {
                var target = new AsyncLazy<object>(isThreadSafe);

                Assert.True(target.Value.IsCompleted);
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void CreatesTaskWhichReturnsDefaultValueOfObject(bool isThreadSafe)
            {
                var target = new AsyncLazy<object>(isThreadSafe);

                Assert.NotNull(target.Value.GetAwaiter().GetResult());
                Assert.Equal(typeof(object), target.Value.GetAwaiter().GetResult().GetType());
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void CreatesTaskWhichReturnsDefaultValueOfBoolean(bool isThreadSafe)
            {
                var target = new AsyncLazy<bool>(isThreadSafe);

                Assert.Equal(new bool(), target.Value.GetAwaiter().GetResult());
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void CreatesTaskWhichReturnsDefaultValueOfDecimal(bool isThreadSafe)
            {
                var target = new AsyncLazy<decimal>(isThreadSafe);

                Assert.Equal(new decimal(), target.Value.GetAwaiter().GetResult());
            }
        }

        public class Constructor_LazyThreadSafetyMode
        {
            [Theory]
            [InlineData(LazyThreadSafetyMode.None)]
            [InlineData(LazyThreadSafetyMode.PublicationOnly)]
            [InlineData(LazyThreadSafetyMode.ExecutionAndPublication)]
            public void SetsIsValueCreatedToFalse(LazyThreadSafetyMode mode)
            {
                var target = new AsyncLazy<object>(mode);

                Assert.False(target.IsValueCreated);
            }

            [Theory]
            [InlineData(LazyThreadSafetyMode.None)]
            [InlineData(LazyThreadSafetyMode.PublicationOnly)]
            [InlineData(LazyThreadSafetyMode.ExecutionAndPublication)]
            public void CreatesTaskWhichIsCompleted(LazyThreadSafetyMode mode)
            {
                var target = new AsyncLazy<object>(mode);

                Assert.True(target.Value.IsCompleted);
            }

            [Theory]
            [InlineData(LazyThreadSafetyMode.None)]
            [InlineData(LazyThreadSafetyMode.PublicationOnly)]
            [InlineData(LazyThreadSafetyMode.ExecutionAndPublication)]
            public void CreatesTaskWhichReturnsDefaultValueOfObject(LazyThreadSafetyMode mode)
            {
                var target = new AsyncLazy<object>(mode);

                Assert.NotNull(target.Value.GetAwaiter().GetResult());
                Assert.Equal(typeof(object), target.Value.GetAwaiter().GetResult().GetType());
            }

            [Theory]
            [InlineData(LazyThreadSafetyMode.None)]
            [InlineData(LazyThreadSafetyMode.PublicationOnly)]
            [InlineData(LazyThreadSafetyMode.ExecutionAndPublication)]
            public void CreatesTaskWhichReturnsDefaultValueOfBoolean(LazyThreadSafetyMode mode)
            {
                var target = new AsyncLazy<bool>(mode);

                Assert.Equal(new bool(), target.Value.GetAwaiter().GetResult());
            }

            [Theory]
            [InlineData(LazyThreadSafetyMode.None)]
            [InlineData(LazyThreadSafetyMode.PublicationOnly)]
            [InlineData(LazyThreadSafetyMode.ExecutionAndPublication)]
            public void CreatesTaskWhichReturnsDefaultValueOfDecimal(LazyThreadSafetyMode mode)
            {
                var target = new AsyncLazy<decimal>(mode);

                Assert.Equal(new decimal(), target.Value.GetAwaiter().GetResult());
            }
        }

        public class Constructor_FuncOfTaskOfT1
        {
            [Fact]
            public void SetsIsValueCreatedToFalse()
            {
                var task = Task.FromResult(new object());

                var target = new AsyncLazy<object>(() => task);

                Assert.False(target.IsValueCreated);
            }

            [Fact]
            public void SetsValueToProvidedTask()
            {
                var task = Task.FromResult(new object());

                var target = new AsyncLazy<object>(() => task);

                Assert.Same(task, target.Value);
            }
        }

        public class Constructor_FuncOfTaskOfT1_Boolean
        {
            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void SetsIsValueCreatedToFalse(bool isThreadSafe)
            {
                var task = Task.FromResult(new object());

                var target = new AsyncLazy<object>(() => task, isThreadSafe);

                Assert.False(target.IsValueCreated);
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void SetsValueToProvidedTask(bool isThreadSafe)
            {
                var task = Task.FromResult(new object());

                var target = new AsyncLazy<object>(() => task, isThreadSafe);

                Assert.Same(task, target.Value);
            }
        }

        public class Constructor_FuncOfTaskOfT1_LazyThreadSafetyMode
        {
            [Theory]
            [InlineData(LazyThreadSafetyMode.None)]
            [InlineData(LazyThreadSafetyMode.PublicationOnly)]
            [InlineData(LazyThreadSafetyMode.ExecutionAndPublication)]
            public void SetsIsValueCreatedToFalse(LazyThreadSafetyMode mode)
            {
                var task = Task.FromResult(new object());

                var target = new AsyncLazy<object>(() => task, mode);

                Assert.False(target.IsValueCreated);
            }

            [Theory]
            [InlineData(LazyThreadSafetyMode.None)]
            [InlineData(LazyThreadSafetyMode.PublicationOnly)]
            [InlineData(LazyThreadSafetyMode.ExecutionAndPublication)]
            public void SetsValueToProvidedTask(LazyThreadSafetyMode mode)
            {
                var task = Task.FromResult(new object());

                var target = new AsyncLazy<object>(() => task, mode);

                Assert.Same(task, target.Value);
            }
        }

        public class GetAwaiter
        {
            [Fact]
            public void ReturnsCompletedAwaiterWithCorrectResult()
            {
                object expected = new object();

                var task = Task.FromResult(expected);

                var target = new AsyncLazy<object>(() => task);
                var awaiter = target.GetAwaiter();

                Assert.True(awaiter.IsCompleted);
                Assert.Same(expected, awaiter.GetResult());
            }

            [Fact]
            public async Task ReturnsCorrectResultWhenAwaited()
            {
                object expected = new object();

                var task = Task.FromResult(expected);

                var target = new AsyncLazy<object>(() => task);

                Assert.Same(expected, await target);
            }
        }
    }
}