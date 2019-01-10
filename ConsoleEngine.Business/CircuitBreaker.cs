using System;
using ConsoleEngine.Domain.Interfaces;

namespace ConsoleEngine.Business
{
    public class CircuitBreaker : ICircuitBreaker
    {
        public ICircuitBreakerState State { get; private set; }

        public Exception LastException { get; private set; }

        public DateTime LastStateChangedDateUtc { get; private set; }

        public bool IsClosed
        {
            get
            {
                return State.IsClose();
            }
        }

        public CircuitBreaker(ICircuitBreakerState startingState)
        {
            State = startingState;
        }

        public void Execute(Action action)
        {
            State.ExecuteAction(action);
        }

        public void HandleException(Exception ex)
        {
            LastException = ex;
        }

        public void SetState(ICircuitBreakerState state)
        {
            State = state;
            LastStateChangedDateUtc = DateTime.UtcNow;
        }
    }
}
