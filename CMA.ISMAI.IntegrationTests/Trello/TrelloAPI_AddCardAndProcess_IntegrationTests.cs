using CMA.ISMAI.IntegrationTests.Trello.Model;
using CMA.ISMAI.Trello.API;
using CMA.ISMAI.Trello.API.Model;
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
    public class TrelloAPI_AddCardAndProcess_IntegrationTests
    {
        [Trait("TrelloController API - Integration Tests", "Add Card and Process")]
        [Theory(DisplayName = "Card shouldn't be added because of null parameters")]
        [InlineData("", "Carlos Miguel Campos", -1, "ISMAI", "Informática", "", true)]
        [InlineData(null, "Carlos Miguel Campos", 1, "ISMAI", "Informática", "", false)]
        [InlineData(null, null, 2, "ISMAI", "Informática", "Description", true)]
        [InlineData("", "", 1, "ISMAI", "", "", true)]
        [InlineData("ISEP - Engenharia informática", "", 0, "ISMAI", "Informática", "", false)]
        [InlineData("ISEP - Engenharia informática", null, -1, "ISMAI", "Informática", "", false)]
        public async Task TrelloController_IntegrationTest_AddCardAndProcess_ShouldFailTheCreation_DueToNullOrEmtpyParameters(string name, string description, int boardId, string instituteName, string courseName, string studentName, bool isCet)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            var myContent = new CardDto(name, DateTime.Now.AddDays(2), description, boardId, new List<string>(), instituteName, courseName, studentName, isCet);
            var json = JsonConvert.SerializeObject(myContent);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Trello/AddCardAndProcess", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            ResponseErrors<AddCardFailedEvent> addCardFailedEvent = JsonConvert.DeserializeObject<ResponseErrors<AddCardFailedEvent>>(result);
            Assert.False(addCardFailedEvent.Success);
            Assert.Equal("AddCardFailedEvent", addCardFailedEvent.Errors.MessageType);
            Assert.True(addCardFailedEvent.Errors.DomainNotifications.Count > 0);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Trait("TrelloController API - Integration Tests", "Add Card and Process")]
        [Theory(DisplayName = "Card shouldn't be added because of bad attachments list")]
        [InlineData("Card And Process - Carlos Miguel Campos- Informática - ISMAI", "Carlos Miguel Campos", 1, "ISMAI", "Informática", "Carlos Campos", true)]
        [InlineData("Card And Process - Carlos Miguel Campos - Informática - ISMAI", "Carlos Miguel Campos", 1, "ISMAI", "Informática", "Carlos Campos", false)]
        public async Task TrelloController_IntegrationTest_AddCardAndProcess_ShouldFailTheCreation_DueToNullFilesUrl(string name, string description, int boardId, string instituteName, string courseName, string studentName, bool isCet)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();
            TestServer testServer = new TestServer(builder);
            HttpClient client = testServer.CreateClient();

            var myContent = new CardDto(name, DateTime.Now.AddDays(2), description, boardId, null, instituteName, courseName, studentName, isCet);
            var json = JsonConvert.SerializeObject(myContent);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync("Trello/AddCardAndProcess", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            
            ResponseErrors<AddCardFailedEvent> addCardFailedEvent = JsonConvert.DeserializeObject<ResponseErrors<AddCardFailedEvent>>(result);
            Assert.False(addCardFailedEvent.Success);
            Assert.True(addCardFailedEvent.Errors.DomainNotifications.Count == 1);
            Assert.Equal("AddCardFailedEvent", addCardFailedEvent.Errors.MessageType);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Trait("TrelloController API - Integration Tests", "Add Card and Process")]
        [Theory(DisplayName = "Card shouldn't be added because of non existing board Id")]
        [InlineData("Card And Process - Carlos Miguel Campos - Informática - ISMAI", "Carlos Miguel Campos", -1, "ISMAI", "Informática", "Carlos Campos", false)]
        [InlineData("Card And Process - Carlos Miguel Campos - Informática - ISMAI", "Carlos Miguel Campos", -1, "ISMAI", "Informática", "Carlos Campos", true)]
        public async Task TrelloController_IntegrationTest_AddCardAndProcess_ShouldFailBecauseOfNone_ExistingBoardId(string name, string description, int boardId, string instituteName, string courseName, string studentName, bool isCet)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            var myContent = new CardDto(name, DateTime.Now.AddDays(2), description, boardId, new List<string>() { "https://www.google.pt/?gws_rd=ssl" }, instituteName, courseName, studentName, isCet);
            var json = JsonConvert.SerializeObject(myContent);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync("Trello/AddCardAndProcess", stringContent);
            var result = await response.Content.ReadAsStringAsync();

            ResponseErrors<AddCardFailedEvent> addCardFailedEvent = JsonConvert.DeserializeObject<ResponseErrors<AddCardFailedEvent>>(result);
            Assert.False(addCardFailedEvent.Success);
            Assert.True(addCardFailedEvent.Errors.DomainNotifications.Count == 1);
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal("AddCardFailedEvent", addCardFailedEvent.Errors.MessageType);
        }

        [Trait("TrelloController API - Integration Tests", "Add Card and Process")]
        [Theory(DisplayName = "Card should be created")]
        [InlineData("Card And Process - Carlos Miguel Campos - Informática - ISMAI", "Carlos Miguel Campos", 3, "ISMAI", "Informática", "Carlos Campos", true)]
        [InlineData("Card And Process - Carlos Miguel Campos - Informática - ISMAI", "Carlos Miguel Campos", 3, "ISMAI", "Informática", "Carlos Campos", false)]
        public async Task TrelloController_IntegrationTest_AddCardAndProcess_ShouldCreateANewCard(string name, string description, int boardId, string instituteName, string courseName, string studentName, bool isCet)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();
            TestServer testServer = new TestServer(builder);
            HttpClient client = testServer.CreateClient();
          
            var myContent = new CardDto(name, DateTime.Now.AddDays(2), description, boardId, new List<string>() { "https://www.google.pt/?gws_rd=ssl" }, instituteName, courseName, studentName, isCet);
            var json = JsonConvert.SerializeObject(myContent);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync("Trello/AddCardAndProcess", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            
            ResponseSucess<AddCardCompletedEvent> addCardCompletedEvent = JsonConvert.DeserializeObject<ResponseSucess<AddCardCompletedEvent>>(result);
            Assert.Equal("AddCardCompletedEvent", addCardCompletedEvent.Data.MessageType);
            Assert.NotEmpty(addCardCompletedEvent.Data.Id);
            Assert.True(addCardCompletedEvent.Success);
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
