using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.API.HealthCheck.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CMA.ISMAI.Trello.API.HealthCheck
{
    public class TrelloHealthCheck : IHealthCheck
    {
        private readonly ILog _log;
        private readonly IHttpRequest _httpRequest;
        public TrelloHealthCheck(ILog log,
                                  IHttpRequest httpRequest)
        {
            _log = log;
            this._httpRequest = httpRequest;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _httpRequest.MakeAnHttpRequest(ReturnTrelloUrl());
                return result.IsSuccessStatusCode ? HealthCheckResult.Healthy("The API is working fine!") :
                                                        HealthCheckResult.Unhealthy("The API is DOWN!");
            }
            catch(Exception ex)
            {
                _log.Fatal(ex.ToString());
                return HealthCheckResult.Unhealthy("The API is DOWN!");
            }
        }

        private string ReturnTrelloUrl()
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory()) // Directory where the json files are located
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();

            return configuration.GetSection("Trello").GetSection("Uri").Value;
        }
    }
}
