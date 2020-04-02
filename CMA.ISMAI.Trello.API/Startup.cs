using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Logging.Service;
using CMA.ISMAI.Trello.API.HealthCheck;
using CMA.ISMAI.Trello.API.HealthCheck.Interface;
using CMA.ISMAI.Trello.Domain.CommandHandlers;
using CMA.ISMAI.Trello.Domain.EventHandlers;
using CMA.ISMAI.Trello.Domain.Interface;
using CMA.ISMAI.Trello.Engine.Automation;
using CMA.ISMAI.Trello.Engine.Interface;
using CMA.ISMAI.Trello.Engine.Service;
using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;

namespace CMA.ISMAI.Trello.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", reloadOnChange: true, optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            var elasticUri = Configuration["ElasticConfiguration:Uri"];

            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
               {
                   AutoRegisterTemplate = true,
               })
            .CreateLogger();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            InitializeDependecyInjection(services);
            services.AddHealthChecks().AddRabbitMQ(Configuration["RabbitMq:Uri"], null, "RabbitMQ")
            .AddCheck<CamundaHealthCheck>("Camunda BPM").AddCheck<TrelloHealthCheck>("Trello");
            services.AddHealthChecksUI();
        }

        private void InitializeDependecyInjection(IServiceCollection services)
        {
            services.AddScoped<ILog, LoggingService>();
            services.AddScoped<ITrello, TrelloService>();
            services.AddScoped<IEngine, EngineService>();
            services.AddScoped<IHttpRequest, HttpRequest>();
            // Domain - Commands
            services.AddScoped<ICardCommandHandler, CardCommandHandler>();
            // Domain - Events
            services.AddScoped<ICardEventHandler, CardEventHandler>();
            services.AddScoped<IEngineEventHandler, EngineEventHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            loggerFactory.AddSerilog();

            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(delegate (Options options)
            {
                options.UIPath = "/hc-ui";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
