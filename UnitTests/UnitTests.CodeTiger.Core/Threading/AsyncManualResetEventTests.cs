using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeTiger.Threading;
using CodeTiger.Threading.Tasks;
using Xunit;

namespace UnitTests.CodeTiger.Threading
{
    /// <summary>
    /// Contains unit tests for the <see cref="AsyncManualResetEvent"/> class.
    /// </summary>
    public class AsyncManualResetEventTests
    {
        public class WaitOne
        {
            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncManualResetEvent(true);

                var waitTask1 = Task.Run(() => target.WaitOne());
                var waitTask2 = Task.Run(() => target.WaitOne());
                var waitTask3 = Task.Run(() => target.WaitOne());

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne());
                var waitTask2 = Task.Run(() => target.WaitOne());
                var waitTask3 = Task.Run(() => target.WaitOne());

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask1 = Task.Run(() => target.WaitOne());
                var waitTask2 = Task.Run(() => target.WaitOne());
                var waitTask3 = Task.Run(() => target.WaitOne());

                Assert.Equal(-1, Task.WaitAny(new[] { waitTask1, waitTask2, waitTask3 }, 250));

                target.Set();

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(true);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne());
                var waitTask2 = Task.Run(() => target.WaitOne());
                var waitTask3 = Task.Run(() => target.WaitOne());

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));
            }
        }

        public class WaitOne_Int32
        {
            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask = Task.Run(() => target.WaitOne(250));

                Assert.False(waitTask.Wait(200));

                Assert.True(waitTask.Wait(100));
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncManualResetEvent(true);
                target.Reset();

                var waitTask = Task.Run(() => target.WaitOne(250));

                Assert.False(waitTask.Wait(200));

                Assert.True(waitTask.Wait(100));
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncManualResetEvent(true);

                var waitTask1 = Task.Run(() => target.WaitOne(250));
                var waitTask2 = Task.Run(() => target.WaitOne(250));
                var waitTask3 = Task.Run(() => target.WaitOne(250));

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(250));
                var waitTask2 = Task.Run(() => target.WaitOne(250));
                var waitTask3 = Task.Run(() => target.WaitOne(250));

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask1 = Task.Run(() => target.WaitOne(250));
                var waitTask2 = Task.Run(() => target.WaitOne(250));
                var waitTask3 = Task.Run(() => target.WaitOne(250));

                Assert.Equal(-1, Task.WaitAny(new[] { waitTask1, waitTask2, waitTask3 }, 150));

                target.Set();

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(true);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(250));
                var waitTask2 = Task.Run(() => target.WaitOne(250));
                var waitTask3 = Task.Run(() => target.WaitOne(250));

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }
        }

        public class WaitOne_TimeSpan
        {
            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.False(waitTask.Wait(200));

                Assert.True(waitTask.Wait(100));
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncManualResetEvent(true);
                target.Reset();

                var waitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.False(waitTask.Wait(200));

                Assert.True(waitTask.Wait(100));
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncManualResetEvent(true);

                var waitTask1 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));
                var waitTask2 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));
                var waitTask3 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));
                var waitTask2 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));
                var waitTask3 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask1 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));
                var waitTask2 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));
                var waitTask3 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.Equal(-1, Task.WaitAny(new[] { waitTask1, waitTask2, waitTask3 }, 200));

                target.Set();

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(true);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));
                var waitTask2 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));
                var waitTask3 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }
        }

        public class WaitOne_CancellationToken
        {
            [Fact]
            public void DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                Assert.False(waitTask.Wait(200));

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => waitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncManualResetEvent(true);

                var waitTask1 = Task.Run(() => target.WaitOne(CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(CancellationToken.None));

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(CancellationToken.None));

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask1 = Task.Run(() => target.WaitOne(CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(CancellationToken.None));

                target.Set();

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(true);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(CancellationToken.None));
                
                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));
            }
        }

        public class WaitOne_Int32_CancellationToken
        {
            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask = Task.Run(() => target.WaitOne(250, CancellationToken.None));

                Assert.True(waitTask.Wait(300));
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                Assert.Equal(-1, Task.WaitAny(new[] { waitTask1, waitTask2, waitTask3 }, 250));

                cancelSource.Cancel();

                var aggregateException1 = Assert.Throws<AggregateException>(() => waitTask1.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException1.Flatten().InnerExceptions.Single().GetType());
                var aggregateException2 = Assert.Throws<AggregateException>(() => waitTask2.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException2.Flatten().InnerExceptions.Single().GetType());
                var aggregateException3 = Assert.Throws<AggregateException>(() => waitTask3.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException3.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncManualResetEvent(true);
                target.Reset();

                var waitTask1 = Task.Run(() => target.WaitOne(250, CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(250, CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(250, CancellationToken.None));

                Assert.Equal(-1, Task.WaitAny(new[] { waitTask1, waitTask2, waitTask3 }, 200));

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 100));

                Assert.False(waitTask1.Result);
                Assert.False(waitTask2.Result);
                Assert.False(waitTask3.Result);
            }

            [Fact]
            public void DoesNotReturnUntilCancelTokenIsSetWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncManualResetEvent(true);
                target.Reset();

                var cancelSource = new CancellationTokenSource();
                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                Assert.Equal(-1, Task.WaitAny(new[] { waitTask1, waitTask2, waitTask3 }, 250));

                cancelSource.Cancel();

                var aggregateException1 = Assert.Throws<AggregateException>(() => waitTask1.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException1.Flatten().InnerExceptions.Single().GetType());
                var aggregateException2 = Assert.Throws<AggregateException>(() => waitTask2.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException2.Flatten().InnerExceptions.Single().GetType());
                var aggregateException3 = Assert.Throws<AggregateException>(() => waitTask3.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException3.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncManualResetEvent(true);

                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));

                target.Set();

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(true);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }
        }

        public class WaitOne_TimeSpan_CancellationToken
        {
            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask = Task.Run(
                    () => target.WaitOne(TimeSpan.FromMilliseconds(250), CancellationToken.None));

                Assert.False(waitTask.Wait(200));

                Assert.True(waitTask.Wait(100));
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                Assert.Equal(-1, Task.WaitAny(new[] { waitTask1, waitTask2, waitTask3 }, 250));

                cancelSource.Cancel();

                var aggregateException1 = Assert.Throws<AggregateException>(() => waitTask1.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException1.Flatten().InnerExceptions.Single().GetType());
                var aggregateException2 = Assert.Throws<AggregateException>(() => waitTask2.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException2.Flatten().InnerExceptions.Single().GetType());
                var aggregateException3 = Assert.Throws<AggregateException>(() => waitTask3.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException3.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncManualResetEvent(true);
                target.Reset();

                var waitTask1 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250),
                    CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250),
                    CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250),
                    CancellationToken.None));

                Assert.Equal(-1, Task.WaitAny(new[] { waitTask1, waitTask2, waitTask3 }, 200));

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 100));

                Assert.False(waitTask1.Result);
                Assert.False(waitTask2.Result);
                Assert.False(waitTask3.Result);
            }

            [Fact]
            public void DoesNotReturnUntilCancelTokenIsSetWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncManualResetEvent(true);
                target.Reset();

                var cancelSource = new CancellationTokenSource();
                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                Assert.Equal(-1, Task.WaitAny(new[] { waitTask1, waitTask2, waitTask3 }, 250));

                cancelSource.Cancel();

                var aggregateException1 = Assert.Throws<AggregateException>(() => waitTask1.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException1.Flatten().InnerExceptions.Single().GetType());
                var aggregateException2 = Assert.Throws<AggregateException>(() => waitTask2.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException2.Flatten().InnerExceptions.Single().GetType());
                var aggregateException3 = Assert.Throws<AggregateException>(() => waitTask3.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException3.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncManualResetEvent(true);

                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));

                target.Set();

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(true);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));

                Assert.True(Task.WaitAll(new[] { waitTask1, waitTask2, waitTask3 }, 50));

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }
        }

        public class WaitOneAsync
        {
            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncManualResetEvent(true);

                var waitTask1 = target.WaitOneAsync();
                var waitTask2 = target.WaitOneAsync();
                var waitTask3 = target.WaitOneAsync();

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);
                target.Set();

                var waitTask1 = target.WaitOneAsync();
                var waitTask2 = target.WaitOneAsync();
                var waitTask3 = target.WaitOneAsync();

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask1 = target.WaitOneAsync();
                var waitTask2 = target.WaitOneAsync();
                var waitTask3 = target.WaitOneAsync();

                await Task.Delay(250);

                Assert.False(waitTask1.IsCompleted);
                Assert.False(waitTask2.IsCompleted);
                Assert.False(waitTask3.IsCompleted);

                target.Set();

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(true);
                target.Set();

                var waitTask1 = target.WaitOneAsync();
                var waitTask2 = target.WaitOneAsync();
                var waitTask3 = target.WaitOneAsync();

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);
            }
        }

        public class WaitOneAsync_Int32
        {
            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask = target.WaitOneAsync(250);

                await Task.Delay(200);
                Assert.False(waitTask.IsCompleted);

                Assert.False(await waitTask.WithTimeout(100));
            }

            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncManualResetEvent(true);
                target.Reset();

                var waitTask = target.WaitOneAsync(250);

                await Task.Delay(200);
                Assert.False(waitTask.IsCompleted);

                Assert.False(await waitTask.WithTimeout(100));
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncManualResetEvent(true);

                var waitTask1 = target.WaitOneAsync(250);
                var waitTask2 = target.WaitOneAsync(250);
                var waitTask3 = target.WaitOneAsync(250);

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);
                target.Set();

                var waitTask1 = target.WaitOneAsync(250);
                var waitTask2 = target.WaitOneAsync(250);
                var waitTask3 = target.WaitOneAsync(250);

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask1 = target.WaitOneAsync(250);
                var waitTask2 = target.WaitOneAsync(250);
                var waitTask3 = target.WaitOneAsync(250);

                await Task.Delay(200);
                Assert.False(waitTask1.IsCompleted);
                Assert.False(waitTask2.IsCompleted);
                Assert.False(waitTask3.IsCompleted);

                target.Set();

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(true);
                target.Set();

                var waitTask1 = target.WaitOneAsync(250);
                var waitTask2 = target.WaitOneAsync(250);
                var waitTask3 = target.WaitOneAsync(250);

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }
        }

        public class WaitOneAsync_TimeSpan
        {
            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                await Task.Delay(200);

                Assert.False(waitTask.IsCompleted);

                Assert.False(await waitTask.WithTimeout(100));
            }

            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncManualResetEvent(true);
                target.Reset();

                var waitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                await Task.Delay(200);
                Assert.False(waitTask.IsCompleted);
                
                Assert.False(await waitTask.WithTimeout(100));
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncManualResetEvent(true);

                var waitTask1 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));
                var waitTask2 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));
                var waitTask3 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncManualResetEvent(false);
                target.Set();

                var waitTask1 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));
                var waitTask2 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));
                var waitTask3 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask1 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));
                var waitTask2 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));
                var waitTask3 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                await Task.Delay(200);
                Assert.False(waitTask1.IsCompleted);
                Assert.False(waitTask2.IsCompleted);
                Assert.False(waitTask3.IsCompleted);

                target.Set();

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncManualResetEvent(true);
                target.Set();

                var waitTask1 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));
                var waitTask2 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));
                var waitTask3 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }
        }

        public class WaitOneAsync_CancellationToken
        {
            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask = target.WaitOneAsync(cancelSource.Token);

                await Task.Delay(200);
                Assert.False(waitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask.WithTimeout(50));
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncManualResetEvent(true);

                var waitTask1 = target.WaitOneAsync(CancellationToken.None);
                var waitTask2 = target.WaitOneAsync(CancellationToken.None);
                var waitTask3 = target.WaitOneAsync(CancellationToken.None);

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncManualResetEvent(false);
                target.Set();

                var waitTask1 = target.WaitOneAsync(CancellationToken.None);
                var waitTask2 = target.WaitOneAsync(CancellationToken.None);
                var waitTask3 = target.WaitOneAsync(CancellationToken.None);

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask1 = target.WaitOneAsync(CancellationToken.None);
                var waitTask2 = target.WaitOneAsync(CancellationToken.None);
                var waitTask3 = target.WaitOneAsync(CancellationToken.None);

                target.Set();

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncManualResetEvent(true);
                target.Set();

                var waitTask1 = target.WaitOneAsync(CancellationToken.None);
                var waitTask2 = target.WaitOneAsync(CancellationToken.None);
                var waitTask3 = target.WaitOneAsync(CancellationToken.None);

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);
            }
        }

        public class WaitOneAsync_Int32_CancellationToken
        {
            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask = target.WaitOneAsync(250, CancellationToken.None);

                await Task.Delay(200);

                Assert.False(waitTask.IsCompleted);

                Assert.False(await waitTask.WithTimeout(100));
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask1 = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);
                var waitTask2 = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);
                var waitTask3 = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                await Task.Delay(200);

                Assert.False(waitTask1.IsCompleted);
                Assert.False(waitTask2.IsCompleted);
                Assert.False(waitTask3.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask1.WithTimeout(50));
                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask2.WithTimeout(50));
                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask3.WithTimeout(50));
            }

            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncManualResetEvent(true);
                target.Reset();

                var waitTask1 = target.WaitOneAsync(250, CancellationToken.None);
                var waitTask2 = target.WaitOneAsync(250, CancellationToken.None);
                var waitTask3 = target.WaitOneAsync(250, CancellationToken.None);

                await Task.Delay(200);

                Assert.False(waitTask1.IsCompleted);
                Assert.False(waitTask2.IsCompleted);
                Assert.False(waitTask3.IsCompleted);

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(100);

                Assert.False(waitTask1.Result);
                Assert.False(waitTask2.Result);
                Assert.False(waitTask3.Result);
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncManualResetEvent(true);
                target.Reset();

                var cancelSource = new CancellationTokenSource();
                var waitTask1 = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);
                var waitTask2 = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);
                var waitTask3 = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                await Task.Delay(200);

                Assert.False(waitTask1.IsCompleted);
                Assert.False(waitTask2.IsCompleted);
                Assert.False(waitTask3.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask1.WithTimeout(50));
                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask2.WithTimeout(50));
                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask3.WithTimeout(50));
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncManualResetEvent(true);

                var waitTask1 = target.WaitOneAsync(Timeout.Infinite, CancellationToken.None);
                var waitTask2 = target.WaitOneAsync(Timeout.Infinite, CancellationToken.None);
                var waitTask3 = target.WaitOneAsync(Timeout.Infinite, CancellationToken.None);

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);
                target.Set();

                var waitTask1 = target.WaitOneAsync(Timeout.Infinite, CancellationToken.None);
                var waitTask2 = target.WaitOneAsync(Timeout.Infinite, CancellationToken.None);
                var waitTask3 = target.WaitOneAsync(Timeout.Infinite, CancellationToken.None);

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask1 = target.WaitOneAsync(Timeout.Infinite, CancellationToken.None);
                var waitTask2 = target.WaitOneAsync(Timeout.Infinite, CancellationToken.None);
                var waitTask3 = target.WaitOneAsync(Timeout.Infinite, CancellationToken.None);

                target.Set();

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsSetTwiceBeforeCallingWaitOnAsynce()
            {
                var target = new AsyncManualResetEvent(true);
                target.Set();

                var waitTask1 = target.WaitOneAsync(Timeout.Infinite, CancellationToken.None);
                var waitTask2 = target.WaitOneAsync(Timeout.Infinite, CancellationToken.None);
                var waitTask3 = target.WaitOneAsync(Timeout.Infinite, CancellationToken.None);

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }
        }

        public class WaitOneAsync_TimeSpan_CancellationToken
        {
            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250), CancellationToken.None);

                await Task.Delay(200);

                Assert.False(waitTask.IsCompleted);

                Assert.False(await waitTask.WithTimeout(100));
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask1 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);
                var waitTask2 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);
                var waitTask3 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                await Task.Delay(200);

                Assert.False(waitTask1.IsCompleted);
                Assert.False(waitTask2.IsCompleted);
                Assert.False(waitTask3.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask1.WithTimeout(50));
                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask2.WithTimeout(50));
                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask3.WithTimeout(50));
            }

            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncManualResetEvent(true);
                target.Reset();

                var waitTask1 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250), CancellationToken.None);
                var waitTask2 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250), CancellationToken.None);
                var waitTask3 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250), CancellationToken.None);

                await Task.Delay(200);

                Assert.False(waitTask1.IsCompleted);
                Assert.False(waitTask2.IsCompleted);
                Assert.False(waitTask3.IsCompleted);

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(100);

                Assert.False(waitTask1.Result);
                Assert.False(waitTask2.Result);
                Assert.False(waitTask3.Result);
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncManualResetEvent(true);
                target.Reset();

                var cancelSource = new CancellationTokenSource();
                var waitTask1 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);
                var waitTask2 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);
                var waitTask3 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                await Task.Delay(200);

                Assert.False(waitTask1.IsCompleted);
                Assert.False(waitTask2.IsCompleted);
                Assert.False(waitTask3.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask1.WithTimeout(50));
                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask2.WithTimeout(50));
                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask3.WithTimeout(50));
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncManualResetEvent(true);

                var waitTask1 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, CancellationToken.None);
                var waitTask2 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, CancellationToken.None);
                var waitTask3 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, CancellationToken.None);

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncManualResetEvent(false);
                target.Set();

                var waitTask1 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, CancellationToken.None);
                var waitTask2 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, CancellationToken.None);
                var waitTask3 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, CancellationToken.None);

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask1 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, CancellationToken.None);
                var waitTask2 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, CancellationToken.None);
                var waitTask3 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, CancellationToken.None);

                target.Set();

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }

            [Fact]
            public async Task ReturnsMoreThanOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncManualResetEvent(true);
                target.Set();

                var waitTask1 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, CancellationToken.None);
                var waitTask2 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, CancellationToken.None);
                var waitTask3 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, CancellationToken.None);

                await Task.WhenAll(waitTask1, waitTask2, waitTask3).WithTimeout(50);

                Assert.True(waitTask1.Result);
                Assert.True(waitTask2.Result);
                Assert.True(waitTask3.Result);
            }
        }
    }
}
