using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace NetCoreConsoleEngine.Extensions
{
    public static class IHostBuilderExtension
    {
        public static IHostBuilder UseHostedService<T>(this IHostBuilder hostBuilder)
        where T : class, IHostedService, IDisposable
        {
            return hostBuilder.ConfigureServices(services => services.AddHostedService<T>());
        }
    }
}
