using System;
using System.Threading;
using System.Threading.Tasks;
using CodeTiger.Threading;
using Xunit;

namespace UnitTests.CodeTiger.Threading
{
    public class AsyncWaitHandleTests
    {
        public class Constructor
        {
            [Fact]
            public void DoesNotCallGetWaitTaskSource()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new MockAsyncWaitHandle(tcs);

                var cts = new CancellationTokenSource();

                Assert.Equal(0, target.WaitTaskRetrievalCount);
            }
        }

        public class WaitOne_TimeSpan_CancellationToken
        {
            [Fact]
            public void CallsGetWaitTaskSourceAsyncWithSameCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new MockAsyncWaitHandle(tcs);

                var cts = new CancellationTokenSource();
                target.WaitOne(TimeSpan.Zero, cts.Token);

                Assert.Equal(1, target.WaitTaskRetrievalCount);
                Assert.Equal(cts.Token, target.MostRecentCancellationToken);
            }
        }

        public class WaitOneAsync_TimeSpan_CancellationToken
        {
            [Fact]
            public void CallsGetWaitTaskSourceAsyncWithSameCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new MockAsyncWaitHandle(tcs);

                var cts = new CancellationTokenSource();
                target.WaitOneAsync(TimeSpan.Zero, cts.Token).GetAwaiter().GetResult();

                Assert.Equal(1, target.WaitTaskRetrievalCount);
                Assert.Equal(cts.Token, target.MostRecentCancellationToken);
            }
        }

        /// <summary>
        /// A subclass of <see cref="AsyncWaitHandle"/> with some functionality mocked to allow for unit testing.
        /// </summary>
        private class MockAsyncWaitHandle : AsyncWaitHandle
        {
            private readonly TaskCompletionSource<bool> _waitTaskCompletionSource;

            public CancellationToken MostRecentCancellationToken { get; private set; }

            public int WaitTaskRetrievalCount { get; private set; }

            public MockAsyncWaitHandle(TaskCompletionSource<bool> waitTaskCompletionSource)
            {
                _waitTaskCompletionSource = waitTaskCompletionSource;
            }

            protected override Task<TaskCompletionSource<bool>> GetWaitTaskSourceAsync(
                CancellationToken cancellationToken)
            {
                MostRecentCancellationToken = cancellationToken;
                WaitTaskRetrievalCount++;

                return Task.FromResult(_waitTaskCompletionSource);
            }
        }
    }
}