using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Solutions.Creditacoes.UI.Models;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services.Interface;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CMA.ISMAI.Solutions.Creditacoes.UI.Services.Service
{
    public class HttpRequest : IHttpRequest
    {

        private readonly ILog _log;
        public HttpRequest(ILog log)
        {
            _log = log;
        }

        public async Task<bool> PostNewCardAsync(CardDto card)
        {
            try
            {
                HttpClient client = new HttpClient();

                var json = JsonConvert.SerializeObject(card);

                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage request = await client.PostAsync("https://localhost:5001/Trello", stringContent);
                return request.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.ToString());
            }
            return false;
        }
    }
}
