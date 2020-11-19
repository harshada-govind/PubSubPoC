using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Subscriber
{
    public class SubscriberService
    {
        private static readonly ILog _logger = LogManager.GetLogger<SubscriberService>();
        private const string ENDPOINT = "Subscriber";

        public static async Task Main(string[] args)
        {
            bool isService = !(Debugger.IsAttached || args.Contains("--console"));
            IHostBuilder builder = CreateHostBuilder(args);

            if (isService)
            {
                builder.UseWindowsService().Build().Run();
                _logger.Info($"{ENDPOINT} started...");
            }
            else
            {
                await builder.RunConsoleAsync();
                Console.WriteLine("Press any key to shutdown");
                Console.ReadKey();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => 
            Host.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config.AddJsonFile("appsettings.json").Build();
               })
               .UseNServiceBus(ctx =>
                   new NServiceBusConfiguration(ENDPOINT, ctx.Configuration).Build()
               )
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
       });
    }
}
