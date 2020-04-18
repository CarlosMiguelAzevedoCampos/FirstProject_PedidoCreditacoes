using CMA.ISMAI.IntegrationTests.Trello.Model;
using CMA.ISMAI.Solutions.Creditacoes.UI.Models;
using CMA.ISMAI.Trello.API;
using CMA.ISMAI.Trello.Domain.Events;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.IntegrationTests.Trello
{
    public class TrelloAPI_DeleteCard_IntegrationTest
    {
        [Trait("TrelloController API - Integration Tests", "Delete Card")]
        [Fact(DisplayName = "Card should be deleted")]
        public async Task TrelloController_IntegrationTest_CardShouldBeDeleted()
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);
            HttpClient client = testServer.CreateClient();
            string id = CreateCardIdAndReturnItAsync(client).Result;
            var response = await client.DeleteAsync(string.Format("Trello/DeleteCard?cardId={0}", id));
            var result = await response.Content.ReadAsStringAsync();
            ResponseSucess<CardHasBeenDeletedEvent> cardStatusIncompleted = JsonConvert.DeserializeObject<ResponseSucess<CardHasBeenDeletedEvent>>(result);
            Assert.True(cardStatusIncompleted.Success);
            Assert.Equal(cardStatusIncompleted.Data.CardId, id);
        }

        private async Task<string> CreateCardIdAndReturnItAsync(HttpClient client)
        {
            var myContent = new CardDto("Carlos Miguel Campos - Informática - ISMAI", DateTime.Now.AddDays(2), "Carlos Miguel Campos", 3,
                new List<string>() { "https://www.google.pt/?gws_rd=ssl" }, "ISMAI", "Informática", "Carlos Campos", true);
            var json = JsonConvert.SerializeObject(myContent);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync("Trello/AddCard", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            ResponseSucess<AddCardCompletedEvent> addCardCompletedEvent = JsonConvert.DeserializeObject<ResponseSucess<AddCardCompletedEvent>>(result);
            return addCardCompletedEvent.Data.Id;
        }
    }
}
