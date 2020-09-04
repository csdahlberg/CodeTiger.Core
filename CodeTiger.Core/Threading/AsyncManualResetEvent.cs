using System.Threading;
using System.Threading.Tasks;

namespace CodeTiger.Threading
{
    /// <summary>
    /// A thread synchronization event that notifies one or more waiting threads that an event has occurred.
    /// </summary>
    public sealed class AsyncManualResetEvent : AsyncWaitHandle
    {
        private volatile TaskCompletionSource<bool> _waitTaskSource = new TaskCompletionSource<bool>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncManualResetEvent"/> class, specifying whether the
        /// event is initially signaled.
        /// </summary>
        /// <param name="initialState"><c>true</c> to set the initial state to signaled; <c>false</c> to set it to
        /// nonsignaled.</param>
        public AsyncManualResetEvent(bool initialState)
        {
            if (initialState)
            {
                Set();
            }
        }

        /// <summary>
        /// Sets the state of the event to signaled, allowing one or more waiting threads to proceed.
        /// </summary>
        public void Set()
        {
            _waitTaskSource.TrySetResult(true);
        }

        /// <summary>
        /// Sets the state of the event to nonsignaled, causing threads to block.
        /// </summary>
        public void Reset()
        {
            var spinner = new SpinWait();
            var newWaitTaskSource = new TaskCompletionSource<bool>();

            var knownWaitTaskSource = _waitTaskSource;

#pragma warning disable 0420 // Disable CS0420, since it is safe to use volatile fields with Interlocked.* methods
            while (knownWaitTaskSource.Task.IsCompleted
                && Interlocked.CompareExchange(ref _waitTaskSource, newWaitTaskSource, knownWaitTaskSource)
                    != knownWaitTaskSource)
            {
                spinner.SpinOnce();
                knownWaitTaskSource = _waitTaskSource;
            }
#pragma warning restore 0420
        }

        /// <summary>
        /// Gets a <see cref="TaskCompletionSource{Boolean}"/> to use for a new wait operation.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns>A <see cref="TaskCompletionSource{Boolean}"/> to use for a new wait operation.</returns>
        protected override TaskCompletionSource<bool> GetWaitTaskSource(
            CancellationToken cancellationToken)
        {
            return _waitTaskSource;
        }

        /// <summary>
        /// Gets a <see cref="TaskCompletionSource{Boolean}"/> to use for a new wait operation.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns>A <see cref="TaskCompletionSource{Boolean}"/> to use for a new wait operation.</returns>
        protected override Task<TaskCompletionSource<bool>> GetWaitTaskSourceAsync(
            CancellationToken cancellationToken)
        {
            return Task.FromResult(_waitTaskSource);
        }
    }
}
