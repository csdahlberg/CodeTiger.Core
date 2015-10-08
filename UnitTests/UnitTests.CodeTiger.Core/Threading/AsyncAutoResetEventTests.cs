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
    /// Contains unit tests for the <see cref="AsyncAutoResetEvent"/> class.
    /// </summary>
    public class AsyncAutoResetEventTests
    {
        public class WaitOne
        {
            [Fact]
            public void ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask = Task.Run(() => target.WaitOne());
                
                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(250))).Result);
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var successfulWaitTask = Task.Run(() => target.WaitOne());

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(250))).Result);
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);

                var successfulWaitTask = Task.Run(() => target.WaitOne());
                
                Assert.False(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(250))).Result);

                target.Set();

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
            }

            [Fact]
            public void ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var successfulWaitTask = Task.Run(() => target.WaitOne());

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
            }

            [Fact]
            public void ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask1 = Task.Run(() => target.WaitOne());

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var successfulWaitTask2 = Task.Run(() => target.WaitOne());

                // Add a small delay to make sure the second task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                Assert.True(Task.Run(() => successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(250))).Result);
                Assert.False(Task.Run(() => successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(250))).Result);

                target.Set();

                Assert.True(Task.Run(() => successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50))).Result);
            }
        }

        public class WaitOne_Int32
        {
            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var waitTask = Task.Run(() => target.WaitOne(250));

                Assert.False(Task.Run(() => waitTask.Wait(TimeSpan.FromMilliseconds(200))).Result);
                Assert.True(Task.Run(() => waitTask.Wait(TimeSpan.FromMilliseconds(100))).Result);
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = Task.Run(() => target.WaitOne(250));

                Assert.False(Task.Run(() => waitTask.Wait(TimeSpan.FromMilliseconds(200))).Result);
                Assert.True(Task.Run(() => waitTask.Wait(TimeSpan.FromMilliseconds(100))).Result);
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask = Task.Run(() => target.WaitOne(250));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(250));

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask.Result);
                Assert.True(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(300))).Result);
                Assert.False(unsuccessfulWaitTask.Result);
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var successfulWaitTask = Task.Run(() => target.WaitOne(250));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(250));

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask.Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                Assert.True(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(300))).Result);
                Assert.False(unsuccessfulWaitTask.Result);
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);

                var successfulWaitTask = Task.Run(() => target.WaitOne(250));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(250));

                Assert.False(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                target.Set();

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask.Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                Assert.True(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(300))).Result);
                Assert.False(unsuccessfulWaitTask.Result);
            }

            [Fact]
            public void ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var successfulWaitTask = Task.Run(() => target.WaitOne(250));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(250));

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                Assert.True(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(300))).Result);
                Assert.False(unsuccessfulWaitTask.Result);
            }

            [Fact]
            public void ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask1 = Task.Run(() => target.WaitOne(250));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var successfulWaitTask2 = Task.Run(() => target.WaitOne(250));

                // Add a small delay to make sure the second task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(250));

                Assert.True(Task.Run(() => successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask1.Result);
                Assert.False(Task.Run(() => successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                target.Set();

                Assert.True(Task.Run(() => successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask2.Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                Assert.True(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000))).Result);
                Assert.False(unsuccessfulWaitTask.Result);
            }
        }

        public class WaitOne_TimeSpan
        {
            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var waitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.False(Task.Run(() => waitTask.Wait(TimeSpan.FromMilliseconds(200))).Result);

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(Task.Run(() => waitTask.Wait(TimeSpan.FromMilliseconds(100))).Result);
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.False(Task.Run(() => waitTask.Wait(TimeSpan.FromMilliseconds(200))).Result);

                Assert.True(Task.Run(() => waitTask.Wait(TimeSpan.FromMilliseconds(100))).Result);
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask.Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(300))).Result);
                Assert.False(unsuccessfulWaitTask.Result);
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var successfulWaitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask.Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                Assert.True(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(300))).Result);
                Assert.False(unsuccessfulWaitTask.Result);
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);

                var successfulWaitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.False(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                target.Set();

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask.Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(300))).Result);
                Assert.False(unsuccessfulWaitTask.Result);
            }

            [Fact]
            public void ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var successfulWaitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask.Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                Assert.True(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(300))).Result);
                Assert.False(unsuccessfulWaitTask.Result);
            }

            [Fact]
            public void ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask1 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var successfulWaitTask2 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                // Add a small delay to make sure the second task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.True(Task.Run(() => successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask1.Result);
                Assert.False(Task.Run(() => successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                target.Set();

                Assert.True(Task.Run(() => successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask2.Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(300))).Result);
                Assert.False(unsuccessfulWaitTask.Result);
            }
        }

        public class WaitOne_CancellationToken
        {
            [Fact]
            public void DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                Assert.False(Task.Run(() => waitTask.Wait(TimeSpan.FromMilliseconds(250))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => waitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => unsuccessfulWaitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => unsuccessfulWaitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                Assert.False(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                target.Set();

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => unsuccessfulWaitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => unsuccessfulWaitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask1 = Task.Run(() => target.WaitOne(cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var successfulWaitTask2 = Task.Run(() => target.WaitOne(cancelSource.Token));

                // Add a small delay to make sure the second task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                Assert.True(Task.Run(() => successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.False(Task.Run(() => successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                target.Set();

                Assert.True(Task.Run(() => successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => unsuccessfulWaitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }
        }

        public class WaitOne_Int32_CancellationToken
        {
            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var waitTask = Task.Run(() => target.WaitOne(250, CancellationToken.None));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(Task.Run(() => waitTask.Wait(TimeSpan.FromMilliseconds(1000))).Result);
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                Assert.False(Task.Run(() => waitTask.Wait(TimeSpan.FromMilliseconds(250))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => waitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = Task.Run(() => target.WaitOne(250, CancellationToken.None));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(Task.Run(() => waitTask.Wait(TimeSpan.FromMilliseconds(1000))).Result);
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void DoesNotReturnUntilCancelTokenIsSetWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var cancelSource = new CancellationTokenSource();
                var waitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                Assert.False(Task.Run(() => waitTask.Wait(TimeSpan.FromMilliseconds(250))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => waitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask.Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => unsuccessfulWaitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask.Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => unsuccessfulWaitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                Assert.False(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                target.Set();

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => unsuccessfulWaitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask.Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => unsuccessfulWaitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask1 = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var successfulWaitTask2 = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                // Add a small delay to make sure the second task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                Assert.True(Task.Run(() => successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask1.Result);
                Assert.False(Task.Run(() => successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                target.Set();

                Assert.True(Task.Run(() => successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask2.Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => unsuccessfulWaitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }
        }

        public class WaitOne_TimeSpan_CancellationToken
        {
            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var waitTask = Task.Run(
                    () => target.WaitOne(TimeSpan.FromMilliseconds(250), CancellationToken.None));

                Assert.False(Task.Run(() => waitTask.Wait(TimeSpan.FromMilliseconds(150))).Result);

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(Task.Run(() => waitTask.Wait(TimeSpan.FromMilliseconds(1000))).Result);
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask = Task.Run(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                Assert.False(Task.Run(() => waitTask.Wait(TimeSpan.FromMilliseconds(250))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => waitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = Task.Run(
                    () => target.WaitOne(TimeSpan.FromMilliseconds(250), CancellationToken.None));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(Task.Run(() => waitTask.Wait(TimeSpan.FromMilliseconds(300))).Result);
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void DoesNotReturnUntilCancelTokenIsSetWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var cancelSource = new CancellationTokenSource();
                var waitTask = Task.Run(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                Assert.False(Task.Run(() => waitTask.Wait(TimeSpan.FromMilliseconds(250))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => waitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask.Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => unsuccessfulWaitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask.Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => unsuccessfulWaitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                Assert.False(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                target.Set();

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask.Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => unsuccessfulWaitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                Assert.True(Task.Run(() => successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask.Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => unsuccessfulWaitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask1 = Task.Run(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var successfulWaitTask2 = Task.Run(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                // Add a small delay to make sure the second task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                Assert.True(Task.Run(() => successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask1.Result);
                Assert.False(Task.Run(() => successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                target.Set();

                Assert.True(Task.Run(() => successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50))).Result);
                Assert.True(successfulWaitTask2.Result);
                Assert.False(Task.Run(() => unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50))).Result);

                cancelSource.Cancel();

                var aggregateException = Assert.Throws<AggregateException>(() => unsuccessfulWaitTask.Wait(50));
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }
        }

        public class WaitOneAsync
        {
            [Fact]
            public async Task ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask = target.WaitOneAsync();

                await successfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(250));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var successfulWaitTask = target.WaitOneAsync();

                await successfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(250));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);

                var successfulWaitTask = target.WaitOneAsync();

                await Task.Delay(250);

                Assert.False(successfulWaitTask.IsCompleted);

                target.Set();

                await successfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(50));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var successfulWaitTask = target.WaitOneAsync();

                await successfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(50));
            }

            [Fact]
            public async Task ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask1 = target.WaitOneAsync();

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(50);

                var successfulWaitTask2 = target.WaitOneAsync();

                // Add a small delay to make sure the second task calls WaitOneAsync first
                await Task.Delay(50);

                await successfulWaitTask1.WithTimeout(TimeSpan.FromMilliseconds(50));

                await Task.Delay(50);
                Assert.False(successfulWaitTask2.IsCompleted);

                target.Set();

                await successfulWaitTask2.WithTimeout(TimeSpan.FromMilliseconds(50));
            }
        }

        public class WaitOneAsync_Int32
        {
            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var waitTask = target.WaitOneAsync(250);

                await Task.Delay(200);
                Assert.False(waitTask.IsCompleted);

                Assert.False(await waitTask.WithTimeout(100));
            }

            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = target.WaitOneAsync(250);

                await Task.Delay(200);
                Assert.False(waitTask.IsCompleted);

                Assert.False(await waitTask.WithTimeout(TimeSpan.FromMilliseconds(100)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask = target.WaitOneAsync(250);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(250);

                Assert.True(await successfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(50)));

                await Task.Delay(200);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(100)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var successfulWaitTask = target.WaitOneAsync(250);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(250);

                Assert.True(await successfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(50)));

                await Task.Delay(200);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(300)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);

                var successfulWaitTask = target.WaitOneAsync(250);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(250);

                await Task.Delay(50);
                Assert.False(successfulWaitTask.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                await Task.Delay(50);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(successfulWaitTask.Result);

                await Task.Delay(100);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(300)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var successfulWaitTask = target.WaitOneAsync(250);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(250);

                Assert.True(await successfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(50)));

                await Task.Delay(200);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(300)));
            }

            [Fact]
            public async Task ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask1 = target.WaitOneAsync(250);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var successfulWaitTask2 = target.WaitOneAsync(250);

                // Add a small delay to make sure the second task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(250);

                Assert.True(await successfulWaitTask1.WithTimeout(TimeSpan.FromMilliseconds(50)));

                await Task.Delay(TimeSpan.FromMilliseconds(50));
                Assert.False(successfulWaitTask2.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                Assert.True(await successfulWaitTask2.WithTimeout(TimeSpan.FromMilliseconds(50)));

                await Task.Delay(TimeSpan.FromMilliseconds(50));
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.WithTimeout(100));
            }
        }

        public class WaitOneAsync_TimeSpan
        {
            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var waitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                await Task.Delay(200);
                Assert.False(waitTask.IsCompleted);

                Assert.False(await waitTask.WithTimeout(100));
            }

            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                await Task.Delay(200);
                Assert.False(waitTask.IsCompleted);

                Assert.False(await waitTask.WithTimeout(TimeSpan.FromMilliseconds(100)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                Assert.True(await successfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(50)));

                await Task.Delay(200);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(100)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var successfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                Assert.True(await successfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(50)));

                await Task.Delay(200);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(300)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);

                var successfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                await Task.Delay(50);
                Assert.False(successfulWaitTask.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                await Task.Delay(50);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(successfulWaitTask.Result);

                await Task.Delay(100);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(300)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var successfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                Assert.True(await successfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(50)));

                await Task.Delay(200);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(300)));
            }

            [Fact]
            public async Task ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask1 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var successfulWaitTask2 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                // Add a small delay to make sure the second task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                Assert.True(await successfulWaitTask1.WithTimeout(TimeSpan.FromMilliseconds(50)));

                await Task.Delay(TimeSpan.FromMilliseconds(50));
                Assert.False(successfulWaitTask2.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                Assert.True(await successfulWaitTask2.WithTimeout(TimeSpan.FromMilliseconds(50)));

                await Task.Delay(TimeSpan.FromMilliseconds(50));
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.WithTimeout(100));
            }
        }

        public class WaitOneAsync_CancellationToken
        {
            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask = target.WaitOneAsync(cancelSource.Token);

                await Task.Delay(250);
                Assert.False(waitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask.WithTimeout(50));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                await Task.Delay(TimeSpan.FromMilliseconds(50));
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                await Task.Delay(50);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                await Task.Delay(50);
                Assert.False(successfulWaitTask.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                await Task.Delay(50);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                await Task.Delay(50);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50));
            }

            [Fact]
            public async Task ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask1 = target.WaitOneAsync(cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var successfulWaitTask2 = target.WaitOneAsync(cancelSource.Token);

                // Add a small delay to make sure the second task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                await Task.Delay(TimeSpan.FromMilliseconds(50));
                Assert.True(successfulWaitTask1.IsCompleted);
                Assert.True(successfulWaitTask1.Result);
                Assert.False(successfulWaitTask2.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                await Task.Delay(TimeSpan.FromMilliseconds(50));
                Assert.True(successfulWaitTask2.IsCompleted);
                Assert.True(successfulWaitTask2.Result);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50));
            }
        }

        public class WaitOneAsync_Int32_CancellationToken
        {
            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var waitTask = target.WaitOneAsync(250, CancellationToken.None);

                await Task.Delay(200);
                Assert.False(waitTask.IsCompleted);

                Assert.False(await waitTask.WithTimeout(100));
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                await Task.Delay(250);
                Assert.False(waitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask.WithTimeout(50));
            }

            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = target.WaitOneAsync(250, CancellationToken.None);

                Assert.False(await waitTask.WithTimeout(TimeSpan.FromMilliseconds(300)));
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var cancelSource = new CancellationTokenSource();
                var waitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                await Task.Delay(TimeSpan.FromMilliseconds(250));
                Assert.False(waitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask.WithTimeout(50));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                await Task.Delay(TimeSpan.FromMilliseconds(50));
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                await Task.Delay(50);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                await Task.Delay(50);
                Assert.False(successfulWaitTask.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                await Task.Delay(50);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                await Task.Delay(50);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50));
            }

            [Fact]
            public async Task ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask1 = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var successfulWaitTask2 = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                // Add a small delay to make sure the second task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                await Task.Delay(TimeSpan.FromMilliseconds(50));
                Assert.True(successfulWaitTask1.IsCompleted);
                Assert.True(successfulWaitTask1.Result);
                Assert.False(successfulWaitTask2.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                await Task.Delay(TimeSpan.FromMilliseconds(50));
                Assert.True(successfulWaitTask2.IsCompleted);
                Assert.True(successfulWaitTask2.Result);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50));
            }
        }

        public class WaitOneAsync_TimeSpan_CancellationToken
        {
            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var waitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250), CancellationToken.None);

                await Task.Delay(200);
                Assert.False(waitTask.IsCompleted);

                Assert.False(await waitTask.WithTimeout(100));
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                await Task.Delay(250);
                Assert.False(waitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask.WithTimeout(50));
            }

            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250), CancellationToken.None);

                Assert.False(await waitTask.WithTimeout(TimeSpan.FromMilliseconds(300)));
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var cancelSource = new CancellationTokenSource();
                var waitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                await Task.Delay(TimeSpan.FromMilliseconds(250));
                Assert.False(waitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask.WithTimeout(50));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                await Task.Delay(TimeSpan.FromMilliseconds(50));
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                await Task.Delay(50);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                await Task.Delay(50);
                Assert.False(successfulWaitTask.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                await Task.Delay(50);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                await Task.Delay(50);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50));
            }

            [Fact]
            public async Task ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask1 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var successfulWaitTask2 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                // Add a small delay to make sure the second task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                await Task.Delay(TimeSpan.FromMilliseconds(50));
                Assert.True(successfulWaitTask1.IsCompleted);
                Assert.True(successfulWaitTask1.Result);
                Assert.False(successfulWaitTask2.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                await Task.Delay(TimeSpan.FromMilliseconds(50));
                Assert.True(successfulWaitTask2.IsCompleted);
                Assert.True(successfulWaitTask2.Result);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50));
            }
        }
    }
}