using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeTiger.Threading;
using Xunit;

namespace UnitTests.CodeTiger.Threading
{
    /// <summary>
    /// Contains unit tests for the <see cref="AsyncLock"/> class.
    /// </summary>
    public static class AsyncLockTests
    {
        [Collection("AsyncLock.Acquire collection")]
        public class Acquire
        {
            public Acquire(LargeThreadPoolFixture fixture)
            {
                _ = fixture;
            }

            [Fact]
            public void ReturnsImmediatelyToFirstCaller()
            {
                var target = new AsyncLock();

                using (var lockObject = target.Acquire()) { }
            }

            [Fact]
            public void ReturnsImmediatelyToSecondCallerIfFirstCallerHasAlreadyDisposedLockObject()
            {
                var target = new AsyncLock();

                using (var firstLockObject = target.Acquire()) { }
                using (var secondLockObject = target.Acquire()) { }
            }

            [Fact]
            public void DoesNotReturnIfFirstCallerHasNotDisposedLockObject()
            {
                var target = new AsyncLock();

                IDisposable? firstLockObject = null;
                Task<IDisposable>? secondLockObjectTask = null;

                try
                {
                    firstLockObject = target.Acquire();
                    Thread.Sleep(TimeSpan.FromMilliseconds(50));

                    secondLockObjectTask = Task.Factory.StartNew(() => target.Acquire(),
                        CancellationToken.None,
                        TaskCreationOptions.LongRunning,
                        TaskScheduler.Default);

                    Assert.False(secondLockObjectTask.Wait(100));
                }
                finally
                {
                    // Clean up any outstanding locks or tasks
                    firstLockObject?.Dispose();
                    secondLockObjectTask?.Result?.Dispose();
                }
            }

            [Fact]
            public void ReturnsToSecondCallerWhenFirstCallerDisposesLockObject()
            {
                var target = new AsyncLock();

                IDisposable? firstLockObject = null;
                Task<IDisposable>? secondLockObjectTask = null;

                try
                {
                    firstLockObject = target.Acquire();

                    Thread.Sleep(TimeSpan.FromMilliseconds(50));

                    secondLockObjectTask = Task.Factory.StartNew(() => target.Acquire(),
                        CancellationToken.None,
                        TaskCreationOptions.LongRunning,
                        TaskScheduler.Default);

                    Thread.Sleep(TimeSpan.FromMilliseconds(100));

                    Assert.False(secondLockObjectTask.IsCompleted);

                    firstLockObject.Dispose();
                    firstLockObject = null;

                    Assert.True(secondLockObjectTask.Wait(50));
                }
                finally
                {
                    // Clean up any outstanding locks or tasks
                    firstLockObject?.Dispose();
                    secondLockObjectTask?.Result?.Dispose();
                }
            }

            [CollectionDefinition("AsyncLock.Acquire collection")]
            public class LargeThreadPoolCollection : ICollectionFixture<LargeThreadPoolFixture>
            {
            }
        }

        [Collection("AsyncLock.Acquire_CancellationToken collection")]
        public class Acquire_CancellationToken
        {
            public Acquire_CancellationToken(LargeThreadPoolFixture fixture)
            {
                _ = fixture;
            }

            [Fact]
            public void ReturnsImmediatelyToFirstCaller()
            {
                var target = new AsyncLock();

                using (var lockObject = target.Acquire(CancellationToken.None)) { }
            }

            [Fact]
            public void ReturnsImmediatelyToSecondCallerIfFirstCallerHasAlreadyDisposedLockObject()
            {
                var target = new AsyncLock();

                using (var firstLockObject = target.Acquire(CancellationToken.None)) { }
                using (var secondLockObject = target.Acquire(CancellationToken.None)) { }
            }

            [Fact]
            public void DoesNotReturnIfFirstCallerHasNotDisposedLockObject()
            {
                var target = new AsyncLock();

                IDisposable? firstLockObject = null;
                Task<IDisposable>? secondLockObjectTask = null;

                try
                {
                    firstLockObject = target.Acquire(CancellationToken.None);

                    Thread.Sleep(TimeSpan.FromMilliseconds(50));

                    secondLockObjectTask = Task.Factory.StartNew(() => target.Acquire(CancellationToken.None),
                        CancellationToken.None,
                        TaskCreationOptions.LongRunning,
                        TaskScheduler.Default);

                    Assert.False(secondLockObjectTask.Wait(100));
                }
                finally
                {
                    // Clean up any outstanding locks or tasks
                    firstLockObject?.Dispose();
                    secondLockObjectTask?.Result?.Dispose();
                }
            }

