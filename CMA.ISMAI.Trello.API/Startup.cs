using CMA.ISMAI.Core;
using CMA.ISMAI.Core.Events.Store.Interface;
using CMA.ISMAI.Core.Events.Store.Service;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Logging.Service;
using CMA.ISMAI.Trello.Domain.CommandHandlers;
using CMA.ISMAI.Trello.Domain.EventHandlers;
using CMA.ISMAI.Trello.Domain.Interface;
using CMA.ISMAI.Trello.Engine.Automation;
using CMA.ISMAI.Trello.Engine.Interface;
using CMA.ISMAI.Trello.Engine.Service;
using CMA.ISMAI.Trello.FileReader.Interfaces;
using CMA.ISMAI.Trello.FileReader.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _currentEnvironment;
        public Startup(IWebHostEnvironment env)
        {
            _currentEnvironment = env;
            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(BaseConfiguration.ReturnSettingsValue("ElasticConfiguration", "Uri")))
               {
                   AutoRegisterTemplate = true,
                   IndexFormat = "TrelloISMAIAPI"
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
            services.AddScoped<ITrello, TrelloService>();
            services.AddScoped<IEngine, EngineService>();
            services.AddScoped<IEventStore, StoreEvent>();
            services.AddScoped<IFileReader, FileReaderService>();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
