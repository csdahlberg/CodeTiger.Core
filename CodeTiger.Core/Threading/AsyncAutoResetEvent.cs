using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CodeTiger.Threading
{
    /// <summary>
    /// A thread synchronization event that notifies a waiting thread that an event has occurred.
    /// </summary>
    public sealed class AsyncAutoResetEvent : AsyncWaitHandle
    {
        private readonly ConcurrentQueue<TaskCompletionSource<bool>> _pendingWaitTaskSources
            = new ConcurrentQueue<TaskCompletionSource<bool>>();
        private readonly AsyncLock _pendingWaitTaskSourcesLock = new AsyncLock();

        private int _isSignaled = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncAutoResetEvent"/> class, specifying whether the
        /// event is initially signaled.
        /// </summary>
        /// <param name="initialState"><c>true</c> to set the initial state to signaled; <c>false</c> to set it to
        /// nonsignaled.</param>
        public AsyncAutoResetEvent(bool initialState)
        {
            _isSignaled = initialState ? 1 : 0;
        }

        /// <summary>
        /// Sets the state of the event to signaled, allowing one waiting thread to proceed.
        /// </summary>
        public void Set()
        {
            Set(CancellationToken.None);
        }

        /// <summary>
        /// Sets the state of the event to signaled, allowing one waiting thread to proceed.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        public void Set(CancellationToken cancellationToken)
        {
            // First try to signal a pending wait.
            if (!TrySignalPendingWaitTask())
            {
                using (_pendingWaitTaskSourcesLock.Acquire(cancellationToken))
                {
                    if (!TrySignalPendingWaitTask())
                    {
                        // If there are no pending waits to signal, mark this event as signaled so that the next
                        // wait will immediately be signaled.
                        Interlocked.Exchange(ref _isSignaled, 1);
                    }
                }
            }
        }

        /// <summary>
        /// Asynchronously sets the state of the event to signaled, allowing one waiting thread to proceed.
        /// </summary>
        /// <returns>A <see cref="Task"/> that will complete when the state of the event has been set to
        /// signaled.</returns>
        public async Task SetAsync()
        {
            await SetAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously sets the state of the event to signaled, allowing one waiting thread to proceed.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns>A <see cref="Task"/> that will complete when the state of the event has been set to
        /// signaled.</returns>
        public async Task SetAsync(CancellationToken cancellationToken)
        {
            // First try to signal a pending wait.
            if (!TrySignalPendingWaitTask())
            {
                using (await _pendingWaitTaskSourcesLock.AcquireAsync(cancellationToken).ConfigureAwait(false))
                {
                    if (!TrySignalPendingWaitTask())
                    {
                        // If there are no pending waits to signal, mark this event as signaled so that the next
                        // wait will immediately be signaled.
                        Interlocked.Exchange(ref _isSignaled, 1);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the state of the event to nonsignaled, causing threads to block.
        /// </summary>
        public void Reset()
        {
            Interlocked.Exchange(ref _isSignaled, 0);
        }

        /// <summary>
        /// Gets a <see cref="TaskCompletionSource{Boolean}"/> to use for a new wait operation.
        /// </summary>
        /// <returns>A <see cref="TaskCompletionSource{Boolean}"/> to use for a new wait operation.</returns>
        protected async override Task<TaskCompletionSource<bool>> GetWaitTaskSourceAsync(
            CancellationToken cancellationToken)
        {
            if (Interlocked.CompareExchange(ref _isSignaled, 0, 1) == 1)
            {
                return CompletedWaitTaskSource;
            }

            TaskCompletionSource<bool> waitTaskSource;

            using (await _pendingWaitTaskSourcesLock.AcquireAsync(cancellationToken).ConfigureAwait(false))
            {
                // If this event is already signaled, return the already-completed wait task.
                if (Interlocked.CompareExchange(ref _isSignaled, 0, 1) == 1)
                {
                    waitTaskSource = CompletedWaitTaskSource;
                }
                else
                {
                    waitTaskSource = new TaskCompletionSource<bool>();
                    cancellationToken.Register(() => waitTaskSource.TrySetCanceled());
                    _pendingWaitTaskSources.Enqueue(waitTaskSource);
                }
            }

            return waitTaskSource;
        }

        private bool TrySignalPendingWaitTask()
        {
            TaskCompletionSource<bool> queuedTask;
            while (_pendingWaitTaskSources.TryDequeue(out queuedTask))
            {
                // If TrySetResult returns false, it was already set by a timeout task.
                if (queuedTask.TrySetResult(true))
                {
                    return true;
                }
            }

            return false;
        }
    }
}