using System;

namespace ConsoleEngine.Domain.Interfaces
{
    public interface IPolicyBuilderDirector
    {
        IPolicyBuilder Builder { get; }

        IPolicyFacade Construct(Action action);
    }
}
