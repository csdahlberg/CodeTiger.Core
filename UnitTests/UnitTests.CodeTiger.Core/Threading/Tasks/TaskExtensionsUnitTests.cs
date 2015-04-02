using System;
using System.Threading;
using System.Threading.Tasks;
using CodeTiger.Threading.Tasks;
using Xunit;

namespace UnitTests.CodeTiger.Threading.Tasks
{
    /// <summary>
    /// Contains unit tests for the <see cref="TaskExtensions"/> class.
    /// </summary>
    public class TaskExtensionsUnitTests
    {
        private static readonly Task _completedTask = Task.FromResult(84);

        public class WithTimeout_Int32
        {
            [Fact]
            public void ReturnsSameTaskWhenTimeoutIsInfinite()
            {
                var taskWithTimeout = _completedTask.WithTimeout(Timeout.Infinite);

                Assert.Same(_completedTask, taskWithTimeout);
            }

            [Fact]
            public void ReturnsSameTaskWhenTimeoutIsZeroAndTaskIsSynchronous()
            {
                var taskWithTimeout = _completedTask.WithTimeout(0);

                Assert.Same(_completedTask, taskWithTimeout);
            }

            [Fact]
            public async Task ReturnsDifferentTaskWhenTimeoutIsZeroAndTaskIsAsynchronous()
            {
                var task = Task.Delay(250);
                var taskWithTimeout = task.WithTimeout(0);

                Assert.NotSame(task, taskWithTimeout);

                await Assert.ThrowsAsync<TimeoutException>(async () => await taskWithTimeout.ConfigureAwait(false))
                    .ConfigureAwait(false);
                await task.ConfigureAwait(false);
            }

            [Fact]
            public async Task ThrowsTimeoutExceptionWhenTimeoutIsLessThanTaskCompletionTime()
            {
                var task = Task.Delay(250);
                var taskWithTimeout = task.WithTimeout(200);

                Assert.NotSame(task, taskWithTimeout);

                await Assert.ThrowsAsync<TimeoutException>(async () => await taskWithTimeout)
                    .ConfigureAwait(false);
                await task.ConfigureAwait(false);
            }

            [Fact]
            public async Task DoesNotThrowExceptionWhenTimeoutIsGreaterThanTaskCompletionTime()
            {
                var task = Task.Delay(250);
                var taskWithTimeout = task.WithTimeout(500);

                await taskWithTimeout.ConfigureAwait(false);
            }

            [Fact]
            public async Task ThrowsTaskCanceledExceptionWhenOriginalTaskIsCanceled()
            {
                var taskSource = new TaskCompletionSource<bool>();
                var taskWithTimeout = taskSource.Task.WithTimeout(500);

                taskSource.SetCanceled();

                await Assert.ThrowsAsync<TaskCanceledException>(async () => await taskWithTimeout)
                    .ConfigureAwait(false);
            }
        }
        
        public class WithTimeout_TimeSpan
        {
            [Fact]
            public void ReturnsSameTaskWhenTimeoutIsInfiniteTimeSpan()
            {
                var taskWithTimeout = _completedTask.WithTimeout(Timeout.InfiniteTimeSpan);

                Assert.Same(_completedTask, taskWithTimeout);
            }

            [Fact]
            public void ReturnsSameTaskWhenTimeoutIsZeroAndTaskIsSynchronous()
            {
                var taskWithTimeout = _completedTask.WithTimeout(TimeSpan.Zero);

                Assert.Same(_completedTask, taskWithTimeout);
            }

            [Fact]
            public async Task ReturnsDifferentTaskWhenTimeoutIsZeroAndTaskIsAsynchronous()
            {
                var task = Task.Delay(500);
                var taskWithTimeout = task.WithTimeout(TimeSpan.Zero);

                Assert.NotSame(task, taskWithTimeout);

                Task.Delay(50).Wait();

                await Assert.ThrowsAsync<TimeoutException>(async () => await taskWithTimeout)
                    .ConfigureAwait(false);
            }

            [Fact]
            public async Task ThrowsTimeoutExceptionWhenTimeoutIsLessThanTaskCompletionTime()
            {
                var task = Task.Delay(500);
                var taskWithTimeout = task.WithTimeout(TimeSpan.FromMilliseconds(200));

                Assert.NotSame(task, taskWithTimeout);
                await Assert.ThrowsAsync<TimeoutException>(async () => await taskWithTimeout.ConfigureAwait(false))
                    .ConfigureAwait(false);
            }

            [Fact]
            public async Task DoesNotThrowExceptionWhenTimeoutIsGreaterThanTaskCompletionTime()
            {
                var task = Task.Delay(250);
                var taskWithTimeout = task.WithTimeout(TimeSpan.FromMilliseconds(500));

                Assert.NotSame(task, taskWithTimeout);
                await taskWithTimeout.ConfigureAwait(false);
            }

            [Fact]
            public async Task ThrowsTaskCanceledExceptionWhenOriginalTaskIsCanceled()
            {
                var taskSource = new TaskCompletionSource<bool>();
                var taskWithTimeout = taskSource.Task.WithTimeout(TimeSpan.FromMilliseconds(500));

                taskSource.SetCanceled();

                await Assert.ThrowsAsync<TaskCanceledException>(async () => await taskWithTimeout)
                    .ConfigureAwait(false);
            }
        }
    }
}