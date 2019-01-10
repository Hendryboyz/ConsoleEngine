using System;
using ConsoleEngine.Domain.Interfaces;
using Polly;

namespace ConsoleEngine.Business
{
    public class PollyPolicyFacade : IPolicyFacade
    {
        private Policy _fallback;

        public PollyPolicyFacade(Policy fallback)
        {
            _fallback = fallback;
        }

        public void Execute(Action action)
        {
            _fallback.Execute(action);
        }
    }
}
