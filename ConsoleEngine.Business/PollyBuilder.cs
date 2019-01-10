using ConsoleEngine.Domain.Interfaces;
using Polly;
using Polly.Timeout;
using System;
using System.Threading.Tasks;

namespace ConsoleEngine.Business
{
    public class PollyBuilder : IPolicyBuilder
    {
        private Policy _fallback;
        private Policy _retryPolicy;
        private Policy _breakerPolicy;
        private Policy _timeoutPolicy;

        public IPolicyFacade Build(Action action)
        {
            _fallback = Policy.Handle<Exception>().Fallback(() => { });

            AddPolicy(_retryPolicy);
            AddPolicy(_breakerPolicy);
            AddPolicy(_timeoutPolicy);

            return new PollyPolicyFacade(_fallback);
        }

        private void AddPolicy(Policy policy)
        {
            if (null != policy)
            {
                _fallback = _fallback.Wrap(policy);
            }
        }

        public IPolicyBuilder SetCircuitBreakerPolicy(int exceptionsAllowedBeforeBreaking, TimeSpan durationOfBreak, Action<Exception, TimeSpan> onBreak, Action onReset)
        {
            _breakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreaker(exceptionsAllowedBeforeBreaking, durationOfBreak, onBreak, onReset);
            return this;
        }

        public IPolicyBuilder SetFallbackPolicy(Action fallbackAction)
        {
            _fallback = Policy.Handle<Exception>().Fallback(fallbackAction);
            return this;
        }

        public IPolicyBuilder SetRetryPolicy(int retryCount, Action<Exception, int> onRetry)
        {
            _retryPolicy = Policy.Handle<Exception>().Retry(retryCount, onRetry);
            return this;
        }

        public IPolicyBuilder SetTimeoutPolicy(int seconds, Action<TimeSpan, Task, Exception> onTimeout)
        {
            _timeoutPolicy = Policy.Timeout(seconds, TimeoutStrategy.Pessimistic, 
                onTimeout: (context, timespan, task, exception) => onTimeout(timespan, task, exception));
            return this;
        }
    }
}
