using ConsoleEngine.Business;
using ConsoleEngine.Domain.Interfaces;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class PollyBuilderTests
    {
        private PollyBuilder pollyBuilder;
        private StringWriter outputWriter;

        [SetUp]
        public void Setup()
        {
            pollyBuilder = new PollyBuilder();
            SetUpOutputWriter();
        }

        private void SetUpOutputWriter()
        {
            outputWriter = new StringWriter();
            Console.SetOut(outputWriter);
        }

        [Test]
        public void GivenDefaultPolicyAndAction_WhenBuildAndExecute_ThenDoneActionOutput()
        {
            IPolicyFacade pollyFacade = pollyBuilder.Build();

            pollyFacade.Execute(() => { Console.WriteLine("Done"); });

            StringAssert.Contains("Done", outputWriter.ToString());
        }

        [Test]
        public void GivenFallbackAndAction_WhenBuildAndExecute_ThenDoneInOutput()
        {
            pollyBuilder.SetFallbackPolicy(() =>
            {
                Console.WriteLine("Some happened");
            });

            IPolicyFacade pollyFacade = pollyBuilder.Build();

            pollyFacade.Execute(() => { Console.WriteLine("Done"); });

            StringAssert.Contains("Done", outputWriter.ToString());
        }

        [Test]
        public void GivenFallbackAndExceptionAction_WhenBuildAndExecute_ThenFallbackMessageInOutput()
        {
            pollyBuilder.SetFallbackPolicy(() =>
            {
                Console.WriteLine("Some happened");
            });

            IPolicyFacade pollyFacade = pollyBuilder.Build();

            pollyFacade.Execute(() => { throw new Exception(); });

            StringAssert.Contains("Some happened", outputWriter.ToString());
        }

        [Test]
        public void GivenTimeoutAndAction_WhenBuildAndExecute_ThenTimeoutMessageInOutput()
        {
            IPolicyFacade pollyFacade = pollyBuilder
               .SetTimeoutPolicy(50, onTimeout: OnTimeout()).Build();

            pollyFacade.Execute(() => { while (true) { } });

            StringAssert.Contains("Timeout", outputWriter.ToString());
        }

        private Action<TimeSpan, Task, Exception> OnTimeout()
        {
            return (timespan, task, exception) =>
            {
                Console.WriteLine("Timeout");
            };
        }

        [Test]
        public void GivenRetryAndExceptionAction_WhenBuildAndExecute_ThenRetryMessageInOutput()
        {
            IPolicyFacade pollyFacade = pollyBuilder
               .SetRetryPolicy(1, 1, onRetry: OnRetry()).Build();

            pollyFacade.Execute(() => { throw new Exception(); });

            StringAssert.Contains("times retry", outputWriter.ToString());
        }

        private Action<Exception, TimeSpan, int> OnRetry()
        {
            return (exception, timespan, retryCount) =>
            {
                Console.WriteLine($"{retryCount} times retry");
            };
        }

        [Test]
        public void GivenBreakerRetryAndExceptionAction_WhenBuildAndExecute_ThenBreakerRetryMessageInOutput()
        {
            IPolicyFacade pollyFacade = pollyBuilder
                .SetRetryPolicy(1, 1, OnRetry())
                .SetCircuitBreakerPolicy(2, TimeSpan.FromMilliseconds(100), OnBreak(), OnReset()).Build();

            pollyFacade.Execute(() => { throw new Exception(); });

            StringAssert.Contains("times retry", outputWriter.ToString());
            StringAssert.Contains("Break", outputWriter.ToString());
        }

        private Action OnReset()
        {
            return () => { Console.WriteLine("Reset"); };
        }

        private Action<Exception, TimeSpan> OnBreak()
        {
            return (exception, timespan) =>
            {
                Console.WriteLine("Break");
            };
        }

        [Test]
        public void GivenAllPolicyAndOutput_WhenBuildAndExecute_ThenAllMessageInOutput()
        {
            IPolicyFacade pollyFacade = pollyBuilder
                .SetRetryPolicy(1, 1, OnRetry())
                .SetCircuitBreakerPolicy(2, TimeSpan.FromMilliseconds(100), OnBreak(), OnReset())
                .SetTimeoutPolicy(50, onTimeout: OnTimeout()).Build();

            pollyFacade.Execute(() =>
            {
                Console.WriteLine("Do");
                while (true) { }
            });

            StringAssert.Contains("Do", outputWriter.ToString());
            StringAssert.Contains("Timeout", outputWriter.ToString());
            StringAssert.Contains("times retry", outputWriter.ToString());
            StringAssert.Contains("Break", outputWriter.ToString());
        }
    }
}