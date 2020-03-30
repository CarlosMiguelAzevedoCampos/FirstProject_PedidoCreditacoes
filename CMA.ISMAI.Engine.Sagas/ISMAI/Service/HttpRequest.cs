using CMA.ISMAI.Engine.Automation.Sagas.ISMAI.Interface;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Engine.ISMAI.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public async Task<string> CardPostAsync(CardDto card)
        {
            try
            {
                _log.Info($"CardPostAsync is being executed!, card Information - Board - {card.BoardId} - Description - {card.Description} - Name {card.Name}");

                var json = JsonConvert.SerializeObject(card);

                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                HttpResponseMessage request = await client.PostAsync("https://localhost:5001/Trello", stringContent);
                _log.Info($"CardPostAsync post request - Board - {card.BoardId} - Description - {card.Description} - Name {card.Name}");

                if (request.IsSuccessStatusCode)
                {
                    var response = request.Content.ReadAsStringAsync();
                    Response<AddCardCompletedEvent> addCardCompletedEvent = JsonConvert.DeserializeObject<Response<AddCardCompletedEvent>>(response.Result);
                    _log.Info($"CardPostAsync post request - Done!! - CardId - {addCardCompletedEvent.Data.Id} - Board - {card.BoardId} - Description - {card.Description} - Name {card.Name}");
                    return addCardCompletedEvent.Data.Id;
                }
                _log.Info($"CardPostAsync post request - Failed!! - Board - {card.BoardId} - Description - {card.Description} - Name {card.Name}");

                return Task.FromResult(string.Empty).Result;
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
                _log.Info($"CardStateAsync is being executed!, card Information - Id {cardId}");

                var response = await client.GetAsync(string.Format("https://localhost:5001/Trello/GetCardStatus?cardId={0}", cardId));
                _log.Info($"CardStateAsync is getting information!, card Information - Id {cardId}");

                if (response.IsSuccessStatusCode)
                {
                    var readAsStringAsync = response.Content.ReadAsStringAsync();
                    Response<GetCardStatusResult> cardStatus = JsonConvert.DeserializeObject<Response<GetCardStatusResult>>(readAsStringAsync.Result);
                    _log.Info($"CardStateAsync done!, card Information - Id {cardId} - Status {cardStatus.Data.MessageType}");

                    return cardStatus.Data.MessageType == "CardStatusCompletedEvent";
                }
                _log.Info($"CardStateAsync failed!, card Information - Id {cardId}");

                return false;
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.ToString());
                return false;
            }
        }

        public async Task<List<string>> GetCardAttachments(string cardId)
        {
            try
            {
                _log.Info($"GetCardAttachments is being executed!, card Information - Id {cardId}");

                var response = await client.GetAsync(string.Format("https://localhost:5001/Trello/GetCardAttachments?cardId={0}"
                    , cardId));
                _log.Info($"GetCardAttachments is getting information!, card Information - Id {cardId}");

                if (response.IsSuccessStatusCode)
                {
                    var readAsStringAsync = response.Content.ReadAsStringAsync();
                    Response<ReturnCardAttachmentsEvent> cardStatus = JsonConvert.DeserializeObject<Response<ReturnCardAttachmentsEvent>>(readAsStringAsync.Result);
                    _log.Info($"GetCardAttachments done!, card Information - Id {cardId} - Event {cardStatus.Data.MessageType}");

                    return cardStatus.Data.Attachments;
                }
                _log.Info($"CardStateAsync failed!, card Information - Id {cardId}");

                return new List<string>();
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.ToString());
                return new List<string>();
            }
        }
    }
}
