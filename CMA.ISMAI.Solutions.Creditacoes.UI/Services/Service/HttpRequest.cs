using CMA.ISMAI.Solutions.Creditacoes.UI.Models;
using CMA.ISMAI.Solutions.Creditacoes.UI.Services.Interface;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CMA.ISMAI.Solutions.Creditacoes.UI.Services.Service
{
    public class HttpRequest : IHttpRequest
    {
        public async Task<bool> CreateNewWorkFlow(DeployDto deploy)
        {
            HttpClient client = new HttpClient();

            var json = JsonConvert.SerializeObject(deploy);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            HttpResponseMessage request = await client.PostAsync("https://localhost:5002/Engine", stringContent);
            return request.IsSuccessStatusCode;
        }

        public async Task<string> PostNewCardAsync(CardDto card)
        {
            HttpClient client = new HttpClient();

            var json = JsonConvert.SerializeObject(card);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            HttpResponseMessage request = await client.PostAsync("https://localhost:5001/Trello", stringContent);
            if (request.IsSuccessStatusCode)
            {
                var response = request.Content.ReadAsStringAsync();
                Response<AddCardCompletedEvent> addCardCompletedEvent = JsonConvert.DeserializeObject<Response<AddCardCompletedEvent>>(response.Result);
                return addCardCompletedEvent.Data.Id;
            }
            return string.Empty;
        }
    }
}
