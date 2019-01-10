using System;

namespace ConsoleEngine.Domain.Interfaces
{
    public interface IPolicyFacade
    {
        void Execute(Action action);
    }
}
