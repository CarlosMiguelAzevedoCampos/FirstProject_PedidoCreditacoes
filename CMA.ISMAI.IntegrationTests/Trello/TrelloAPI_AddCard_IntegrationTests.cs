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
    public class TrelloAPI_AddCard_IntegrationTests
    {
        [Trait("TrelloController API - Integration Tests", "Add Card")]
        [Theory(DisplayName = "Card shouldn't be added because of null parameters")]
        [InlineData("", "Carlos Miguel Campos", -1, "ISMAI", "Informática", "", true)]
        [InlineData(null, "Carlos Miguel Campos", 1, "ISMAI", "Informática", "", false)]
        [InlineData(null, null, 4, "ISMAI", "Informática", "Description", true)]
        [InlineData("", "", 7, "ISMAI", "", "", true)]
        [InlineData("ISEP - Engenharia informática", "", 5, "ISMAI", "Informática", "", false)]
        [InlineData("ISEP - Engenharia informática", null, -1, "ISMAI", "Informática", "", false)]
        public async Task TrelloController_IntegrationTest_AddCard_ShouldFailTheCreation_DueToNullOrEmtpyParameters(string name, string description, int boardId, string instituteName, string courseName, string studentName, bool IsCetOrOtherCondition)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            var myContent = new CardDto(name, DateTime.Now.AddDays(2), description, boardId, new List<string>(), instituteName, courseName, studentName, IsCetOrOtherCondition);
            var json = JsonConvert.SerializeObject(myContent);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Trello/AddCard", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            ResponseErrors<AddCardFailedEvent> addCardFailedEvent = JsonConvert.DeserializeObject<ResponseErrors<AddCardFailedEvent>>(result);
            Assert.False(addCardFailedEvent.Success);
            Assert.True(addCardFailedEvent.Errors.DomainNotifications.Count > 0);
            Assert.Equal("AddCardFailedEvent", addCardFailedEvent.Errors.MessageType);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Trait("TrelloController API - Integration Tests", "Add Card")]
        [Theory(DisplayName = "Card shouldn't be added because of bad attachments list")]
        [InlineData("Carlos Miguel Campos - Informática - ISMAI", "Carlos Miguel Campos", 3, "ISMAI", "Informática", "Carlos Campos", true)]
        [InlineData("Carlos Miguel Campos - Informática - ISMAI", "Carlos Miguel Campos", 3, "ISMAI", "Informática", "Carlos Campos", false)]
        public async Task TrelloController_IntegrationTest_AddCard_ShouldFailTheCreation_DueToNullFilesUrl(string name, string description, int boardId, string instituteName, string courseName, string studentName, bool IsCetOrOtherCondition)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();
            TestServer testServer = new TestServer(builder);
            HttpClient client = testServer.CreateClient();

            var myContent = new CardDto(name, DateTime.Now.AddDays(2), description, boardId, null, instituteName, courseName, studentName, IsCetOrOtherCondition);
            var json = JsonConvert.SerializeObject(myContent);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync("Trello/AddCard", stringContent);
            var result = await response.Content.ReadAsStringAsync();

            ResponseErrors<AddCardFailedEvent> addCardFailedEvent = JsonConvert.DeserializeObject<ResponseErrors<AddCardFailedEvent>>(result);
            Assert.False(addCardFailedEvent.Success);
            Assert.Equal("AddCardFailedEvent", addCardFailedEvent.Errors.MessageType);
            Assert.True(addCardFailedEvent.Errors.DomainNotifications.Count == 1);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Trait("TrelloController API - Integration Tests", "Add Card")]
        [Theory(DisplayName = "Card shouldn't be added because of non existing board Id")]
        [InlineData("Carlos Miguel Campos - Informática - ISMAI", "Carlos Miguel Campos", -1, "ISMAI", "Informática", "Carlos Campos", false)]
        [InlineData("Carlos Miguel Campos - Informática - ISMAI", "Carlos Miguel Campos", -1, "ISMAI", "Informática", "Carlos Campos", true)]
        public async Task TrelloController_IntegrationTest_AddCard_ShouldFailBecauseOfNone_ExistingBoardId(string name, string description, int boardId, string instituteName, string courseName, string studentName, bool IsCetOrOtherCondition)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);
            HttpClient client = testServer.CreateClient();
            var myContent = new CardDto(name, DateTime.Now.AddDays(2), description, boardId, new List<string>() { "https://www.google.pt/?gws_rd=ssl" }, instituteName, courseName, studentName, IsCetOrOtherCondition);
            var json = JsonConvert.SerializeObject(myContent);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync("Trello/AddCard", stringContent);
            var result = await response.Content.ReadAsStringAsync();

            ResponseErrors<AddCardFailedEvent> addCardFailedEvent = JsonConvert.DeserializeObject<ResponseErrors<AddCardFailedEvent>>(result);
            Assert.False(addCardFailedEvent.Success);
            Assert.True(addCardFailedEvent.Errors.DomainNotifications.Count == 1);
            Assert.Equal("AddCardFailedEvent", addCardFailedEvent.Errors.MessageType);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Trait("TrelloController API - Integration Tests", "Add Card")]
        [Theory(DisplayName = "Card should be created")]
        [InlineData("Carlos Miguel Campos - Informática - ISMAI", "Carlos Miguel Campos", 3, "ISMAI", "Informática", "Carlos Campos", true)]
        [InlineData("Carlos Miguel Campos - Informática - ISMAI", "Carlos Miguel Campos", 3, "ISMAI", "Informática", "Carlos Campos", false)]
        public async Task TrelloController_IntegrationTest_AddCard_ShouldCreateANewCard(string name, string description, int boardId, string instituteName, string courseName, string studentName, bool IsCetOrOtherCondition)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);
            HttpClient client = testServer.CreateClient();

            var myContent = new CardDto(name, DateTime.Now.AddDays(2), description, boardId, new List<string>() { "https://www.google.pt/?gws_rd=ssl" }, instituteName, courseName, studentName, IsCetOrOtherCondition);
            var json = JsonConvert.SerializeObject(myContent);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync("Trello/AddCard", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            ResponseSucess<AddCardCompletedEvent> addCardCompletedEvent = JsonConvert.DeserializeObject<ResponseSucess<AddCardCompletedEvent>>(result);
            Assert.NotEmpty(addCardCompletedEvent.Data.Id);
            Assert.Equal("AddCardCompletedEvent", addCardCompletedEvent.Data.MessageType);
            Assert.True(addCardCompletedEvent.Success);
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
