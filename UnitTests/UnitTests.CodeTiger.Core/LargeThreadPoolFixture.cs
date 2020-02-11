using System;
using System.Threading;

namespace UnitTests.CodeTiger
{
    public class LargeThreadPoolFixture
    {
        private static readonly int _minWorkerThreads = Math.Max(32, Environment.ProcessorCount * 8);

        public LargeThreadPoolFixture()
        {
            ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxCompletionPortThreads);

            if (maxWorkerThreads < _minWorkerThreads)
            {
                ThreadPool.SetMaxThreads(_minWorkerThreads, maxCompletionPortThreads);
            }

            ThreadPool.GetMinThreads(out int minWorkerThreads, out int minCompletionPortThreads);

            if (minWorkerThreads < _minWorkerThreads)
            {
                ThreadPool.SetMinThreads(_minWorkerThreads, minCompletionPortThreads);
            }
        }
    }
}
