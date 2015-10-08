using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using CodeTiger.Threading.Tasks;

namespace CodeTiger.Threading
{
    /// <summary>
    /// A thread synchronization event that can be used with task-based asynchronous programming.
    /// </summary>
    public abstract class AsyncWaitHandle
    {
        private static readonly TaskCompletionSource<bool> _completedWaitTaskSource = CreateCompletedSource();

        /// <summary>
        /// Gets the <see cref="TaskCompletionSource{Boolean}"/> to use when a wait operation completes
        /// immediately.
        /// </summary>
        protected static TaskCompletionSource<bool> CompletedWaitTaskSource
        {
            get { return _completedWaitTaskSource; }
        }

        /// <summary>
        /// Blocks the calling thread until this event is signaled.
        /// </summary>
        public void WaitOne()
        {
            WaitOne(Timeout.InfiniteTimeSpan, CancellationToken.None);
        }

        /// <summary>
        /// Blocks the calling thread until this event is signaled or the specified timeout elapses.
        /// </summary>
        /// <param name="timeoutMilliseconds">The number of milliseconds to wait, or
        /// <see cref="Timeout.Infinite"/> (negative 1) to wait indefinitely.</param>
        /// <returns><c>true</c> if this event was signaled before the timeout elapsed; otherwise, <c>false</c>.
        /// </returns>
        public bool WaitOne(int timeoutMilliseconds)
        {
            return WaitOne(TimeSpan.FromMilliseconds(timeoutMilliseconds), CancellationToken.None);
        }

        /// <summary>
        /// Blocks the calling thread until this event is signaled or the specified timeout elapses.
        /// </summary>
        /// <param name="timeout">The amount of time to wait, <see cref="Timeout.InfiniteTimeSpan"/> (negative 1
        /// milliseconds) to wait indefinitely.</param>
        /// <returns><c>true</c> if this event was signaled before the timeout elapsed; otherwise, <c>false</c>.
        /// </returns>
        public bool WaitOne(TimeSpan timeout)
        {
            return WaitOne(timeout, CancellationToken.None);
        }

        /// <summary>
        /// Blocks the calling thread until this event is signaled or the provided cancellation token is set.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns><c>true</c> if this event was signaled before the timeout elapsed; otherwise, <c>false</c>.
        /// </returns>
        public void WaitOne(CancellationToken cancellationToken)
        {
            WaitOne(Timeout.InfiniteTimeSpan, cancellationToken);
        }

        /// <summary>
        /// Blocks the calling thread until this event is signaled, the specified timeout elapses, or the provided
        /// cancellation token is set.
        /// </summary>
        /// <param name="timeoutMilliseconds">The number of milliseconds to wait, or
        /// <see cref="Timeout.Infinite"/> (negative 1) to wait indefinitely.</param>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns><c>true</c> if this event was signaled before the timeout elapsed; otherwise, <c>false</c>.
        /// </returns>
        public bool WaitOne(int timeoutMilliseconds, CancellationToken cancellationToken)
        {
            return WaitOne(TimeSpan.FromMilliseconds(timeoutMilliseconds), cancellationToken);
        }

        /// <summary>
        /// Blocks the calling thread until this event is signaled, the specified timeout elapses, or the provided
        /// cancellation token is set.
        /// </summary>
        /// <param name="timeout">The amount of time to wait, <see cref="Timeout.InfiniteTimeSpan"/> (negative 1
        /// milliseconds) to wait indefinitely.</param>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns><c>true</c> if this event was signaled before the timeout elapsed; otherwise, <c>false</c>.
        /// </returns>
        public bool WaitOne(TimeSpan timeout, CancellationToken cancellationToken)
        {
            var waitTaskSource = GetWaitTaskSource(cancellationToken);

            if (cancellationToken.CanBeCanceled)
            {
                // Have the cancellation token attempt to cancel the wait task.
                cancellationToken.Register(() => waitTaskSource.TrySetCanceled());
            }

            return Task.Run(() => waitTaskSource.Task.Wait(timeout, cancellationToken)).Result;
        }

