using System.Threading;
using System.Threading.Tasks;
using CodeTiger.Threading;
using Xunit;

namespace UnitTests.CodeTiger.Threading
{
    /// <summary>
    /// Contains unit tests for the <see cref="AsyncLock"/> class.
    /// </summary>
    public class AsyncLockTests
    {
        public class Acquire
        {
            [Fact]
            public void ReturnsImmediatelyToFirstCaller()
            {
                var target = new AsyncLock();

                var lockObjectTask = Task.Factory.StartNew(() => target.Acquire());

                Assert.True(lockObjectTask.Wait(50));
            }

            [Fact]
            public void ReturnsImmediatelyToSecondCallerIfFirstCallerHasAlreadyDisposedLockObject()
            {
                var target = new AsyncLock();

                var firstLockObjectTask = Task.Factory.StartNew(() => target.Acquire());
                firstLockObjectTask.GetAwaiter().GetResult().Dispose();

                var secondLockObjectTask = Task.Factory.StartNew(() => target.Acquire());

                Assert.True(secondLockObjectTask.Wait(50));
            }

            [Fact]
            public void DoesNotReturnIfFirstCallerHasNotDisposedLockObject()
            {
                var target = new AsyncLock();

                var firstLockObject = target.Acquire();

                Task.Delay(50).GetAwaiter().GetResult();

                var secondLockObjectTask = Task.Factory.StartNew(() => target.Acquire());

                Assert.False(secondLockObjectTask.Wait(100));
            }

            [Fact]
            public void ReturnsToSecondCallerWhenFirstCallerDisposesLockObject()
            {
                var target = new AsyncLock();

                var firstLockObject = target.Acquire();

                Task.Delay(50).GetAwaiter().GetResult();

                var secondLockObjectTask = Task.Factory.StartNew(() => target.Acquire());

                Task.Delay(100).GetAwaiter().GetResult();

                Assert.False(secondLockObjectTask.IsCompleted);

                firstLockObject.Dispose();

                Assert.True(secondLockObjectTask.Wait(50));
            }
        }

        public class Acquire_CancellationToken
        {
            [Fact]
            public void ReturnsImmediatelyToFirstCaller()
            {
                var target = new AsyncLock();

                var lockObjectTask = Task.Factory.StartNew(() => target.Acquire(CancellationToken.None));

                Assert.True(lockObjectTask.Wait(50));
            }

            [Fact]
            public void ReturnsImmediatelyToSecondCallerIfFirstCallerHasAlreadyDisposedLockObject()
            {
                var target = new AsyncLock();

                var firstLockObjectTask = Task.Factory.StartNew(() => target.Acquire(CancellationToken.None));
                firstLockObjectTask.GetAwaiter().GetResult().Dispose();

                var secondLockObjectTask = Task.Factory.StartNew(() => target.Acquire(CancellationToken.None));

                Assert.True(secondLockObjectTask.Wait(50));
            }

            [Fact]
            public void DoesNotReturnIfFirstCallerHasNotDisposedLockObject()
            {
                var target = new AsyncLock();

                var firstLockObject = target.Acquire(CancellationToken.None);

                Task.Delay(50).GetAwaiter().GetResult();

                var secondLockObjectTask = Task.Factory.StartNew(() => target.Acquire(CancellationToken.None));

                Assert.False(secondLockObjectTask.Wait(100));
            }

            [Fact]
            public void ReturnsToSecondCallerWhenFirstCallerDisposesLockObject()
            {
                var target = new AsyncLock();

                var firstLockObject = target.Acquire(CancellationToken.None);

                Task.Delay(50).GetAwaiter().GetResult();

                var secondLockObjectTask = Task.Factory.StartNew(() => target.Acquire(CancellationToken.None));

                Task.Delay(100).GetAwaiter().GetResult();

                Assert.False(secondLockObjectTask.IsCompleted);

                firstLockObject.Dispose();

                Assert.True(secondLockObjectTask.Wait(50));
            }

            [Fact]
            public void ThrowsTaskCanceledExceptionWhenCancellationTokenIsSet()
            {
                var target = new AsyncLock();

                var firstLockObject = target.Acquire(CancellationToken.None);

                Task.Delay(50).GetAwaiter().GetResult();

                var cancellationTokenSource = new CancellationTokenSource();
                var secondLockObjectTask = Task.Factory.StartNew(
                    () => target.Acquire(cancellationTokenSource.Token));

                Task.Delay(50).GetAwaiter().GetResult();

                Assert.False(secondLockObjectTask.Wait(50));

                cancellationTokenSource.Cancel();

                Assert.Throws<TaskCanceledException>(() => secondLockObjectTask.GetAwaiter().GetResult());
            }
        }

