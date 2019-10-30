using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeTiger.Threading.Tasks;
using Xunit;

namespace UnitTests.CodeTiger.Threading.Tasks
{
    /// <summary>
    /// Contains unit tests for the <see cref="TaskExtensions"/> class.
    /// </summary>
    public static class TaskExtensionsUnitTests
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

                // Wait for outstanding tasks to complete
                task.Wait();
            }

            [Fact]
            public void ReturnsTaskWhichThrowsTimeoutExceptionWhenTimeoutIsLessThanOriginalTaskDuration()
            {
                var task = Task.Delay(200);
                var taskWithTimeout = task.WithTimeout(50);

                var aggregateException = Assert.Throws<AggregateException>(() => taskWithTimeout.Wait());
                Assert.IsType<TimeoutException>(aggregateException.Flatten().InnerExceptions.Single());

                // Wait for outstanding tasks to complete
                task.Wait();
            }

            [Fact]
            public void ReturnsTaskWhichCompletesNormallyWhenTimeoutIsGreaterThanTaskCompletionTime()
            {
                var task = Task.Delay(50);
                var taskWithTimeout = task.WithTimeout(200);

                taskWithTimeout.Wait();

                // Wait for outstanding tasks to complete
                task.Wait();
            }

            [Fact]
            public void ReturnsTaskWhichThrowsTaskCanceledExceptionWhenOriginalTaskIsCanceled()
            {
                var taskSource = new TaskCompletionSource<bool>();
                var taskWithTimeout = taskSource.Task.WithTimeout(200);

                taskSource.SetCanceled();

                var aggregateException = Assert.Throws<AggregateException>(() => taskWithTimeout.Wait());
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

                // Wait for outstanding tasks to complete
                task.Wait();
            }

            [Fact]
            public void ReturnsTaskWhichThrowsTimeoutExceptionWhenTimeoutIsLessThanOriginalTaskDuration()
            {
                var task = Task.Delay(200);
                var taskWithTimeout = task.WithTimeout(TimeSpan.FromMilliseconds(50));

                var actual = Assert.Throws<AggregateException>(() => taskWithTimeout.Wait());
                Assert.IsType<TimeoutException>(actual.Flatten().InnerExceptions.Single());

                // Wait for outstanding tasks to complete
                task.Wait();
            }

            [Fact]
            public void ReturnsTaskWhichCompletesNormallyWhenTimeoutIsGreaterThanTaskCompletionTime()
            {
                var task = Task.Delay(50);
                var taskWithTimeout = task.WithTimeout(TimeSpan.FromMilliseconds(200));

                taskWithTimeout.Wait();

                // Wait for outstanding tasks to complete
                task.Wait();
            }

            [Fact]
            public void ReturnsTaskWhichThrowsTaskCanceledExceptionWhenOriginalTaskIsCanceled()
            {
                var taskSource = new TaskCompletionSource<bool>();
                var taskWithTimeout = taskSource.Task.WithTimeout(TimeSpan.FromMilliseconds(200));

                taskSource.SetCanceled();

                var aggregateException = Assert.Throws<AggregateException>(() => taskWithTimeout.Wait());
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
                    () => task.Wait(Timeout.InfiniteTimeSpan, CancellationToken.None));
            }

            [Fact]
            public void ReturnsImmediatelyWhenTaskIsCompleted()
            {
                var task = Task.FromResult(true);
                
                bool actual = task.Wait(Timeout.InfiniteTimeSpan, CancellationToken.None);

                Assert.True(actual);
            }

            [Fact]
            public void ReturnsWhenTaskCompletesWhenTaskCompletesBeforeTimeout()
            {
                var task = Task.Delay(50);
                
                bool actual = task.Wait(TimeSpan.FromMilliseconds(200), CancellationToken.None);

                Assert.True(actual);
            }

            [Fact]
            public void ThrowsOperationCanceledExceptionImmediatelyWhenCancellationTokenIsInitiallySet()
            {
                var task = Task.Delay(100);
                
                Assert.Throws<OperationCanceledException>(
                    () => task.Wait(Timeout.InfiniteTimeSpan, new CancellationToken(true)));

                // Wait for outstanding tasks to complete
                task.Wait();
            }

            [Fact]
            public void ThrowsOperationCanceledExceptionWhenCancellationTokenIsSet()
            {
                var cancellationTokenSource = new CancellationTokenSource();

                var task = Task.Delay(250);
                
                var target = Task.Factory.StartNew(
                    () => task.Wait(Timeout.InfiniteTimeSpan, cancellationTokenSource.Token),
                    CancellationToken.None,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default);

                Thread.Sleep(TimeSpan.FromMilliseconds(150));

                Assert.False(target.IsCompleted);

                cancellationTokenSource.Cancel();
                
                var aggregateException = Assert.Throws<AggregateException>(() => target.Wait());
                Assert.Equal(typeof(OperationCanceledException),
                    aggregateException.Flatten().InnerExceptions.Single().GetType());

                // Wait for outstanding tasks to complete
                task.Wait();
            }

            [Fact]
            public void ReturnsFalseImmediatelyWhenTimeoutIsZero()
            {
                var task = Task.Delay(100);
                
                bool actual = task.Wait(TimeSpan.Zero, CancellationToken.None);

                Assert.False(actual);

                // Wait for outstanding tasks to complete
                task.Wait();
            }

            [Fact]
            public void ReturnsFalseWhenTimeoutExpires()
            {
                var task = Task.Delay(100);
                
                bool actual = task.Wait(TimeSpan.FromMilliseconds(25), CancellationToken.None);

                Assert.False(actual);

                // Wait for outstanding tasks to complete
                task.Wait();
            }
        }
    }
}