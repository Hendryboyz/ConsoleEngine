using System;

namespace ConsoleEngine.Domain.Interfaces
{
    public interface ICircuitBreaker
    {
        ICircuitBreakerState State { get; }

        Exception LastException { get; }

        DateTime LastStateChangedDateUtc { get; }

        void SetState(ICircuitBreakerState state);

        void Execute(Action action);

        void HandleException(Exception ex);

        bool IsClosed { get; }
    }
}