        /// <summary>
        /// Asynchronously waits until this event is signaled.
        /// </summary>
        /// <returns>A <see cref="Task"/> that will complete when the event is signaled.</returns>
        public Task WaitOneAsync()
        {
            return WaitOneAsync(Timeout.InfiniteTimeSpan, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronously waits until this event is signaled or the specified timeout elapses.
        /// </summary>
        /// <param name="timeoutMilliseconds">The number of milliseconds to wait, or
        /// <see cref="Timeout.Infinite"/> (negative 1) to wait indefinitely.</param>
        /// <returns>A <see cref="Task{Boolean}"/> that will complete with a result of <c>true</c> if this wait
        /// handle was signaled before the timeout elapsed; otherwise, with a result of <c>false</c>.</returns>
        public Task<bool> WaitOneAsync(int timeoutMilliseconds)
        {
            return WaitOneAsync(TimeSpan.FromMilliseconds(timeoutMilliseconds), CancellationToken.None);
        }

        /// <summary>
        /// Asynchronously waits until this event is signaled or the specified timeout elapses.
        /// </summary>
        /// <param name="timeout">The amount of time to wait, <see cref="Timeout.InfiniteTimeSpan"/> (negative 1
        /// milliseconds) to wait indefinitely.</param>
        /// <returns>A <see cref="Task{Boolean}"/> that will complete with a result of <c>true</c> if this wait
        /// handle was signaled before the timeout elapsed; otherwise, with a result of <c>false</c>.</returns>
        public Task<bool> WaitOneAsync(TimeSpan timeout)
        {
            return WaitOneAsync(timeout, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronously waits until this event is signaled or the provided cancellation token is set.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns>A <see cref="Task{Boolean}"/> that will complete with a result of <c>true</c> if this wait
        /// handle was signaled before the timeout elapsed; otherwise, with a result of <c>false</c>.</returns>
        public Task<bool> WaitOneAsync(CancellationToken cancellationToken)
        {
            return WaitOneAsync(Timeout.InfiniteTimeSpan, cancellationToken);
        }

        /// <summary>
        /// Asynchronously waits until this event is signaled, the specified timeout elapses, or the provided
        /// cancellation token is set.
        /// </summary>
        /// <param name="timeoutMilliseconds">The number of milliseconds to wait, or
        /// <see cref="Timeout.Infinite"/> (negative 1) to wait indefinitely.</param>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns>A <see cref="Task{Boolean}"/> that will complete with a result of <c>true</c> if this wait
        /// handle was signaled before the timeout elapsed; otherwise, with a result of <c>false</c>.</returns>
        public Task<bool> WaitOneAsync(int timeoutMilliseconds, CancellationToken cancellationToken)
        {
            return WaitOneAsync(TimeSpan.FromMilliseconds(timeoutMilliseconds), cancellationToken);
        }

        /// <summary>
        /// Asynchronously waits until this event is signaled, the specified timeout elapses, or the provided
        /// cancellation token is set.
        /// </summary>
        /// <param name="timeout">The amount of time to wait, <see cref="Timeout.InfiniteTimeSpan"/> (negative 1
        /// milliseconds) to wait indefinitely.</param>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns>A <see cref="Task{Boolean}"/> that will complete with a result of <c>true</c> if this wait
        /// handle was signaled before the timeout elapsed; otherwise, with a result of <c>false</c>.</returns>
        public async Task<bool> WaitOneAsync(TimeSpan timeout, CancellationToken cancellationToken)
        {
            var waitTaskSource = await GetWaitTaskSourceAsync(cancellationToken).ConfigureAwait(false);

            if (cancellationToken.CanBeCanceled)
            {
                // Have the cancellation token attempt to cancel the wait task.
                cancellationToken.Register(() => waitTaskSource.TrySetCanceled());
            }
            
            if (timeout == Timeout.InfiniteTimeSpan)
            {
                return await waitTaskSource.Task.ConfigureAwait(false);
            }

            using (var compositeCancellationSource = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken))
            {
                var timeoutTask = Task.Delay(timeout, compositeCancellationSource.Token);
                var completedTask = await Task.WhenAny(waitTaskSource.Task, timeoutTask).ConfigureAwait(false);

                if (completedTask == timeoutTask)
                {
                    waitTaskSource.TrySetResult(false);
                }
                else
                {
                    // If the timeout task has not yet completed, use the composite cancellation token to cancel it
                    // so it will not continue running in the background.
                    compositeCancellationSource.Cancel();
                }
            }

            return await waitTaskSource.Task.ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a <see cref="TaskCompletionSource{Boolean}"/> object to use for a new wait operation.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns>A <see cref="TaskCompletionSource{Boolean}"/> object to use for a new wait operation.
        /// </returns>
        protected abstract TaskCompletionSource<bool> GetWaitTaskSource(
            CancellationToken cancellationToken);

        /// <summary>
        /// Gets a <see cref="TaskCompletionSource{Boolean}"/> object to use for a new wait operation.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns>A <see cref="TaskCompletionSource{Boolean}"/> object to use for a new wait operation.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected abstract Task<TaskCompletionSource<bool>> GetWaitTaskSourceAsync(
            CancellationToken cancellationToken);

        private static TaskCompletionSource<bool> CreateCompletedSource()
        {
            var tcs = new TaskCompletionSource<bool>();

            tcs.SetResult(true);

            return tcs;
        }
    }
}