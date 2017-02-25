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

                target.WaitOne();
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                target.WaitOne();
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);

                var successfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(),
                    TaskCreationOptions.LongRunning);

                Thread.Sleep(TimeSpan.FromMilliseconds(250));

                Assert.False(successfulWaitTask.IsCompleted);

                target.Set();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
            }

            [Fact]
            public void ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                target.WaitOne();
            }

            [Fact]
            public void ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask1 = Task.Factory.StartNew(() => target.WaitOne(),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var successfulWaitTask2 = Task.Factory.StartNew(() => target.WaitOne(),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the second task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                Assert.True(successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(250)));
                Assert.False(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(250)));

                target.Set();

                Assert.True(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
            }
        }

        public class WaitOne_Int32
        {
            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var waitTask = Task.Factory.StartNew(() => target.WaitOne(250),
                    TaskCreationOptions.LongRunning);

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(200)));
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(100)));
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = Task.Factory.StartNew(() => target.WaitOne(250), TaskCreationOptions.LongRunning);

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(200)));
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(100)));
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(250),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(250),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask.Result);
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(300)));
                Assert.False(unsuccessfulWaitTask.Result);
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var successfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(250),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(250),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(300)));
                Assert.False(unsuccessfulWaitTask.Result);
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);

                var successfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(250),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(250),
                    TaskCreationOptions.LongRunning);

                Assert.False(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(300)));
                Assert.False(unsuccessfulWaitTask.Result);
            }

            [Fact]
            public void ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var successfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(250),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(250),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(300)));
                Assert.False(unsuccessfulWaitTask.Result);
            }

            [Fact]
            public void ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask1 = Task.Factory.StartNew(() => target.WaitOne(250),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var successfulWaitTask2 = Task.Factory.StartNew(() => target.WaitOne(250),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the second task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(250),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask1.Result);
                Assert.False(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask2.Result);
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.Result);
            }
        }

        public class WaitOne_TimeSpan
        {
            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var waitTask = Task.Factory.StartNew(() => target.WaitOne(TimeSpan.FromMilliseconds(250)),
                    TaskCreationOptions.LongRunning);

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(200)));

                // Wait for the task to complete so aren't any lingering background threads.
                waitTask.Wait();
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = Task.Factory.StartNew(() => target.WaitOne(TimeSpan.FromMilliseconds(250)),
                    TaskCreationOptions.LongRunning);

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(200)));

                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(100)));
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(TimeSpan.FromMilliseconds(250)),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(TimeSpan.FromMilliseconds(250)),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(300)));
                Assert.False(unsuccessfulWaitTask.Result);
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var successfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(TimeSpan.FromMilliseconds(250)),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(TimeSpan.FromMilliseconds(250)),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(300)));
                Assert.False(unsuccessfulWaitTask.Result);
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);

                var successfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(TimeSpan.FromMilliseconds(250)),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(TimeSpan.FromMilliseconds(250)),
                    TaskCreationOptions.LongRunning);

                Assert.False(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(300)));
                Assert.False(unsuccessfulWaitTask.Result);
            }

            [Fact]
            public void ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var successfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(TimeSpan.FromMilliseconds(250)),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(TimeSpan.FromMilliseconds(250)),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(300)));
                Assert.False(unsuccessfulWaitTask.Result);
            }

            [Fact]
            public void ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask1 = Task.Factory.StartNew(() => target.WaitOne(TimeSpan.FromMilliseconds(250)),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var successfulWaitTask2 = Task.Factory.StartNew(() => target.WaitOne(TimeSpan.FromMilliseconds(250)),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the second task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(TimeSpan.FromMilliseconds(250)),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask1.Result);
                Assert.False(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask2.Result);
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(300)));
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
                var waitTask = Task.Factory.StartNew(() => target.WaitOne(cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(250)));

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
                var successfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

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
                var successfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

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
                var successfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.False(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

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
                var successfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

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
                var successfulWaitTask1 = Task.Factory.StartNew(() => target.WaitOne(cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var successfulWaitTask2 = Task.Factory.StartNew(() => target.WaitOne(cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the second task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

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

                var waitTask = Task.Factory.StartNew(() => target.WaitOne(250, CancellationToken.None),
                    TaskCreationOptions.LongRunning);

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask = Task.Factory.StartNew(() => target.WaitOne(Timeout.Infinite, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(250)));

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

                var waitTask = Task.Factory.StartNew(() => target.WaitOne(250, CancellationToken.None),
                    TaskCreationOptions.LongRunning);

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void DoesNotReturnUntilCancelTokenIsSetWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var cancelSource = new CancellationTokenSource();
                var waitTask = Task.Factory.StartNew(() => target.WaitOne(Timeout.Infinite, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(250)));

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
                var successfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(Timeout.Infinite, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(Timeout.Infinite, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

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
                var successfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(Timeout.Infinite, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(Timeout.Infinite, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

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
                var successfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(Timeout.Infinite, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(Timeout.Infinite, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.False(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

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
                var successfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(Timeout.Infinite, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(Timeout.Infinite, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

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
                var successfulWaitTask1 = Task.Factory.StartNew(() => target.WaitOne(Timeout.Infinite, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var successfulWaitTask2 = Task.Factory.StartNew(() => target.WaitOne(Timeout.Infinite, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the second task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(() => target.WaitOne(Timeout.Infinite, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask1.Result);
                Assert.False(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask2.Result);
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

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

                var waitTask = Task.Factory.StartNew(
                    () => target.WaitOne(TimeSpan.FromMilliseconds(250), CancellationToken.None),
                    TaskCreationOptions.LongRunning);

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(150)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask = Task.Factory.StartNew(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(250)));

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

                var waitTask = Task.Factory.StartNew(
                    () => target.WaitOne(TimeSpan.FromMilliseconds(250), CancellationToken.None),
                    TaskCreationOptions.LongRunning);

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(300)));
                Assert.False(waitTask.Result);
            }

            [Fact]
            public void DoesNotReturnUntilCancelTokenIsSetWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var cancelSource = new CancellationTokenSource();
                var waitTask = Task.Factory.StartNew(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(250)));

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
                var successfulWaitTask = Task.Factory.StartNew(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

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
                var successfulWaitTask = Task.Factory.StartNew(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

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
                var successfulWaitTask = Task.Factory.StartNew(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.False(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

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
                var successfulWaitTask = Task.Factory.StartNew(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask.Result);
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

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
                var successfulWaitTask1 = Task.Factory.StartNew(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the first task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var successfulWaitTask2 = Task.Factory.StartNew(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                // Add a small delay to make sure the second task calls WaitOne first
                Thread.Sleep(TimeSpan.FromMilliseconds(50));

                var unsuccessfulWaitTask = Task.Factory.StartNew(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token),
                    TaskCreationOptions.LongRunning);

                Assert.True(successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask1.Result);
                Assert.False(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(successfulWaitTask2.Result);
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

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

                await target.WaitOneAsync().ConfigureAwait(false);
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                await target.WaitOneAsync().ConfigureAwait(false);
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);

                var successfulWaitTask = target.WaitOneAsync();

                await Task.Delay(TimeSpan.FromMilliseconds(250)).ConfigureAwait(false);

                Assert.False(successfulWaitTask.IsCompleted);

                target.Set();

                await successfulWaitTask.ConfigureAwait(false);
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var successfulWaitTask = target.WaitOneAsync();

                await successfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
            }

            [Fact]
            public async Task ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask1 = target.WaitOneAsync();

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var successfulWaitTask2 = target.WaitOneAsync();

                // Add a small delay to make sure the second task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                await successfulWaitTask1.ConfigureAwait(false);

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
                Assert.False(successfulWaitTask2.IsCompleted);

                target.Set();

                await successfulWaitTask2.ConfigureAwait(false);
            }
        }

        public class WaitOneAsync_Int32
        {
            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var waitTask = target.WaitOneAsync(250);

                await Task.Delay(TimeSpan.FromMilliseconds(200)).ConfigureAwait(false);
                Assert.False(waitTask.IsCompleted);

                Assert.False(await waitTask.ConfigureAwait(false));
            }

            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = target.WaitOneAsync(250);

                await Task.Delay(TimeSpan.FromMilliseconds(200)).ConfigureAwait(false);
                Assert.False(waitTask.IsCompleted);

                Assert.False(await waitTask.ConfigureAwait(false));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask = target.WaitOneAsync(250);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(250);

                Assert.True(await successfulWaitTask.ConfigureAwait(false));

                await Task.Delay(TimeSpan.FromMilliseconds(200)).ConfigureAwait(false);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.ConfigureAwait(false));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var successfulWaitTask = target.WaitOneAsync(250);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(250);

                Assert.True(await successfulWaitTask.ConfigureAwait(false));

                await Task.Delay(TimeSpan.FromMilliseconds(200)).ConfigureAwait(false);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.ConfigureAwait(false));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);

                var successfulWaitTask = target.WaitOneAsync(250);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(250);

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
                Assert.False(successfulWaitTask.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(await successfulWaitTask.ConfigureAwait(false));
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.ConfigureAwait(false));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var successfulWaitTask = target.WaitOneAsync(250);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(250);

                Assert.True(await successfulWaitTask.ConfigureAwait(false));

                await Task.Delay(TimeSpan.FromMilliseconds(200)).ConfigureAwait(false);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.ConfigureAwait(false));
            }

            [Fact]
            public async Task ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask1 = target.WaitOneAsync(250);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var successfulWaitTask2 = target.WaitOneAsync(250);

                // Add a small delay to make sure the second task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(250);

                Assert.True(await successfulWaitTask1.ConfigureAwait(false));

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
                Assert.False(successfulWaitTask2.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                Assert.True(await successfulWaitTask2.ConfigureAwait(false));

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.WithTimeout(100).ConfigureAwait(false));
            }
        }

        public class WaitOneAsync_TimeSpan
        {
            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var waitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                await Task.Delay(TimeSpan.FromMilliseconds(200)).ConfigureAwait(false);
                Assert.False(waitTask.IsCompleted);

                Assert.False(await waitTask.WithTimeout(100).ConfigureAwait(false));
            }

            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                await Task.Delay(200).ConfigureAwait(false);
                Assert.False(waitTask.IsCompleted);

                Assert.False(await waitTask.ConfigureAwait(false));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                Assert.True(await successfulWaitTask.ConfigureAwait(false));

                await Task.Delay(200).ConfigureAwait(false);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.ConfigureAwait(false));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var successfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                Assert.True(await successfulWaitTask.ConfigureAwait(false));

                await Task.Delay(200).ConfigureAwait(false);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.ConfigureAwait(false));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);

                var successfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                await Task.Delay(50).ConfigureAwait(false);
                Assert.False(successfulWaitTask.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                Assert.True(await successfulWaitTask.ConfigureAwait(false));
                await Task.Delay(50).ConfigureAwait(false);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.ConfigureAwait(false));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var successfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                Assert.True(await successfulWaitTask.ConfigureAwait(false));

                await Task.Delay(200).ConfigureAwait(false);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.ConfigureAwait(false));
            }

            [Fact]
            public async Task ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask1 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var successfulWaitTask2 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                // Add a small delay to make sure the second task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                Assert.True(await successfulWaitTask1.ConfigureAwait(false));

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
                Assert.False(successfulWaitTask2.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                Assert.True(await successfulWaitTask2.ConfigureAwait(false));

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                Assert.False(await unsuccessfulWaitTask.WithTimeout(100).ConfigureAwait(false));
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

                await Task.Delay(250).ConfigureAwait(false);
                Assert.False(waitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask.WithTimeout(50))
                    .ConfigureAwait(false);
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(await successfulWaitTask.ConfigureAwait(false));
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50))
                    .ConfigureAwait(false);
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                await Task.Delay(50).ConfigureAwait(false);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(await successfulWaitTask.ConfigureAwait(false));
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50))
                    .ConfigureAwait(false);
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                await Task.Delay(50).ConfigureAwait(false);
                Assert.False(successfulWaitTask.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                await Task.Delay(50).ConfigureAwait(false);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(await successfulWaitTask.ConfigureAwait(false));
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50))
                    .ConfigureAwait(false);
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                await Task.Delay(50).ConfigureAwait(false);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(await successfulWaitTask.ConfigureAwait(false));
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50))
                    .ConfigureAwait(false);
            }

            [Fact]
            public async Task ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask1 = target.WaitOneAsync(cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var successfulWaitTask2 = target.WaitOneAsync(cancelSource.Token);

                // Add a small delay to make sure the second task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
                Assert.True(successfulWaitTask1.IsCompleted);
                Assert.True(await successfulWaitTask1.ConfigureAwait(false));
                Assert.False(successfulWaitTask2.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
                Assert.True(successfulWaitTask2.IsCompleted);
                Assert.True(await successfulWaitTask2.ConfigureAwait(false));
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50))
                    .ConfigureAwait(false);
            }
        }

        public class WaitOneAsync_Int32_CancellationToken
        {
            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var waitTask = target.WaitOneAsync(250, CancellationToken.None);

                await Task.Delay(200).ConfigureAwait(false);
                Assert.False(waitTask.IsCompleted);

                Assert.False(await waitTask.WithTimeout(100).ConfigureAwait(false));
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                await Task.Delay(250).ConfigureAwait(false);
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

                Assert.False(await waitTask.ConfigureAwait(false));
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var cancelSource = new CancellationTokenSource();
                var waitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                await Task.Delay(TimeSpan.FromMilliseconds(250)).ConfigureAwait(false);
                Assert.False(waitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask.WithTimeout(50))
                    .ConfigureAwait(false);
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(await successfulWaitTask.ConfigureAwait(false));
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50))
                    .ConfigureAwait(false);
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                await Task.Delay(50).ConfigureAwait(false);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(await successfulWaitTask.ConfigureAwait(false));
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50))
                    .ConfigureAwait(false);
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                await Task.Delay(50).ConfigureAwait(false);
                Assert.False(successfulWaitTask.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                await Task.Delay(50).ConfigureAwait(false);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(await successfulWaitTask.ConfigureAwait(false));
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50))
                    .ConfigureAwait(false);
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                await Task.Delay(50).ConfigureAwait(false);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(await successfulWaitTask.ConfigureAwait(false));
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50))
                    .ConfigureAwait(false);
            }

            [Fact]
            public async Task ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask1 = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var successfulWaitTask2 = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                // Add a small delay to make sure the second task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
                Assert.True(successfulWaitTask1.IsCompleted);
                Assert.True(await successfulWaitTask1.ConfigureAwait(false));
                Assert.False(successfulWaitTask2.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
                Assert.True(successfulWaitTask2.IsCompleted);
                Assert.True(await successfulWaitTask2.ConfigureAwait(false));
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50))
                    .ConfigureAwait(false);
            }
        }

        public class WaitOneAsync_TimeSpan_CancellationToken
        {
            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var waitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250), CancellationToken.None);

                await Task.Delay(200).ConfigureAwait(false);
                Assert.False(waitTask.IsCompleted);

                Assert.False(await waitTask.WithTimeout(100).ConfigureAwait(false));
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                await Task.Delay(250).ConfigureAwait(false);
                Assert.False(waitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask.WithTimeout(50))
                    .ConfigureAwait(false);
            }

            [Fact]
            public async Task DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250), CancellationToken.None);

                Assert.False(await waitTask.ConfigureAwait(false));
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var cancelSource = new CancellationTokenSource();
                var waitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                await Task.Delay(TimeSpan.FromMilliseconds(250)).ConfigureAwait(false);
                Assert.False(waitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => waitTask.WithTimeout(50))
                    .ConfigureAwait(false);
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(await successfulWaitTask.ConfigureAwait(false));
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50))
                    .ConfigureAwait(false);
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                await Task.Delay(50).ConfigureAwait(false);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(await successfulWaitTask.ConfigureAwait(false));
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50))
                    .ConfigureAwait(false);
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                await Task.Delay(50).ConfigureAwait(false);
                Assert.False(successfulWaitTask.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                await Task.Delay(50).ConfigureAwait(false);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(await successfulWaitTask.ConfigureAwait(false));
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50))
                    .ConfigureAwait(false);
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                await Task.Delay(50).ConfigureAwait(false);
                Assert.True(successfulWaitTask.IsCompleted);
                Assert.True(await successfulWaitTask.ConfigureAwait(false));
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50))
                    .ConfigureAwait(false);
            }

            [Fact]
            public async Task ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask1 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var successfulWaitTask2 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                // Add a small delay to make sure the second task calls WaitOneAsync first
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
                Assert.True(successfulWaitTask1.IsCompleted);
                Assert.True(await successfulWaitTask1.ConfigureAwait(false));
                Assert.False(successfulWaitTask2.IsCompleted);
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                target.Set();

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
                Assert.True(successfulWaitTask2.IsCompleted);
                Assert.True(await successfulWaitTask2.ConfigureAwait(false));
                Assert.False(unsuccessfulWaitTask.IsCompleted);

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(() => unsuccessfulWaitTask.WithTimeout(50))
                    .ConfigureAwait(false);
            }
        }
    }
}