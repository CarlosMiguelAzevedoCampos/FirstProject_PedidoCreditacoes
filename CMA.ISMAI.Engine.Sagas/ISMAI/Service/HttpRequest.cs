using CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Interface;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Engine.ISMAI.Model;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
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

        public async Task<string> CardPostAsync(string name, DateTime dueTime, int boardId, string description)
        {
            try
            {
                var myContent = new CardDto(name, DateTime.Now.AddDays(2), boardId, description);
                var json = JsonConvert.SerializeObject(myContent);

                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                HttpResponseMessage request = await client.PostAsync("https://localhost:5001/Trello", stringContent);
                if (request.IsSuccessStatusCode)
                {
                    var response = request.Content.ReadAsStringAsync();
                    Response addCardCompletedEvent = JsonConvert.DeserializeObject<Response>(response.Result);
                    return addCardCompletedEvent.Data.Id;
                }
                return Task.FromResult(Guid.NewGuid().ToString()).Result;
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.ToString());
                return string.Empty;
            }
        }

        public async Task<bool> CardStateAsync(string cardId)
        {
            try
            {
                var response = await client.GetAsync(string.Format("https://localhost:5001/Trello?id={0}", cardId));
                var readAsStringAsync = response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.ToString());
                return false;
            }
        }
    }
}
