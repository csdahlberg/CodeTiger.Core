using System;
using System.Threading;
using System.Threading.Tasks;

namespace CodeTiger.Threading.Tasks
{
    /// <summary>
    /// Contains extension methods for the <see cref="Task"/> and <see cref="Task{TResult}"/> classes.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Wraps a <see cref="Task"/> with a new <see cref="Task"/> which will throw a
        /// <see cref="TimeoutException"/> if the original <see cref="Task"/> takes longer than a specified number
        /// of milliseconds to complete.
        /// </summary>
        /// <param name="task">The original task.</param>
        /// <param name="timeoutMilliseconds">The maximum number of milliseconds to wait for
        /// <paramref name="task"/> to complete.</param>
        /// <returns>A new <see cref="Task"/> which wraps <paramref name="task"/>, or just <paramref name="task"/>
        /// if either <paramref name="timeoutMilliseconds"/> is equal to <see cref="Timeout.Infinite"/> or
        /// <paramref name="task"/> has already completed.</returns>
        /// <exception cref="TimeoutException">Thrown when <paramref name="task"/> takes longer than the specified
        /// number of milliseconds to complete.</exception>
        public static Task WithTimeout(this Task task, int timeoutMilliseconds)
        {
            return WithTimeout(task, TimeSpan.FromMilliseconds(timeoutMilliseconds));
        }

        /// <summary>
        /// Wraps a <see cref="Task"/> with a new <see cref="Task"/> which will throw a
        /// <see cref="TimeoutException"/> if the original <see cref="Task"/> takes longer than a specified amount
        /// of time to complete.
        /// </summary>
        /// <param name="task">The original task.</param>
        /// <param name="timeout">The maximum amount of time to wait for <paramref name="task"/> to complete.
        /// </param>
        /// <returns>A new <see cref="Task"/> which wraps <paramref name="task"/>, or just <paramref name="task"/>
        /// if either <paramref name="timeout"/> is equal to <see cref="Timeout.InfiniteTimeSpan"/> or
        /// <paramref name="task"/> has already completed.</returns>
        /// <exception cref="TimeoutException">Thrown when <paramref name="task"/> takes longer than the specified
        /// amount of time to complete.</exception>
        public static Task WithTimeout(this Task task, TimeSpan timeout)
        {
            if (task == null || task.IsCompleted || timeout == Timeout.InfiniteTimeSpan)
            {
                return task;
            }

            return WaitForCompletionOrTimeoutAsync(task, timeout);
        }

        /// <summary>
        /// Wraps a <see cref="Task{TResult}"/> with a new <see cref="Task{TResult}"/> which will throw a
        /// <see cref="TimeoutException"/> if the original <see cref="Task{TResult}"/> takes longer than a
        /// specified number of milliseconds to complete.
        /// </summary>
        /// <param name="task">The original task.</param>
        /// <param name="timeoutMilliseconds">The maximum number of milliseconds to wait for
        /// <paramref name="task"/> to complete.</param>
        /// <returns>A new <see cref="Task{TResult}"/> which wraps <paramref name="task"/>, or just
        /// <paramref name="task"/> if either <paramref name="timeoutMilliseconds"/> is equal to
        /// <see cref="Timeout.Infinite"/> or <paramref name="task"/> has already completed.</returns>
        /// <exception cref="TimeoutException">Thrown when <paramref name="task"/> takes longer than the specified
        /// number of milliseconds to complete.</exception>
        public static Task<TResult> WithTimeout<TResult>(this Task<TResult> task, int timeoutMilliseconds)
        {
            return WithTimeout(task, TimeSpan.FromMilliseconds(timeoutMilliseconds));
        }

        /// <summary>
        /// Wraps a <see cref="Task{TResult}"/> with a new <see cref="Task{TResult}"/> which will throw a
        /// <see cref="TimeoutException"/> if the original <see cref="Task{TResult}"/> takes longer than a
        /// specified amount of time to complete.
        /// </summary>
        /// <param name="task">The original task.</param>
        /// <param name="timeout">The maximum amount of time to wait for <paramref name="task"/> to complete.
        /// </param>
        /// <returns>A new <see cref="Task{TResult}"/> which wraps <paramref name="task"/>, or just
        /// <paramref name="task"/> if either <paramref name="timeout"/> is equal to
        /// <see cref="Timeout.InfiniteTimeSpan"/> or <paramref name="task"/> has already completed.</returns>
        /// <exception cref="TimeoutException">Thrown when <paramref name="task"/> takes longer than the specified
        /// amount of time to complete.</exception>
        public static async Task<TResult> WithTimeout<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            if (timeout == Timeout.InfiniteTimeSpan || task.IsCompleted)
            {
                return await task;
            }

            await WaitForCompletionOrTimeoutAsync(task, timeout);

            return await task;
        }

        private static async Task WaitForCompletionOrTimeoutAsync(Task task, TimeSpan timeout)
        {
            var timeoutCancelTokenSource = new CancellationTokenSource();
            var timeoutTask = Task.Delay(timeout, timeoutCancelTokenSource.Token);

            var completedTask = await Task.WhenAny(task, timeoutTask).ConfigureAwait(false);
            if (completedTask == timeoutTask)
            {
                throw new TimeoutException();
            }
            
            timeoutCancelTokenSource.Cancel();
        }
    }
}