            [Fact]
            public void ReturnsToSecondCallerWhenFirstCallerDisposesLockObject()
            {
                var target = new AsyncLock();

                IDisposable? firstLockObject = null;
                Task<IDisposable>? secondLockObjectTask = null;

                try
                {
                    firstLockObject = target.Acquire(CancellationToken.None);

                    Thread.Sleep(TimeSpan.FromMilliseconds(50));

                    secondLockObjectTask = Task.Factory.StartNew(() => target.Acquire(CancellationToken.None),
                        CancellationToken.None,
                        TaskCreationOptions.LongRunning,
                        TaskScheduler.Default);

                    Thread.Sleep(TimeSpan.FromMilliseconds(100));

                    Assert.False(secondLockObjectTask.IsCompleted);

                    firstLockObject.Dispose();
                    firstLockObject = null;

                    Assert.True(secondLockObjectTask.Wait(50));
                }
                finally
                {
                    // Clean up any outstanding locks or tasks
                    firstLockObject?.Dispose();
                    secondLockObjectTask?.Result?.Dispose();
                }
            }

            [Fact]
            public void ThrowsTaskCanceledExceptionWhenCancellationTokenIsSet()
            {
                var target = new AsyncLock();

                IDisposable? firstLockObject = null;
                Task<IDisposable>? secondLockObjectTask = null;

                try
                {
                    firstLockObject = target.Acquire(CancellationToken.None);

                    Thread.Sleep(TimeSpan.FromMilliseconds(50));

                    using (var cancellationTokenSource = new CancellationTokenSource())
                    {
                        secondLockObjectTask = Task.Factory.StartNew(
                            () => target.Acquire(cancellationTokenSource.Token),
                            CancellationToken.None,
                            TaskCreationOptions.LongRunning,
                            TaskScheduler.Default);

                        Thread.Sleep(TimeSpan.FromMilliseconds(50));

                        Assert.False(secondLockObjectTask.Wait(50));

                        cancellationTokenSource.Cancel();

                        var aggregateException = Assert.Throws<AggregateException>(
                            () => secondLockObjectTask.Wait(50));
                        Assert.Equal(typeof(TaskCanceledException),
                            aggregateException.Flatten().InnerExceptions.Single().GetType());
                        secondLockObjectTask = null;
                    }
                }
                finally
                {
                    // Clean up any outstanding locks or tasks
                    firstLockObject?.Dispose();
                    secondLockObjectTask?.Result?.Dispose();
                }
            }

            [CollectionDefinition("AsyncLock.Acquire_CancellationToken collection")]
            public class LargeThreadPoolCollection : ICollectionFixture<LargeThreadPoolFixture>
            {
            }
        }

        [Collection("AsyncLock.AcquireAsync collection")]
        public class AcquireAsync
        {
            public AcquireAsync(LargeThreadPoolFixture fixture)
            {
                _ = fixture;
            }

            [Fact]
            public async Task ReturnsCompletedTaskToFirstCaller()
            {
                var target = new AsyncLock();

                var lockObjectTask = target.AcquireAsync();

                Assert.True(lockObjectTask.IsCompleted);

                // Clean up any outstanding locks or tasks
                (await lockObjectTask.ConfigureAwait(false)).Dispose();
            }

            [Fact]
            public async Task ReturnsSameCompletedTaskToSecondCallerIfFirstCallerHasAlreadyDisposedLockObject()
            {
                var target = new AsyncLock();

                var firstLockObjectTask = target.AcquireAsync();

                Assert.True(firstLockObjectTask.IsCompleted);

                (await firstLockObjectTask.ConfigureAwait(false)).Dispose();

                var secondLockObjectTask = target.AcquireAsync();

                Assert.Same(firstLockObjectTask, secondLockObjectTask);

                // Clean up any outstanding locks or tasks
                (await secondLockObjectTask.ConfigureAwait(false)).Dispose();
            }

            [Fact]
            public async Task ReturnsUncompletedTaskToSecondCallerIfFirstCallerHasNotDisposedLockObject()
            {
                var target = new AsyncLock();

                var firstLockObjectTask = target.AcquireAsync();

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var secondLockObjectTask = target.AcquireAsync();

                Assert.NotSame(firstLockObjectTask, secondLockObjectTask);
                Assert.False(secondLockObjectTask.IsCompleted);

                // Clean up any outstanding locks or tasks
                (await firstLockObjectTask.ConfigureAwait(false)).Dispose();
                (await secondLockObjectTask.ConfigureAwait(false)).Dispose();
            }

