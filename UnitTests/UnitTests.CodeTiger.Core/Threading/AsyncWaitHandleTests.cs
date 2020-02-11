using System;
using System.Threading;
using System.Threading.Tasks;
using CodeTiger.Threading;
using Xunit;

namespace UnitTests.CodeTiger.Threading
{
    public static class AsyncWaitHandleTests
    {
        [Collection("AsyncWaitHandle.Constructor collection")]
        public class Constructor
        {
            public Constructor(LargeThreadPoolFixture fixture)
            {
                _ = fixture;
            }

            [Fact]
            public void DoesNotCallGetWaitTaskSource()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new TestableAsyncWaitHandle(tcs);

                Assert.Equal(0, target.WaitTaskRetrievalCount);
            }

            [CollectionDefinition("AsyncWaitHandle.Constructor collection")]
            public class LargeThreadPoolCollection : ICollectionFixture<LargeThreadPoolFixture>
            {
            }
        }

        [Collection("AsyncWaitHandle.WaitOne collection")]
        public class WaitOne
        {
            public WaitOne(LargeThreadPoolFixture fixture)
            {
                _ = fixture;
            }

            [Fact]
            public void CallsGetWaitTaskSourceWithCorrectCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new TestableAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                target.WaitOne();

                Assert.Equal(1, target.WaitTaskRetrievalCount);
                Assert.Equal(CancellationToken.None, target.MostRecentCancellationToken);
            }

