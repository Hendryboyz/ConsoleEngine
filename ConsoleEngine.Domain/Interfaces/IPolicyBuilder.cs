using System;
using System.Threading.Tasks;

namespace ConsoleEngine.Domain.Interfaces
{
    public interface IPolicyBuilder
    {
        IPolicyFacade Build();

        IPolicyBuilder SetTimeoutPolicy(int seconds, Action<TimeSpan, Task, Exception> onTimeout);

        IPolicyBuilder SetFallbackPolicy(Action fallbackAction);

        IPolicyBuilder SetCircuitBreakerPolicy(int exceptionsAllowedBeforeBreaking, TimeSpan durationOfBreak, Action<Exception, TimeSpan> onBreak, Action onReset);

        IPolicyBuilder SetRetryPolicy(int retryCount, int sleepMilliseconds, Action<Exception, TimeSpan, int> onRetry);
    }
}
