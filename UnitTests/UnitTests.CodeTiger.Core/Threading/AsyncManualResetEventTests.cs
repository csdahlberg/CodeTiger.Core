using System;
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

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(250)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(250)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(250)));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne());
                var waitTask2 = Task.Run(() => target.WaitOne());
                var waitTask3 = Task.Run(() => target.WaitOne());

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(250)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(250)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(250)));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask1 = Task.Run(() => target.WaitOne());
                var waitTask2 = Task.Run(() => target.WaitOne());
                var waitTask3 = Task.Run(() => target.WaitOne());

                Assert.False(waitTask1.Wait(TimeSpan.FromMilliseconds(250)));
                Assert.False(waitTask2.Wait(TimeSpan.FromMilliseconds(250)));
                Assert.False(waitTask3.Wait(TimeSpan.FromMilliseconds(250)));

                target.Set();

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(true);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne());
                var waitTask2 = Task.Run(() => target.WaitOne());
                var waitTask3 = Task.Run(() => target.WaitOne());

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));
            }
        }

        public class WaitOne_Int32
        {
            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask = Task.Run(() => target.WaitOne(250));

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(150)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncManualResetEvent(true);
                target.Reset();

                var waitTask = Task.Run(() => target.WaitOne(250));

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(150)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncManualResetEvent(true);

                var waitTask1 = Task.Run(() => target.WaitOne(250));
                var waitTask2 = Task.Run(() => target.WaitOne(250));
                var waitTask3 = Task.Run(() => target.WaitOne(250));

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(250));
                var waitTask2 = Task.Run(() => target.WaitOne(250));
                var waitTask3 = Task.Run(() => target.WaitOne(250));

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask1 = Task.Run(() => target.WaitOne(250));
                var waitTask2 = Task.Run(() => target.WaitOne(250));
                var waitTask3 = Task.Run(() => target.WaitOne(250));

                Assert.False(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(true);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(250));
                var waitTask2 = Task.Run(() => target.WaitOne(250));
                var waitTask3 = Task.Run(() => target.WaitOne(250));

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));
            }
        }

        public class WaitOne_TimeSpan
        {
            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(150)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncManualResetEvent(true);
                target.Reset();

                var waitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(150)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncManualResetEvent(true);

                var waitTask1 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));
                var waitTask2 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));
                var waitTask3 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));
                var waitTask2 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));
                var waitTask3 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask1 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));
                var waitTask2 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));
                var waitTask3 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.False(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(true);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));
                var waitTask2 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));
                var waitTask3 = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));
            }
        }

        public class WaitOne_CancellationToken
        {
            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(250)));

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask.WithTimeout(TimeSpan.FromMilliseconds(500))
                        .ConfigureAwait(false));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncManualResetEvent(true);

                var waitTask1 = Task.Run(() => target.WaitOne(CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(CancellationToken.None));

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(250)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(250)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(250)));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(CancellationToken.None));

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(250)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(250)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(250)));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask1 = Task.Run(() => target.WaitOne(CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(CancellationToken.None));

                target.Set();

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(250)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(250)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(250)));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(true);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(CancellationToken.None));

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(250)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(250)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(250)));
            }
        }

        public class WaitOne_Int32_CancellationToken
        {
            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask = Task.Run(() => target.WaitOne(250, CancellationToken.None));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(500)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                Assert.False(waitTask1.Wait(TimeSpan.FromMilliseconds(250)));
                Assert.False(waitTask2.Wait(TimeSpan.FromMilliseconds(250)));
                Assert.False(waitTask3.Wait(TimeSpan.FromMilliseconds(250)));

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask1.WithTimeout(TimeSpan.FromMilliseconds(500)).ConfigureAwait(false));
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask2.WithTimeout(TimeSpan.FromMilliseconds(500)).ConfigureAwait(false));
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask3.WithTimeout(TimeSpan.FromMilliseconds(500)).ConfigureAwait(false));
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncManualResetEvent(true);
                target.Reset();

                var waitTask1 = Task.Run(() => target.WaitOne(250, CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(250, CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(250, CancellationToken.None));

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(500)));
                Assert.False(waitTask1.GetAwaiter().GetResult());
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(500)));
                Assert.False(waitTask2.GetAwaiter().GetResult());
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(500)));
                Assert.False(waitTask3.GetAwaiter().GetResult());
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncManualResetEvent(true);
                target.Reset();

                var cancelSource = new CancellationTokenSource();
                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                Assert.False(waitTask1.Wait(TimeSpan.FromMilliseconds(250)));

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask1.WithTimeout(TimeSpan.FromMilliseconds(500)).ConfigureAwait(false));
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask2.WithTimeout(TimeSpan.FromMilliseconds(500)).ConfigureAwait(false));
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask3.WithTimeout(TimeSpan.FromMilliseconds(500)).ConfigureAwait(false));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncManualResetEvent(true);

                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));

                target.Set();

                Task.Delay(50).GetAwaiter().GetResult();

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(true);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.Infinite, CancellationToken.None));

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));
            }
        }

        public class WaitOne_TimeSpan_CancellationToken
        {
            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250),
                    CancellationToken.None));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(500)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncManualResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                Assert.False(waitTask1.Wait(TimeSpan.FromMilliseconds(250)));
                Assert.False(waitTask2.Wait(TimeSpan.FromMilliseconds(250)));
                Assert.False(waitTask3.Wait(TimeSpan.FromMilliseconds(250)));

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask1.WithTimeout(TimeSpan.FromMilliseconds(500)).ConfigureAwait(false));
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask2.WithTimeout(TimeSpan.FromMilliseconds(500)).ConfigureAwait(false));
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask3.WithTimeout(TimeSpan.FromMilliseconds(500)).ConfigureAwait(false));
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

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(500)));
                Assert.False(waitTask1.GetAwaiter().GetResult());
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(500)));
                Assert.False(waitTask2.GetAwaiter().GetResult());
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(500)));
                Assert.False(waitTask3.GetAwaiter().GetResult());
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncManualResetEvent(true);
                target.Reset();

                var cancelSource = new CancellationTokenSource();
                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                Assert.False(waitTask1.Wait(TimeSpan.FromMilliseconds(250)));

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask1.WithTimeout(TimeSpan.FromMilliseconds(500)).ConfigureAwait(false));
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask2.WithTimeout(TimeSpan.FromMilliseconds(500)).ConfigureAwait(false));
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask3.WithTimeout(TimeSpan.FromMilliseconds(500)).ConfigureAwait(false));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncManualResetEvent(true);

                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(false);

                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));

                target.Set();

                Task.Delay(50).GetAwaiter().GetResult();

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));
            }

            [Fact]
            public void ReturnsMoreThanOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncManualResetEvent(true);
                target.Set();

                var waitTask1 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));
                var waitTask2 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));
                var waitTask3 = Task.Run(() => target.WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None));

                Assert.True(waitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.True(waitTask3.Wait(TimeSpan.FromMilliseconds(50)));
            }
        }
    }
}
