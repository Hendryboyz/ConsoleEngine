using ConsoleEngine.Domain.Interfaces;
using System;

namespace ConsoleEngine.Business
{
    class BreakerHalfOpenState : ICircuitBreakerState
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
