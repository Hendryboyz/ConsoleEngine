using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace NetCoreConsoleEngine.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection RegisterConfiguration(this IServiceCollection services, HostBuilderContext host)
        {
            services.AddOptions();
            //services.Configure<MessageConsumerSettings>(
            //           options => host.Configuration.GetSection("MessageConsumerSettings").Bind(options));
            return services;
        }

        public static IServiceCollection ConfigureHostOptions(this IServiceCollection services, HostBuilderContext host)
        {
            services.Configure<HostOptions>(option =>
            {
                option.ShutdownTimeout = TimeSpan.FromSeconds(5);
            });
            return services;
        }

        public static IServiceCollection RegisterService(this IServiceCollection services, HostBuilderContext host)
        {
            //services.AddSingleton<IIDPMetaDataService, IDPMetaDataService>();
            return services;
        }

        public static IServiceCollection RegisterWrapper(this IServiceCollection services)
        {
            //services.AddSingleton<ILoggerWrapper, NLoggerWrapper>();
            return services;
        }
    }
}
