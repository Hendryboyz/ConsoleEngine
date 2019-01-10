using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCoreConsoleEngine.Extensions;
using NLog.Extensions.Logging;
using System.IO;

namespace NetCoreConsoleEngine
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("Conifgurations/hostsettings.json", optional: true);
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(Directory.GetCurrentDirectory());
                    configApp.AddJsonFile("Conifgurations/appsettings.json", optional: true, reloadOnChange: true);
                    configApp.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.RegisterConfiguration(hostContext)
                        .ConfigureHostOptions(hostContext)
                        .RegisterWrapper()
                        .RegisterService(hostContext);
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.SetMinimumLevel(LogLevel.Trace);
                    configLogging.AddNLog(new NLogProviderOptions
                    {
                        CaptureMessageTemplates = true,
                        CaptureMessageProperties = true
                    });
                    NLog.LogManager.LoadConfiguration("Conifgurations/nlog.config");
                })
                .UseConsoleLifetime()
                .Build();

            using (host)
            {
                await host.StartAsync();
                await host.WaitForShutdownAsync();
            }
        }
    }
}
