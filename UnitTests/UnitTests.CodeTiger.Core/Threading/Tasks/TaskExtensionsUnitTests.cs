using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeTiger.Threading.Tasks;
using Xunit;
using TaskExtensions = CodeTiger.Threading.Tasks.TaskExtensions;

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
            public void ReturnsOriginalTaskWhenTimeoutIsInfinite()
            {
                var taskWithTimeout = _completedTask.WithTimeout(Timeout.Infinite);

                Assert.Same(_completedTask, taskWithTimeout);
            }

            [Fact]
            public void ReturnsOriginalTaskWhenTimeoutIsZeroAndOriginalTaskIsSynchronous()
            {
                var taskWithTimeout = _completedTask.WithTimeout(0);

                Assert.Same(_completedTask, taskWithTimeout);
            }

            [Fact]
            public void ReturnsDifferentTaskWhenTimeoutIsZeroAndOriginalTaskIsAsynchronous()
            {
                var task = Task.Delay(50);
                var taskWithTimeout = task.WithTimeout(0);

                Assert.NotSame(task, taskWithTimeout);
            }

            [Fact]
            public async Task ReturnsTaskWhichThrowsTimeoutExceptionWhenTimeoutIsLessThanOriginalTaskDuration()
            {
                var task = Task.Delay(200);
                var taskWithTimeout = task.WithTimeout(100);

                await Assert.ThrowsAsync<TimeoutException>(() => taskWithTimeout);
            }

            [Fact]
            public void ReturnsTaskWhichCompletesNormallyWhenTimeoutIsGreaterThanTaskCompletionTime()
            {
                var task = Task.Delay(100);
                var taskWithTimeout = task.WithTimeout(200);

                Assert.True(taskWithTimeout.Wait(150));
            }

            [Fact]
            public void ReturnsTaskWhichThrowsTaskCanceledExceptionWhenOriginalTaskIsCanceled()
            {
                var taskSource = new TaskCompletionSource<bool>();
                var taskWithTimeout = taskSource.Task.WithTimeout(500);

                taskSource.SetCanceled();

                var aggregateException = Assert.Throws<AggregateException>(() => taskWithTimeout.Wait(50));
                Assert.Equal(typeof(TaskCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }
        }
        
        public class WithTimeout_TimeSpan
        {
            [Fact]
            public void ReturnsOriginalTaskWhenTimeoutIsInfiniteTimeSpan()
            {
                var taskWithTimeout = _completedTask.WithTimeout(Timeout.InfiniteTimeSpan);

                Assert.Same(_completedTask, taskWithTimeout);
            }

            [Fact]
            public void ReturnsOriginalTaskWhenTimeoutIsZeroAndOriginalTaskIsSynchronous()
            {
                var taskWithTimeout = _completedTask.WithTimeout(TimeSpan.Zero);

                Assert.Same(_completedTask, taskWithTimeout);
            }

            [Fact]
            public void ReturnsDifferentTaskWhenTimeoutIsZeroAndOriginalTaskIsAsynchronous()
            {
                var task = Task.Delay(50);
                var taskWithTimeout = task.WithTimeout(TimeSpan.Zero);

                Assert.NotSame(task, taskWithTimeout);
            }

            [Fact]
            public async Task ReturnsTaskWhichThrowsTimeoutExceptionWhenTimeoutIsLessThanOriginalTaskDuration()
            {
                var task = Task.Delay(200);
                var taskWithTimeout = task.WithTimeout(TimeSpan.FromMilliseconds(100));

                await Assert.ThrowsAsync<TimeoutException>(() => taskWithTimeout);
            }

            [Fact]
            public void ReturnsTaskWhichCompletesNormallyWhenTimeoutIsGreaterThanTaskCompletionTime()
            {
                var task = Task.Delay(100);
                var taskWithTimeout = task.WithTimeout(TimeSpan.FromMilliseconds(200));

                Assert.True(taskWithTimeout.Wait(150));
            }

            [Fact]
            public void ReturnsTaskWhichThrowsTaskCanceledExceptionWhenOriginalTaskIsCanceled()
            {
                var taskSource = new TaskCompletionSource<bool>();
                var taskWithTimeout = taskSource.Task.WithTimeout(TimeSpan.FromMilliseconds(500));

                taskSource.SetCanceled();

                var aggregateException = Assert.Throws<AggregateException>(() => taskWithTimeout.Wait(50));
                Assert.Equal(typeof(TaskCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }
        }

        public class Wait_TimeSpan_CancellationToken
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenTaskIsNull()
            {
                Task task = null;

                Assert.Throws<ArgumentNullException>(
                    () => TaskExtensions.Wait(task, Timeout.InfiniteTimeSpan, CancellationToken.None));
            }

            [Fact]
            public void ReturnsImmediatelyWhenTaskIsCompleted()
            {
                var task = Task.FromResult(true);

                var target = Task.Run(
                    () => TaskExtensions.Wait(task, Timeout.InfiniteTimeSpan, CancellationToken.None));

                Task.Delay(1).Wait();

                Assert.True(target.IsCompleted);
                Assert.True(target.Result);
            }

            [Fact]
            public void ReturnsWhenTaskCompletesWhenTaskCompletesBeforeTimeout()
            {
                var task = Task.Delay(50);

                var target = Task.Run(
                    () => TaskExtensions.Wait(task, TimeSpan.FromMilliseconds(100), CancellationToken.None));

                Task.Delay(80).Wait();

                Assert.True(target.IsCompleted);
                Assert.True(target.Result);
            }

            [Fact]
            public void ThrowsOperationCanceledExceptionImmediatelyWhenCancellationTokenIsInitiallySet()
            {
                var cancellationTokenSource = new CancellationTokenSource();

                var task = Task.Delay(250);

                var target = Task.Run(
                    () => TaskExtensions.Wait(task, Timeout.InfiniteTimeSpan, cancellationTokenSource.Token));

                Task.Delay(150).Wait();

                Assert.False(target.IsCompleted);

                cancellationTokenSource.Cancel();

                Task.Delay(1).Wait();

                Assert.True(target.IsCompleted);
                var aggregateException = Assert.Throws<AggregateException>(() => target.Wait());
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ThrowsOperationCanceledExceptionWhenCancellationTokenIsSet()
            {
                var task = Task.Delay(50);

                var target = Task.Run(
                    () => TaskExtensions.Wait(task, Timeout.InfiniteTimeSpan, new CancellationToken(true)));

                Task.Delay(1).Wait();

                Assert.True(target.IsCompleted);
                var aggregateException = Assert.Throws<AggregateException>(() => target.Wait());
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());
            }

            [Fact]
            public void ReturnsFalseImmediatelyWhenTimeoutIsZero()
            {
                var task = Task.Delay(50);

                var target = Task.Run(
                    () => TaskExtensions.Wait(task, TimeSpan.Zero, CancellationToken.None));

                Task.Delay(1).Wait();

                Assert.True(target.IsCompleted);
                Assert.False(target.Result);
            }

            [Fact]
            public void ReturnsFalseWhenTimeoutExpires()
            {
                var task = Task.Delay(50);

                var target = Task.Run(
                    () => TaskExtensions.Wait(task, TimeSpan.FromMilliseconds(30), CancellationToken.None));

                Task.Delay(40).Wait();

                Assert.True(target.IsCompleted);
                Assert.False(target.Result);
            }
        }
    }
}