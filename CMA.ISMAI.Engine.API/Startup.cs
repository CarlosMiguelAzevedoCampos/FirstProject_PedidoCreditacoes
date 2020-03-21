using CMA.ISMAI.Automation.Interface;
using CMA.ISMAI.Automation.Service;
using CMA.ISMAI.Engine.Domain.CommandHandlers;
using CMA.ISMAI.Engine.Domain.EventHandlers;
using CMA.ISMAI.Engine.Domain.Interface;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Logging.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;

namespace CMA.ISMAI.Engine.API
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
        }

        private void InitializeDependecyInjection(IServiceCollection services)
        {
            services.AddScoped<ILog, LoggingService>();
            services.AddScoped<IEngine, EngineService>();
            // Domain - Commands
            services.AddScoped<IWorkflowCommandHandler, WorkFlowCommandHandler>();
            // Domain - Events
            services.AddScoped<IWorkflowEventHandler, WorkFlowEventHandler>();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