        public class AcquireAsync
        {
            [Fact]
            public void ReturnsCompletedTaskToFirstCaller()
            {
                var target = new AsyncLock();

                var lockObjectTask = target.AcquireAsync();

                Assert.True(lockObjectTask.IsCompleted);
            }

            [Fact]
            public void ReturnsSameCompletedTaskToSecondCallerIfFirstCallerHasAlreadyDisposedLockObject()
            {
                var target = new AsyncLock();

                var firstLockObjectTask = target.AcquireAsync();
                firstLockObjectTask.GetAwaiter().GetResult().Dispose();

                var secondLockObjectTask = target.AcquireAsync();

                Assert.Same(firstLockObjectTask, secondLockObjectTask);
            }

            [Fact]
            public void ReturnsUncompletedTaskToSecondCallerIfFirstCallerHasNotDisposedLockObject()
            {
                var target = new AsyncLock();

                var firstLockObjectTask = target.AcquireAsync();

                Task.Delay(50).GetAwaiter().GetResult();

                var secondLockObjectTask = target.AcquireAsync();

                Assert.NotSame(firstLockObjectTask, secondLockObjectTask);
                Assert.False(secondLockObjectTask.IsCompleted);
            }

            [Fact]
            public void ReturnsUncompletedTaskToSecondCallerThatCompletesWhenFirstCallerDisposesLockObject()
            {
                var target = new AsyncLock();

                var firstLockObjectTask = target.AcquireAsync();

                Task.Delay(50).GetAwaiter().GetResult();

                var secondLockObjectTask = target.AcquireAsync();

                Assert.NotSame(firstLockObjectTask, secondLockObjectTask);
                Assert.False(secondLockObjectTask.IsCompleted);

                firstLockObjectTask.GetAwaiter().GetResult().Dispose();

                Task.Delay(50).GetAwaiter().GetResult();

                Assert.True(secondLockObjectTask.IsCompleted);
            }
        }

        public class AcquireAsync_CancellationToken
        {
            [Fact]
            public void ReturnsCompletedTaskToFirstCaller()
            {
                var target = new AsyncLock();

                var lockObjectTask = target.AcquireAsync(CancellationToken.None);

                Assert.True(lockObjectTask.IsCompleted);
            }

            [Fact]
            public void ReturnsSameCompletedTaskToSecondCallerIfFirstCallerHasAlreadyDisposedLockObject()
            {
                var target = new AsyncLock();

                var firstLockObjectTask = target.AcquireAsync(CancellationToken.None);
                firstLockObjectTask.GetAwaiter().GetResult().Dispose();

                var secondLockObjectTask = target.AcquireAsync(CancellationToken.None);

                Assert.Same(firstLockObjectTask, secondLockObjectTask);
            }

            [Fact]
            public void ReturnsUncompletedTaskToSecondCallerIfFirstCallerHasNotDisposedLockObject()
            {
                var target = new AsyncLock();

                var firstLockObjectTask = target.AcquireAsync(CancellationToken.None);

                Task.Delay(50).GetAwaiter().GetResult();

                var secondLockObjectTask = target.AcquireAsync(CancellationToken.None);

                Assert.NotSame(firstLockObjectTask, secondLockObjectTask);
                Assert.False(secondLockObjectTask.IsCompleted);
            }

            [Fact]
            public void ReturnsUncompletedTaskToSecondCallerThatCompletesWhenFirstCallerDisposesLockObject()
            {
                var target = new AsyncLock();

                var firstLockObjectTask = target.AcquireAsync(CancellationToken.None);

                Task.Delay(50).GetAwaiter().GetResult();

                var secondLockObjectTask = target.AcquireAsync(CancellationToken.None);

                Assert.NotSame(firstLockObjectTask, secondLockObjectTask);
                Assert.False(secondLockObjectTask.IsCompleted);

                firstLockObjectTask.GetAwaiter().GetResult().Dispose();

                Task.Delay(50).GetAwaiter().GetResult();

                Assert.True(secondLockObjectTask.IsCompleted);
            }

            [Fact]
            public void ReturnsUncompletedTaskToSecondCallerThatCancelsWhenCancellationTokenIsSet()
            {
                var target = new AsyncLock();

                var firstLockObjectTask = target.AcquireAsync(CancellationToken.None);

                Task.Delay(50).GetAwaiter().GetResult();

                var cancellationTokenSource = new CancellationTokenSource();
                var secondLockObjectTask = target.AcquireAsync(cancellationTokenSource.Token);

                Task.Delay(50).GetAwaiter().GetResult();

                Assert.NotSame(firstLockObjectTask, secondLockObjectTask);
                Assert.False(secondLockObjectTask.Wait(50));

                cancellationTokenSource.Cancel();

                Assert.Throws<TaskCanceledException>(() => secondLockObjectTask.GetAwaiter().GetResult());
            }
        }
    }
}