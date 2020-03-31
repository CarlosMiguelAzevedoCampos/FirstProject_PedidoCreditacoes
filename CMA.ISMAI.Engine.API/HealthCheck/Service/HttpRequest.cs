using CMA.ISMAI.Engine.API.HealthCheck.Interface;
using CMA.ISMAI.Logging.Interface;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CMA.ISMAI.Engine.API.HealthCheck.Service
{
    public class HttpRequest : IHttpRequest
    {
        private readonly ILog _log;
        public HttpRequest(ILog log)
        {
            _log = log;
        }
        public async Task<HttpResponseMessage> MakeAnHttpRequest(string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                var result = await client.GetAsync(url);
                return result;
            }
            catch(Exception ex)
            {
                _log.Fatal(ex.ToString());
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}
