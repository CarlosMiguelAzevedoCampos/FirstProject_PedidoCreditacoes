using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Logging.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;

namespace CMA.ISMAI.Notifications
{
    class Program : BaseConfiguration
    {

        static void Main(string[] args)
        {
            var services = ConfigureServices();

            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddSerilog();

            serviceProvider.GetRequiredService<ConsoleApplication>().Run();
            Console.ReadKey();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(ReturnSettingsValue("ElasticConfiguration", "Uri")))
               {
                   AutoRegisterTemplate = true,
               })
            .CreateLogger();

            services.AddLogging();
            services.AddTransient<ConsoleApplication>();
            services.AddScoped<ILog, LoggingService>();
            return services;
        }
    }
}
