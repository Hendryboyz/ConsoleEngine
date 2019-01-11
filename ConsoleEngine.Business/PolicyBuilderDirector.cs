using ConsoleEngine.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace ConsoleEngine.Business
{
    public class PolicyBuilderDirector : IPolicyBuilderDirector
    {
        public IPolicyBuilder _builder { get; private set; }

        public PolicyBuilderDirector(IPolicyBuilder policyBuilder)
        {
            _builder = policyBuilder;
        }

        public IPolicyFacade Construct()
        {
            _builder.SetRetryPolicy(2, 1, OnRetry());
            _builder.SetTimeoutPolicy(1, onTimeout: OnTimeout());
            return _builder.Build();
        }


        private Action<Exception, TimeSpan, int> OnRetry()
        {
            return (exception, timespan, retryCount) =>
            {
                Console.WriteLine($"{retryCount} times retry");
            };
        }

        private Action<TimeSpan, Task, Exception> OnTimeout()
        {
            return (timespan, task, exception) =>
            {
                Console.WriteLine("WriteMemcached Timeout");
            };
        }
    }
}
