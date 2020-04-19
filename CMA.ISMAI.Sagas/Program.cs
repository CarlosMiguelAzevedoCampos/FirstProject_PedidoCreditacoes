using CMA.ISMAI.Core;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Logging.Service;
using CMA.ISMAI.Sagas.Domain.Interface;
using CMA.ISMAI.Sagas.Domain.Service.Creditacao;
using CMA.ISMAI.Sagas.Domain.Service.Saga;
using CMA.ISMAI.Sagas.Service;
using CMA.ISMAI.Sagas.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;

namespace CMA.ISMAI.Sagas.UI
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
            IServiceCollection  services = new ServiceCollection();
            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext() 
               .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(BaseConfiguration.ReturnSettingsValue("ElasticConfiguration", "Uri")))
               {
                   AutoRegisterTemplate = true,
               })
            .CreateLogger();

            services.AddLogging();
            services.AddScoped<ILog, LoggingService>();
            services.AddScoped<ICreditacaoDomainService, CreditacaoDomainService>();
            services.AddScoped<ISagaService, CreditacaoService>();
            services.AddScoped<IHttpRequest, HttpRequest>();
            services.AddScoped<ISagaDomainService, SagaDomainService>();
            services.AddScoped<ICreditacaoWithCardCreationDomainService, CreditacaoWithCardCreationDomainService>();
            services.AddScoped<ICreditacaoWithNoCardCreationDomainService, CreditacaoWithNoCardCreationService>();
            services.AddScoped<ICreditacaoFinalStepDomainService, CreditacaoFinalStepDomainService>();
            services.AddScoped<ISagaNotification, CreditacaoNotificationService>();
            services.AddScoped<ITaskProcessingDomainService, TaskProcessingDomainService>();
            services.AddTransient<ConsoleApplication>();
            return services;
        }
    }
}
