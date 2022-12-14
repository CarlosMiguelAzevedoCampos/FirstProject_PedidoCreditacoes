using CMA.ISMAI.Core;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Logging.Service;
using CMA.ISMAI.Sagas.Domain.Interface;
using CMA.ISMAI.Sagas.Domain.Service.Creditacao;
using CMA.ISMAI.Sagas.Domain.Service.Saga;
using CMA.ISMAI.Sagas.Service;
using CMA.ISMAI.Sagas.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Threading.Tasks;

namespace CMA.ISMAI.Sagas.UI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hostBuilder = new HostBuilder();
            var services = ConfigureServices();

            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddSerilog();

            serviceProvider.GetRequiredService<ConsoleApplication>().Run();
            await hostBuilder.RunConsoleAsync();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection  services = new ServiceCollection();
            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext() 
               .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(BaseConfiguration.ReturnSettingsValue("ElasticConfiguration", "Uri")))
               {
                   AutoRegisterTemplate = true,
                   IndexFormat = "TrelloISMAISaga"
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
