using System;

namespace ConsoleEngine.Domain.Interfaces
{
    public interface ICircuitBreakerState
    {
        void ExecuteAction(Action action);
        bool IsClose();
    }
}
