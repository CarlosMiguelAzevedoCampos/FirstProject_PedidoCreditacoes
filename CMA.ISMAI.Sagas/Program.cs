using CMA.ISMAI.Engine.Automation.Sagas;
using CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Interface;
using CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Service;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Logging.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;

namespace CMA.ISMAI.Sagas
{
    class Program
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
               .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200/"))
               {
                   AutoRegisterTemplate = true,
               })
            .CreateLogger();

            services.AddLogging();
            services.AddScoped<ILog, LoggingService>();
            services.AddTransient<ICreditacoesService, CreditacoesService>();
            services.AddTransient<IHttpRequest, HttpRequest>();
            services.AddTransient<ConsoleApplication>();
            return services;
        }       
    }
}
