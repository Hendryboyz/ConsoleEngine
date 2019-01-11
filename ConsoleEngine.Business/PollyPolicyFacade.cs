using System;
using ConsoleEngine.Domain.Interfaces;
using Polly;

namespace ConsoleEngine.Business
{
    public class PollyPolicyFacade : IPolicyFacade
    {
        private Policy _policy;

        public PollyPolicyFacade(Policy policy)
        {
            _policy = policy;
        }

        public void Execute(Action action)
        {
            _policy.Execute(action);
        }
    }
}
