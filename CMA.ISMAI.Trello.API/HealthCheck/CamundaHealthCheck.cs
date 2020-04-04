using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.API.HealthCheck.Interface;
using CMA.ISMAI.Trello.Settings;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CMA.ISMAI.Trello.API.HealthCheck
{
    public class CamundaHealthCheck : IHealthCheck
    {
        private readonly ILog _log;
        private readonly IHttpRequest _httpRequest;
        public CamundaHealthCheck(ILog log,
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
                var result = await _httpRequest.MakeAnHttpRequest(SettingsReader.ReturnKey("CamundaHealth", "Uri"));
                return result.IsSuccessStatusCode ? HealthCheckResult.Healthy("The API is working fine!") :
                                                        HealthCheckResult.Unhealthy("The API is DOWN!");
            }
            catch(Exception ex)
            {
                _log.Fatal(ex.ToString());
                return HealthCheckResult.Unhealthy("The API is DOWN!");
            }
        }
    }
}
