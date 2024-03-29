﻿using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using CodeTiger.Resources;

namespace CodeTiger.Threading
{
    /// <summary>
    /// Provides a mechanism for the mutual-exclusion of running code.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
        Justification = "LockReleaser does not contain any unmanaged resources, and only implements IDisposable"
            + " so that consumers may use 'using' blocks to hold locks.")]
    [SuppressMessage("CodeTiger.Reliability", "CT2001",
        Justification = "LockReleaser does not contain any unmanaged resources, and only implements IDisposable"
            + " so that consumers may use 'using' blocks to hold locks.")]
    public sealed class AsyncLock
    {
        private readonly ConcurrentQueue<TaskCompletionSource<IDisposable>> _pendingWaitTaskSources
            = new ConcurrentQueue<TaskCompletionSource<IDisposable>>();
        private readonly LockReleaser _releaser;
        private readonly Task<IDisposable> _completedWaitTask;

        private int _acquiredCount;

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
            return Acquire(CancellationToken.None);
        }

        /// <summary>
        /// Acquires an exclusive lock synchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns>An <see cref="IDisposable"/> object that must be disposed to release the acquired lock.
        /// </returns>
        public IDisposable Acquire(CancellationToken cancellationToken)
        {
            if (Interlocked.CompareExchange(ref _acquiredCount, 1, 0) == 0)
            {
                return _releaser;
            }

            TaskCompletionSource<IDisposable> waitTaskSource;

            lock (_pendingWaitTaskSources)
            {
                if (Interlocked.CompareExchange(ref _acquiredCount, 1, 0) == 0)
                {
                    return _releaser;
                }

                waitTaskSource = new TaskCompletionSource<IDisposable>();
                _pendingWaitTaskSources.Enqueue(waitTaskSource);
            }

            if (cancellationToken.CanBeCanceled)
            {
                cancellationToken.Register(() => waitTaskSource.TrySetCanceled());
            }

            return waitTaskSource.Task.Result;
        }

        /// <summary>
        /// Acquires an exclusive lock asynchronously.
        /// </summary>
        /// <returns>An <see cref="IDisposable"/> object that must be disposed to release the acquired lock.
        /// </returns>
        public Task<IDisposable> AcquireAsync()
        {
            return AcquireAsync(CancellationToken.None);
        }

        /// <summary>
        /// Acquires an exclusive lock asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns>An <see cref="IDisposable"/> object that must be disposed to release the acquired lock.
        /// </returns>
        public Task<IDisposable> AcquireAsync(CancellationToken cancellationToken)
        {
            if (Interlocked.CompareExchange(ref _acquiredCount, 1, 0) == 0)
            {
                return _completedWaitTask;
            }

            TaskCompletionSource<IDisposable> waitTaskSource;

            lock (_pendingWaitTaskSources)
            {
                if (Interlocked.CompareExchange(ref _acquiredCount, 1, 0) == 0)
                {
                    return _completedWaitTask;
                }

                waitTaskSource = new TaskCompletionSource<IDisposable>();
                _pendingWaitTaskSources.Enqueue(waitTaskSource);
            }

            if (!cancellationToken.CanBeCanceled)
            {
                return waitTaskSource.Task;
            }

            var cancellationRegistration = cancellationToken.Register(() => waitTaskSource.TrySetCanceled());

#if NET5_0_OR_GREATER
            return waitTaskSource.Task
                .ContinueWith(async task =>
                    {
                        if (waitTaskSource.Task.IsCanceled)
                        {
                            await cancellationRegistration.DisposeAsync().ConfigureAwait(false);
                        }

                        return await task.ConfigureAwait(false);
                    },
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Current)
                .Unwrap();
#else
            return waitTaskSource.Task
                .ContinueWith(task =>
                {
                    if (waitTaskSource.Task.IsCanceled)
                    {
                        cancellationRegistration.Dispose();
                    }

                    return task;
                },
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Current)
                .Unwrap();
#endif
        }

        private void ReleaseLock()
        {
            if (TrySignalPendingWaitTask())
            {
                return;
            }

            lock (_pendingWaitTaskSources)
            {
                if (!TrySignalPendingWaitTask())
                {
                    if (Interlocked.CompareExchange(ref _acquiredCount, 0, 1) != 1)
                    {
                        throw new InvalidOperationException(Strings.LockNotAcquiredSoCannotBeReleasedMessage);
                    }
                }
            }
        }

        private bool TrySignalPendingWaitTask()
        {
            while (_pendingWaitTaskSources.TryDequeue(out var queuedTask))
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
