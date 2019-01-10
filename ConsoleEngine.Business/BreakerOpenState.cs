using System;
using ConsoleEngine.Domain.Interfaces;

namespace ConsoleEngine.Business
{
    class BreakerOpenState : ICircuitBreakerState
    {
        public void ExecuteAction(Action action)
        {
            throw new NotImplementedException();
        }

        public bool IsClose()
        {
            throw new NotImplementedException();
        }
    }
}