            [CollectionDefinition("AsyncWaitHandle.WaitOne collection")]
            public class LargeThreadPoolCollection : ICollectionFixture<LargeThreadPoolFixture>
            {
            }
        }

        [Collection("AsyncWaitHandle.WaitOne_Int32 collection")]
        public class WaitOne_Int32
        {
            public WaitOne_Int32(LargeThreadPoolFixture fixture)
            {
                _ = fixture;
            }

            [Fact]
            public void CallsGetWaitTaskSourceWithCorrectCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new TestableAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                target.WaitOne(0);

                Assert.Equal(1, target.WaitTaskRetrievalCount);
                Assert.Equal(CancellationToken.None, target.MostRecentCancellationToken);
            }

            [CollectionDefinition("AsyncWaitHandle.WaitOne_Int32 collection")]
            public class LargeThreadPoolCollection : ICollectionFixture<LargeThreadPoolFixture>
            {
            }
        }

        [Collection("AsyncWaitHandle.WaitOne_TimeSpan collection")]
        public class WaitOne_TimeSpan
        {
            public WaitOne_TimeSpan(LargeThreadPoolFixture fixture)
            {
                _ = fixture;
            }

            [Fact]
            public void CallsGetWaitTaskSourceWithCorrectCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new TestableAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                target.WaitOne(TimeSpan.Zero);

                Assert.Equal(1, target.WaitTaskRetrievalCount);
                Assert.Equal(CancellationToken.None, target.MostRecentCancellationToken);
            }

            [CollectionDefinition("AsyncWaitHandle.WaitOne_TimeSpan collection")]
            public class LargeThreadPoolCollection : ICollectionFixture<LargeThreadPoolFixture>
            {
            }
        }

        [Collection("AsyncWaitHandle.WaitOne_CancellationToken collection")]
        public class WaitOne_CancellationToken
        {
            public WaitOne_CancellationToken(LargeThreadPoolFixture fixture)
            {
                _ = fixture;
            }

            [Fact]
            public void CallsGetWaitTaskSourceWithSameCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new TestableAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                using (var cts = new CancellationTokenSource())
                {
                    target.WaitOne(cts.Token);

                    Assert.Equal(cts.Token, target.MostRecentCancellationToken);
                }

                Assert.Equal(1, target.WaitTaskRetrievalCount);
            }

            [CollectionDefinition("AsyncWaitHandle.WaitOne_CancellationToken collection")]
            public class LargeThreadPoolCollection : ICollectionFixture<LargeThreadPoolFixture>
            {
            }
        }

        [Collection("AsyncWaitHandle.WaitOne_Int32_CancellationToken collection")]
        public class WaitOne_Int32_CancellationToken
        {
            public WaitOne_Int32_CancellationToken(LargeThreadPoolFixture fixture)
            {
                _ = fixture;
            }

            [Fact]
            public void CallsGetWaitTaskSourceWithSameCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new TestableAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                using (var cts = new CancellationTokenSource())
                {
                    target.WaitOne(0, cts.Token);

                    Assert.Equal(cts.Token, target.MostRecentCancellationToken);
                }

                Assert.Equal(1, target.WaitTaskRetrievalCount);
            }

            [CollectionDefinition("AsyncWaitHandle.WaitOne_Int32_CancellationToken collection")]
            public class LargeThreadPoolCollection : ICollectionFixture<LargeThreadPoolFixture>
            {
            }
        }

        [Collection("AsyncWaitHandle.WaitOne_TimeSpan_CancellationToken collection")]
        public class WaitOne_TimeSpan_CancellationToken
        {
            public WaitOne_TimeSpan_CancellationToken(LargeThreadPoolFixture fixture)
            {
                _ = fixture;
            }

            [Fact]
            public void CallsGetWaitTaskSourceWithSameCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new TestableAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                using (var cts = new CancellationTokenSource())
                {
                    target.WaitOne(TimeSpan.Zero, cts.Token);

                    Assert.Equal(cts.Token, target.MostRecentCancellationToken);
                }

                Assert.Equal(1, target.WaitTaskRetrievalCount);
            }

            [CollectionDefinition("AsyncWaitHandle.WaitOne_TimeSpan_CancellationToken collection")]
            public class LargeThreadPoolCollection : ICollectionFixture<LargeThreadPoolFixture>
            {
            }
        }

        [Collection("AsyncWaitHandle.WaitOneAsync collection")]
        public class WaitOneAsync
        {
            public WaitOneAsync(LargeThreadPoolFixture fixture)
            {
                _ = fixture;
            }

            [Fact]
            public async Task CallsGetWaitTaskSourceAsyncWithCorrectCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new TestableAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                await target.WaitOneAsync().ConfigureAwait(false);

                Assert.Equal(1, target.WaitTaskAsyncRetrievalCount);
                Assert.Equal(CancellationToken.None, target.MostRecentCancellationToken);
            }

            [CollectionDefinition("AsyncWaitHandle.WaitOneAsync collection")]
            public class LargeThreadPoolCollection : ICollectionFixture<LargeThreadPoolFixture>
            {
            }
        }

        [Collection("AsyncWaitHandle.WaitOneAsync_Int32 collection")]
        public class WaitOneAsync_Int32
        {
            public WaitOneAsync_Int32(LargeThreadPoolFixture fixture)
            {
                _ = fixture;
            }

            [Fact]
            public async Task CallsGetWaitTaskSourceAsyncWithCorrectCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new TestableAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                await target.WaitOneAsync(0).ConfigureAwait(false);

                Assert.Equal(1, target.WaitTaskAsyncRetrievalCount);
                Assert.Equal(CancellationToken.None, target.MostRecentCancellationToken);
            }

            [CollectionDefinition("AsyncWaitHandle.WaitOneAsync_Int32 collection")]
            public class LargeThreadPoolCollection : ICollectionFixture<LargeThreadPoolFixture>
            {
            }
        }

        [Collection("AsyncWaitHandle.WaitOneAsync_TimeSpan collection")]
        public class WaitOneAsync_TimeSpan
        {
            public WaitOneAsync_TimeSpan(LargeThreadPoolFixture fixture)
            {
                _ = fixture;
            }

            [Fact]
            public async Task CallsGetWaitTaskSourceAsyncWithCorrectCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new TestableAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                await target.WaitOneAsync(TimeSpan.Zero).ConfigureAwait(false);

                Assert.Equal(1, target.WaitTaskAsyncRetrievalCount);
                Assert.Equal(CancellationToken.None, target.MostRecentCancellationToken);
            }

            [CollectionDefinition("AsyncWaitHandle.WaitOneAsync_TimeSpan collection")]
            public class LargeThreadPoolCollection : ICollectionFixture<LargeThreadPoolFixture>
            {
            }
        }

        [Collection("AsyncWaitHandle.WaitOneAsync_CancellationToken collection")]
        public class WaitOneAsync_CancellationToken
        {
            public WaitOneAsync_CancellationToken(LargeThreadPoolFixture fixture)
            {
                _ = fixture;
            }

            [Fact]
            public async Task CallsGetWaitTaskSourceAsyncWithSameCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new TestableAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                using (var cts = new CancellationTokenSource())
                {
                    await target.WaitOneAsync(cts.Token).ConfigureAwait(false);

                    Assert.Equal(cts.Token, target.MostRecentCancellationToken);
                }

                Assert.Equal(1, target.WaitTaskAsyncRetrievalCount);
            }

            [CollectionDefinition("AsyncWaitHandle.WaitOneAsync_CancellationToken collection")]
            public class LargeThreadPoolCollection : ICollectionFixture<LargeThreadPoolFixture>
            {
            }
        }

        [Collection("AsyncWaitHandle.WaitOneAsync_Int32_CancellationToken collection")]
        public class WaitOneAsync_Int32_CancellationToken
        {
            public WaitOneAsync_Int32_CancellationToken(LargeThreadPoolFixture fixture)
            {
                _ = fixture;
            }

            [Fact]
            public async Task CallsGetWaitTaskSourceAsyncWithSameCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new TestableAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                using (var cts = new CancellationTokenSource())
                {
                    await target.WaitOneAsync(0, cts.Token).ConfigureAwait(false);

                    Assert.Equal(cts.Token, target.MostRecentCancellationToken);
                }

                Assert.Equal(1, target.WaitTaskAsyncRetrievalCount);
            }

            [CollectionDefinition("AsyncWaitHandle.WaitOneAsync_Int32_CancellationToken collection")]
            public class LargeThreadPoolCollection : ICollectionFixture<LargeThreadPoolFixture>
            {
            }
        }

        [Collection("AsyncWaitHandle.WaitOneAsync_TimeSpan_CancellationToken collection")]
        public class WaitOneAsync_TimeSpan_CancellationToken
        {
            public WaitOneAsync_TimeSpan_CancellationToken(LargeThreadPoolFixture fixture)
            {
                _ = fixture;
            }

            [Fact]
            public async Task CallsGetWaitTaskSourceAsyncWithSameCancellationToken()
            {
                var tcs = new TaskCompletionSource<bool>();
                var target = new TestableAsyncWaitHandle(tcs);

                tcs.SetResult(false);

                using (var cts = new CancellationTokenSource())
                {
                    await target.WaitOneAsync(TimeSpan.Zero, cts.Token).ConfigureAwait(false);

                    Assert.Equal(cts.Token, target.MostRecentCancellationToken);
                }

                Assert.Equal(1, target.WaitTaskAsyncRetrievalCount);
            }

            [CollectionDefinition("AsyncWaitHandle.WaitOneAsync_TimeSpan_CancellationToken collection")]
            public class LargeThreadPoolCollection : ICollectionFixture<LargeThreadPoolFixture>
            {
            }
        }

        /// <summary>
        /// A subclass of <see cref="AsyncWaitHandle"/> with some functionality mocked to allow for unit testing.
        /// </summary>
        private class TestableAsyncWaitHandle : AsyncWaitHandle
        {
            private readonly TaskCompletionSource<bool> _waitTaskCompletionSource;

            public CancellationToken MostRecentCancellationToken { get; private set; }

            public int WaitTaskRetrievalCount { get; private set; }

            public int WaitTaskAsyncRetrievalCount { get; private set; }

            public TestableAsyncWaitHandle(TaskCompletionSource<bool> waitTaskCompletionSource)
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