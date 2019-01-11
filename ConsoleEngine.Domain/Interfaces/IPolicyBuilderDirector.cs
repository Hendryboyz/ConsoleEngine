using System;

namespace ConsoleEngine.Domain.Interfaces
{
    public interface IPolicyBuilderDirector
    {
        IPolicyBuilder _builder { get; }

        IPolicyFacade Construct();
    }
}
