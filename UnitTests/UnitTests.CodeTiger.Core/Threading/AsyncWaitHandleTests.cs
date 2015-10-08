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

        public class WaitOne
        {
            [Fact]
            public void CallsGetWaitTaskSourceWithCorrectCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new MockAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                target.WaitOne();

                Assert.Equal(1, target.WaitTaskRetrievalCount);
                Assert.Equal(CancellationToken.None, target.MostRecentCancellationToken);
            }
        }

        public class WaitOne_Int32
        {
            [Fact]
            public void CallsGetWaitTaskSourceWithCorrectCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new MockAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                target.WaitOne(0);

                Assert.Equal(1, target.WaitTaskRetrievalCount);
                Assert.Equal(CancellationToken.None, target.MostRecentCancellationToken);
            }
        }

        public class WaitOne_TimeSpan
        {
            [Fact]
            public void CallsGetWaitTaskSourceWithCorrectCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new MockAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                target.WaitOne(TimeSpan.Zero);

                Assert.Equal(1, target.WaitTaskRetrievalCount);
                Assert.Equal(CancellationToken.None, target.MostRecentCancellationToken);
            }
        }

        public class WaitOne_CancellationToken
        {
            [Fact]
            public void CallsGetWaitTaskSourceWithSameCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new MockAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                var cts = new CancellationTokenSource();
                target.WaitOne(cts.Token);

                Assert.Equal(1, target.WaitTaskRetrievalCount);
                Assert.Equal(cts.Token, target.MostRecentCancellationToken);
            }
        }

        public class WaitOne_Int32_CancellationToken
        {
            [Fact]
            public void CallsGetWaitTaskSourceWithSameCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new MockAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                var cts = new CancellationTokenSource();
                target.WaitOne(0, cts.Token);

                Assert.Equal(1, target.WaitTaskRetrievalCount);
                Assert.Equal(cts.Token, target.MostRecentCancellationToken);
            }
        }

        public class WaitOne_TimeSpan_CancellationToken
        {
            [Fact]
            public void CallsGetWaitTaskSourceWithSameCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new MockAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                var cts = new CancellationTokenSource();
                target.WaitOne(TimeSpan.Zero, cts.Token);

                Assert.Equal(1, target.WaitTaskRetrievalCount);
                Assert.Equal(cts.Token, target.MostRecentCancellationToken);
            }
        }

        public class WaitOneAsync
        {
            [Fact]
            public async Task CallsGetWaitTaskSourceAsyncWithCorrectCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new MockAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                await target.WaitOneAsync();

                Assert.Equal(1, target.WaitTaskAsyncRetrievalCount);
                Assert.Equal(CancellationToken.None, target.MostRecentCancellationToken);
            }
        }

        public class WaitOneAsync_Int32
        {
            [Fact]
            public async Task CallsGetWaitTaskSourceAsyncWithCorrectCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new MockAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                await target.WaitOneAsync(0);

                Assert.Equal(1, target.WaitTaskAsyncRetrievalCount);
                Assert.Equal(CancellationToken.None, target.MostRecentCancellationToken);
            }
        }

        public class WaitOneAsync_TimeSpan
        {
            [Fact]
            public async Task CallsGetWaitTaskSourceAsyncWithCorrectCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new MockAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                await target.WaitOneAsync(TimeSpan.Zero);

                Assert.Equal(1, target.WaitTaskAsyncRetrievalCount);
                Assert.Equal(CancellationToken.None, target.MostRecentCancellationToken);
            }
        }

        public class WaitOneAsync_CancellationToken
        {
            [Fact]
            public async Task CallsGetWaitTaskSourceAsyncWithSameCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new MockAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                var cts = new CancellationTokenSource();
                await target.WaitOneAsync(cts.Token);

                Assert.Equal(1, target.WaitTaskAsyncRetrievalCount);
                Assert.Equal(cts.Token, target.MostRecentCancellationToken);
            }
        }

        public class WaitOneAsync_Int32_CancellationToken
        {
            [Fact]
            public async Task CallsGetWaitTaskSourceAsyncWithSameCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new MockAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                var cts = new CancellationTokenSource();
                await target.WaitOneAsync(0, cts.Token);

                Assert.Equal(1, target.WaitTaskAsyncRetrievalCount);
                Assert.Equal(cts.Token, target.MostRecentCancellationToken);
            }
        }

        public class WaitOneAsync_TimeSpan_CancellationToken
        {
            [Fact]
            public async Task CallsGetWaitTaskSourceAsyncWithSameCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new MockAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                var cts = new CancellationTokenSource();
                await target.WaitOneAsync(TimeSpan.Zero, cts.Token);

                Assert.Equal(1, target.WaitTaskAsyncRetrievalCount);
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

            public int WaitTaskAsyncRetrievalCount { get; private set; }

            public MockAsyncWaitHandle(TaskCompletionSource<bool> waitTaskCompletionSource)
            {
                _waitTaskCompletionSource = waitTaskCompletionSource;
            }

            protected override TaskCompletionSource<bool> GetWaitTaskSource(
                CancellationToken cancellationToken)
            {
                MostRecentCancellationToken = cancellationToken;
                WaitTaskRetrievalCount++;

                return _waitTaskCompletionSource;
            }

            protected override Task<TaskCompletionSource<bool>> GetWaitTaskSourceAsync(
                CancellationToken cancellationToken)
            {
                MostRecentCancellationToken = cancellationToken;
                WaitTaskAsyncRetrievalCount++;

                return Task.FromResult(_waitTaskCompletionSource);
            }
        }
    }
}