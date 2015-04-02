using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace CodeTiger.Threading
{
    /// <summary>
    /// Provides a mechanism for the mutual-exclusion of running code.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
        Justification = "LockReleaser does not contain any unmanaged resources, and only implements IDisposable"
            + " so that consumers may use 'using' blocks to hold locks.")]
    public sealed class AsyncLock
    {
        private readonly ConcurrentQueue<TaskCompletionSource<IDisposable>> _pendingWaitTaskSources
            = new ConcurrentQueue<TaskCompletionSource<IDisposable>>();
        private readonly LockReleaser _releaser;
        private readonly Task<IDisposable> _completedWaitTask;

        private int _acquiredCount = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLock"/> class.
        /// </summary>
        public AsyncLock()
        {
            _releaser = new LockReleaser(this);
            _completedWaitTask = Task.FromResult<IDisposable>(_releaser);
        }

        /// <summary>
        /// Acquires an exclusive lock synchronously.
        /// </summary>
        /// <returns>An <see cref="IDisposable"/> object that must be disposed to release the acquired lock.
        /// </returns>
        public IDisposable Acquire()
        {
            return GetWaitTask(CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Acquires an exclusive lock synchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns>An <see cref="IDisposable"/> object that must be disposed to release the acquired lock.
        /// </returns>
        public IDisposable Acquire(CancellationToken cancellationToken)
        {
            return GetWaitTask(cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Acquires an exclusive lock asynchronously.
        /// </summary>
        /// <returns>An <see cref="IDisposable"/> object that must be disposed to release the acquired lock.
        /// </returns>
        public Task<IDisposable> AcquireAsync()
        {
            return GetWaitTask(CancellationToken.None);
        }

        /// <summary>
        /// Acquires an exclusive lock asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns>An <see cref="IDisposable"/> object that must be disposed to release the acquired lock.
        /// </returns>
        public Task<IDisposable> AcquireAsync(CancellationToken cancellationToken)
        {
            return GetWaitTask(cancellationToken);
        }

        private Task<IDisposable> GetWaitTask(CancellationToken cancellationToken)
        {
            if (Interlocked.CompareExchange(ref _acquiredCount, 1, 0) == 0)
            {
                return _completedWaitTask;
            }

            lock (_pendingWaitTaskSources)
            {
                if (Interlocked.CompareExchange(ref _acquiredCount, 1, 0) == 0)
                {
                    return _completedWaitTask;
                }

                var waitTaskSource = new TaskCompletionSource<IDisposable>();
                var waitTask = waitTaskSource.Task;

                if (cancellationToken != CancellationToken.None)
                {
                    var cancellationRegistration = cancellationToken
                        .Register(() => waitTaskSource.TrySetCanceled());

                    waitTask = waitTask
                        .ContinueWith(task =>
                            {
                                if (waitTask.IsCanceled)
                                {
                                    cancellationRegistration.Dispose();
                                }

                                return task;
                            },
                            TaskContinuationOptions.ExecuteSynchronously)
                        .Unwrap();
                }

                _pendingWaitTaskSources.Enqueue(waitTaskSource);

                return waitTask;
            }
        }

        private void ReleaseLock()
        {
            if (!TrySignalPendingWaitTask())
            {
                lock (_pendingWaitTaskSources)
                {
                    if (!TrySignalPendingWaitTask())
                    {
                        if (Interlocked.CompareExchange(ref _acquiredCount, 0, 1) != 1)
                        {
                            throw new InvalidOperationException(
                                "The lock is not currently acquired, so it cannot be released.");
                        }
                    }
                }
            }
        }

        private bool TrySignalPendingWaitTask()
        {
            TaskCompletionSource<IDisposable> queuedTask;
            while (_pendingWaitTaskSources.TryDequeue(out queuedTask))
            {
                // If TrySetResult returns false, it was already set by a timeout task.
                if (queuedTask.TrySetResult(_releaser))
                {
                    return true;
                }
            }

            return false;
        }

        private class LockReleaser : IDisposable
        {
            private readonly AsyncLock _parent;

            public LockReleaser(AsyncLock parent)
            {
                _parent = parent;
            }

            public void Dispose()
            {
                _parent.ReleaseLock();
            }
        }
    }
}