using CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Interface;
using CMA.ISMAI.Logging.Interface;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Service
{
    public class HttpRequest : IHttpRequest
    {
        private readonly HttpClient client;
        private readonly ILog _log;

        public HttpRequest(ILog log)
        {
            client = new HttpClient();
            this._log = log;
        }
        public async Task<bool> CardStateAsync(string cardId)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("");
                HttpStatusCode result = HttpStatusCode.BadRequest;
                if (response.IsSuccessStatusCode)
                {
                    result = response.StatusCode;
                }
                return result == HttpStatusCode.OK;
            }
            catch(Exception ex)
            {
                _log.Fatal(ex.ToString());
                return false;
            }
        }
    }
}
