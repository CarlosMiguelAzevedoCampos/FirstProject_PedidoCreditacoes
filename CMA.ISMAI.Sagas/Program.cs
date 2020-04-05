using CMA.ISMAI.Engine.Automation.Sagas;
using CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Interface;
using CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Service;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Logging.Service;
using CMA.ISMAI.Sagas.Creditacoes;
using CMA.ISMAI.Sagas.Services;
using CMA.ISMAI.Sagas.Services.Base;
using CMA.ISMAI.Sagas.Services.Interface;
using CMA.ISMAI.Sagas.Services.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;

namespace CMA.ISMAI.Sagas
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
            IServiceCollection  services = new ServiceCollection();
            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext() 
               .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(ReturnSettingsValue("ElasticConfiguration", "Uri")))
               {
                   AutoRegisterTemplate = true,
               })
            .CreateLogger();

            services.AddLogging();
            services.AddScoped<ILog, LoggingService>();
            services.AddScoped<ICreditacaoService, CreditacaoService>();
            services.AddScoped<ISagaService, SagaService>();
            services.AddScoped<IHttpRequest, HttpRequest>();
            services.AddScoped<ISagaCreditacoesWorker, SagaCreditacao>();
            services.AddScoped<ICreditacoesNotification, CreditacoesNotification>();
            services.AddTransient<ConsoleApplication>();
            return services;
        }
    }
}
