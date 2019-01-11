using ConsoleEngine.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace ConsoleEngine.Business
{
    public class PolicyBuilderDirector : IPolicyBuilderDirector
    {
        public IPolicyBuilder Builder { get; private set; }

        public PolicyBuilderDirector()
        {

        }

        public IPolicyFacade Construct()
        {
            return Builder
               .SetRetryPolicy(2, 1, OnRetry())
               .SetTimeoutPolicy(1, onTimeout: OnTimeout())
               .Build();
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