            [Fact]
            public async Task ReturnsUncompletedTaskToSecondCallerThatCompletesWhenFirstCallerDisposesLockObject()
            {
                var target = new AsyncLock();

                var firstLockObjectTask = target.AcquireAsync();

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var secondLockObjectTask = target.AcquireAsync();
                Assert.NotSame(firstLockObjectTask, secondLockObjectTask);

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
                Assert.False(secondLockObjectTask.IsCompleted);

                // Clean up any outstanding locks or tasks
                (await firstLockObjectTask.ConfigureAwait(false)).Dispose();
                (await secondLockObjectTask.ConfigureAwait(false)).Dispose();
            }

            [CollectionDefinition("AsyncLock.AcquireAsync collection")]
            public class LargeThreadPoolCollection : ICollectionFixture<LargeThreadPoolFixture>
            {
            }
        }

        [Collection("AsyncLock.AcquireAsync_CancellationToken collection")]
        public class AcquireAsync_CancellationToken
        {
            public AcquireAsync_CancellationToken(LargeThreadPoolFixture fixture)
            {
                _ = fixture;
            }

            [Fact]
            public async Task ReturnsCompletedTaskToFirstCaller()
            {
                var target = new AsyncLock();

                var lockObjectTask = target.AcquireAsync(CancellationToken.None);

                Assert.True(lockObjectTask.IsCompleted);

                // Clean up any outstanding locks or tasks
                (await lockObjectTask.ConfigureAwait(false)).Dispose();
            }

            [Fact]
            public async Task ReturnsSameCompletedTaskToSecondCallerIfFirstCallerHasAlreadyDisposedLockObject()
            {
                var target = new AsyncLock();

                var firstLockObjectTask = target.AcquireAsync(CancellationToken.None);

                Assert.True(firstLockObjectTask.IsCompleted);

                (await firstLockObjectTask.ConfigureAwait(false)).Dispose();

                var secondLockObjectTask = target.AcquireAsync(CancellationToken.None);

                Assert.Same(firstLockObjectTask, secondLockObjectTask);

                // Clean up any outstanding locks or tasks
                (await secondLockObjectTask.ConfigureAwait(false)).Dispose();
            }

            [Fact]
            public async Task ReturnsUncompletedTaskToSecondCallerIfFirstCallerHasNotDisposedLockObject()
            {
                var target = new AsyncLock();

                var firstLockObjectTask = target.AcquireAsync(CancellationToken.None);

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var secondLockObjectTask = target.AcquireAsync(CancellationToken.None);

                Assert.NotSame(firstLockObjectTask, secondLockObjectTask);
                Assert.False(secondLockObjectTask.IsCompleted);

                // Clean up any outstanding locks or tasks
                (await firstLockObjectTask.ConfigureAwait(false)).Dispose();
                (await secondLockObjectTask.ConfigureAwait(false)).Dispose();
            }

            [Fact]
            public async Task ReturnsUncompletedTaskToSecondCallerThatCompletesWhenFirstCallerDisposesLockObject()
            {
                var target = new AsyncLock();

                var firstLockObjectTask = target.AcquireAsync(CancellationToken.None);

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                var secondLockObjectTask = target.AcquireAsync(CancellationToken.None);
                Assert.NotSame(firstLockObjectTask, secondLockObjectTask);

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
                Assert.False(secondLockObjectTask.IsCompleted);

                // Clean up any outstanding locks or tasks
                (await firstLockObjectTask.ConfigureAwait(false)).Dispose();
                (await secondLockObjectTask.ConfigureAwait(false)).Dispose();
            }

            [Fact]
            public async Task ReturnsUncompletedTaskToSecondCallerThatCancelsWhenCancellationTokenIsSet()
            {
                var target = new AsyncLock();

                var firstLockObjectTask = target.AcquireAsync(CancellationToken.None);

                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                using (var cancellationTokenSource = new CancellationTokenSource())
                {
                    var secondLockObjectTask = target.AcquireAsync(cancellationTokenSource.Token);

                    await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);

                    Assert.NotSame(firstLockObjectTask, secondLockObjectTask);
                    Assert.False(secondLockObjectTask.IsCompleted);

                    cancellationTokenSource.Cancel();

                    await Assert.ThrowsAsync<TaskCanceledException>(() => secondLockObjectTask)
                        .ConfigureAwait(false);

                    // Clean up any outstanding locks or tasks
                    (await firstLockObjectTask.ConfigureAwait(false)).Dispose();
                }
            }

            [CollectionDefinition("AsyncLock.AcquireAsync_CancellationToken collection")]
            public class LargeThreadPoolCollection : ICollectionFixture<LargeThreadPoolFixture>
            {
            }
        }
    }
}
