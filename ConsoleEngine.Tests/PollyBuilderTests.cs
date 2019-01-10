using ConsoleEngine.Business;
using ConsoleEngine.Domain.Interfaces;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class PollyBuilderTests
    {
        private IPolicyBuilder pollyBuilder;

        [SetUp]
        public void Setup()
        {
            pollyBuilder = new PollyBuilder();
            pollyBuilder.SetFallbackPolicy(() =>
            {
                Console.WriteLine("over");
            });
        }


        [Test]
        public void GivenDefaultPolicy_WhenBuild_ThenDone()
        {
            pollyBuilder
                .Build(() => { Console.WriteLine("Done"); });
        }

        [Test]
        public void GivenDefaultPolicy_WhenBuild_ThenFallbackPolicy()
        {
            pollyBuilder
                .Build(() => { throw new Exception(); });
        }

        [Test]
        public void GivenTimeoutPolicy_WhenBuild_ThenCheckLog()
        {
            pollyBuilder
                .SetTimeoutPolicy(1, onTimeout: OnTimeout())
                .Build(() => { while (true) { } });
        }

        private Action<TimeSpan, Task, Exception> OnTimeout()
        {
            return (timespan, task, exception) =>
            {
                Console.WriteLine("WriteMemcached Timeout");
            };
        }

        [Test]
        public void GivenRetry_WhenBuild_ThenCheckLog()
        {
            pollyBuilder
                .SetRetryPolicy(2, OnRetry())
                .Build(() => { throw new Exception(); });
        }

        private Action<Exception, int> OnRetry()
        {
            return (exception, retryCount) =>
            {
                Console.WriteLine($"{retryCount} times retry");
            };
        }

        [Test]
        public void GivenBreakerRetry_WhenBuild_ThenCheckLog()
        {
            pollyBuilder
                .SetCircuitBreakerPolicy(3, TimeSpan.FromMilliseconds(1000), OnBreak(), OnReset())
                .SetRetryPolicy(2, OnRetry())
                .Build(() => { throw new Exception(); });
        }

        private Action OnReset()
        {
            return () =>
            {
                Console.WriteLine("Reset it");
            };
        }

        private Action<Exception, TimeSpan> OnBreak()
        {
            return (exception, timespan) =>
            {
                Console.WriteLine("Break this");
            };
        }

        [Test]
        public void GivenBreakerRetryTimeout_WhenBuild_ThenCheckLog()
        {
            pollyBuilder
                .SetRetryPolicy(2, OnRetry())
                .SetCircuitBreakerPolicy(3, TimeSpan.FromMilliseconds(1000), OnBreak(), OnReset())
                .SetTimeoutPolicy(1, onTimeout: OnTimeout())
                .Build(() => {
                    Console.WriteLine("do it");
                    while (true) { }
                });
        }
    }
}