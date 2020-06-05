using CMA.ISMAI.Core;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Solutions.Creditacoes.UI.Models;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services.Interface;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
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
                using (var x = new System.Net.Http.HttpClient())
                {
                    var s = new System.Net.Http.HttpRequestMessage();
                    s.RequestUri = new Uri("http://mywebapi/Trello/GetCardStatus?cardId=132"); // ASP.NET 2.x
                    var response = await x.SendAsync(s);
                }
                HttpClient client = new HttpClient();
                var json = JsonConvert.SerializeObject(card);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage request = await client.PostAsync(BaseConfiguration.ReturnSettingsValue("Trello", "Uri"), stringContent);
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
