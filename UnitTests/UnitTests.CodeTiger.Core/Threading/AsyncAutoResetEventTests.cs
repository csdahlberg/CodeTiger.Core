using System;
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
                
                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(250)));
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var successfulWaitTask = Task.Run(() => target.WaitOne());

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);

                var successfulWaitTask = Task.Run(() => target.WaitOne());

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                Assert.False(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(250)));

                target.Set();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
            }

            [Fact]
            public void ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var successfulWaitTask = Task.Run(() => target.WaitOne());

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
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

                Assert.True(successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));

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

                var waitTask = Task.Run(() => target.WaitOne(250));

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(150)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = Task.Run(() => target.WaitOne(250));

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(150)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask = Task.Run(() => target.WaitOne(250));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(250));

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
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

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);

                var successfulWaitTask = Task.Run(() => target.WaitOne(250));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(250));

                Assert.False(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                
                target.Set();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
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

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
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

                Assert.True(successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
            }
        }

        public class WaitOne_TimeSpan
        {
            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var waitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(150)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(150)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
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

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);

                var successfulWaitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(TimeSpan.FromMilliseconds(250)));

                Assert.False(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
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

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
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

                Assert.True(successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
            }
        }

        public class WaitOne_CancellationToken
        {
            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(250)));

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                Assert.False(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                
                target.Set();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(cancelSource.Token));

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOne()
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

                Assert.True(successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
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
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(250)));

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = Task.Run(() => target.WaitOne(250, CancellationToken.None));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var cancelSource = new CancellationTokenSource();
                var waitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(250)));

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                Assert.False(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                
                target.Set();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(() => target.WaitOne(Timeout.Infinite, cancelSource.Token));

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOne()
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

                Assert.True(successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
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

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(150)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask = Task.Run(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(250)));

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = Task.Run(
                    () => target.WaitOne(TimeSpan.FromMilliseconds(250), CancellationToken.None));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var cancelSource = new CancellationTokenSource();
                var waitTask = Task.Run(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(250)));

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOne()
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

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOne()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = Task.Run(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                // Add a small delay to make sure the first task calls WaitOne first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = Task.Run(
                    () => target.WaitOne(Timeout.InfiniteTimeSpan, cancelSource.Token));

                Assert.False(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                
                target.Set();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOne()
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

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOne()
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

                Assert.True(successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }
        }

        public class WaitOneAsync
        {
            [Fact]
            public void ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask = target.WaitOneAsync();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(250)));
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var successfulWaitTask = target.WaitOneAsync();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);

                var successfulWaitTask = target.WaitOneAsync();

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                Assert.False(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(250)));

                target.Set();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
            }

            [Fact]
            public void ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
            }

            [Fact]
            public void ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask1 = target.WaitOneAsync();

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var successfulWaitTask2 = target.WaitOneAsync();

                // Add a small delay to make sure the second task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                Assert.True(successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
            }
        }

        public class WaitOneAsync_Int32
        {
            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var waitTask = target.WaitOneAsync(250);

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(150)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(10000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = target.WaitOneAsync(250);

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(150)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask = target.WaitOneAsync(250);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(250);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var successfulWaitTask = target.WaitOneAsync(250);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(250);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);

                var successfulWaitTask = target.WaitOneAsync(250);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(250);

                Assert.False(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var successfulWaitTask = target.WaitOneAsync(250);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(250);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask1 = target.WaitOneAsync(250);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var successfulWaitTask2 = target.WaitOneAsync(250);

                // Add a small delay to make sure the second task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(250);

                Assert.True(successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
            }
        }

        public class WaitOneAsync_TimeSpan
        {
            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var waitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(150)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(150)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var successfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);

                var successfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                Assert.False(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var successfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public void ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);

                var successfulWaitTask1 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var successfulWaitTask2 = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                // Add a small delay to make sure the second task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250));

                Assert.True(successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(unsuccessfulWaitTask.GetAwaiter().GetResult());
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

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(250)));

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                Assert.False(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask1 = target.WaitOneAsync(cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var successfulWaitTask2 = target.WaitOneAsync(cancelSource.Token);

                // Add a small delay to make sure the second task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(cancelSource.Token);

                Assert.True(successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }
        }

        public class WaitOneAsync_Int32_CancellationToken
        {
            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var waitTask = target.WaitOneAsync(250, CancellationToken.None);

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(250)));

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = target.WaitOneAsync(250, CancellationToken.None);

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var cancelSource = new CancellationTokenSource();
                var waitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(250)));

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                Assert.False(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask1 = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var successfulWaitTask2 = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                // Add a small delay to make sure the second task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.Infinite, cancelSource.Token);

                Assert.True(successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }
        }

        public class WaitOneAsync_TimeSpan_CancellationToken
        {
            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var waitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250), CancellationToken.None);

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(150)));

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsAlwaysUnset()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var waitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(250)));

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public void DoesNotReturnUntilTimeoutElapsesWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var waitTask = target.WaitOneAsync(TimeSpan.FromMilliseconds(250), CancellationToken.None);

                // Wait for the task to complete so aren't any lingering background threads.
                Assert.True(waitTask.Wait(TimeSpan.FromMilliseconds(1000)));
                Assert.False(waitTask.GetAwaiter().GetResult());
            }

            [Fact]
            public async Task DoesNotReturnUntilCancelTokenIsSetWhenEventIsInitiallySetThenReset()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Reset();

                var cancelSource = new CancellationTokenSource();
                var waitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                Assert.False(waitTask.Wait(TimeSpan.FromMilliseconds(250)));

                cancelSource.Cancel();

                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await waitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsInitiallySet()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsNotInitiallySetThenSetAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(false);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                Assert.False(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsOnceWhenEventIsSetTwiceBeforeCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);
                target.Set();

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                Assert.True(successfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }

            [Fact]
            public async Task ReturnsTwiceWhenEventIsSetBeforeAndAfterCallingWaitOneAsync()
            {
                var target = new AsyncAutoResetEvent(true);

                var cancelSource = new CancellationTokenSource();
                var successfulWaitTask1 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                // Add a small delay to make sure the first task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var successfulWaitTask2 = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                // Add a small delay to make sure the second task calls WaitOneAsync first
                Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();

                var unsuccessfulWaitTask = target.WaitOneAsync(Timeout.InfiniteTimeSpan, cancelSource.Token);

                Assert.True(successfulWaitTask1.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                target.Set();

                Assert.True(successfulWaitTask2.Wait(TimeSpan.FromMilliseconds(50)));
                Assert.False(unsuccessfulWaitTask.Wait(TimeSpan.FromMilliseconds(50)));

                cancelSource.Cancel();

                // Wait for the canceled task to complete so aren't any lingering background threads.
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await unsuccessfulWaitTask.WithTimeout(TimeSpan.FromMilliseconds(1000)));
            }
        }
    }
}