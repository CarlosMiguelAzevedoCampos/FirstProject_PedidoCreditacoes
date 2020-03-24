using CMA.ISMAI.Trello.API;
using CMA.ISMAI.Trello.API.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.IntegrationTests
{
    public class TrelloAPI_IntegrationTests
    {
        [Theory]
        [InlineData("", "Carlos Miguel Campos",-1)]
        [InlineData(null, "Carlos Miguel Campos",1)]
        [InlineData(null, null,2)]
        [InlineData("", "",1)]
        [InlineData("ISEP - Engenharia informática", "",0)]
        [InlineData("ISEP - Engenharia informática", null,-1)]
        public async Task TrelloController_IntegrationTest_AddCard_ShouldFailTheCreationDueToNullOrEmtpyParameters(string name, string description, int boardId)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            var myContent = new CardDto(name, DateTime.Now.AddDays(2), description, boardId);
            var json = JsonConvert.SerializeObject(myContent);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Trello", stringContent);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("ISEP - Engenharia informática", "Miguel Azevedo Silva", 0)]
        [InlineData("Informática - ISMAI", "Carlos Miguel Campos", 1)]
        [InlineData("ISEP - Engenharia informática", "Miguel Azevedo Silva", 2)]
        public async Task TrelloController_IntegrationTest_AddCard_ShouldCreateANewCard(string name, string description, int boardId)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            var myContent = new CardDto(name, DateTime.Now.AddDays(2), description, boardId);
            var json = JsonConvert.SerializeObject(myContent);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Trello", stringContent);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("Informática - ISMAI", "Carlos Miguel Campos")]
        [InlineData("ISEP - Engenharia informática", "Miguel Azevedo Silva")]
        public async Task TrelloController_IntegrationTest_AddCard_ShouldFailBecauseOfNoneExistingBoardId(string name, string description)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            var myContent = new CardDto(name, DateTime.Now.AddDays(2), description, -1);
            var json = JsonConvert.SerializeObject(myContent);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Trello", stringContent);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task TrelloController_IntegrationTest_GetCardStatus_ShouldReturnFailOnCardValidation(string id)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();

            var response = await client.GetAsync(string.Format("Trello?id={0}", id));
            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("5e6e8509dddc96602b7ac32d")]
        public async Task TrelloController_IntegrationTest_GetCardStatus__ShouldReturnTheCardValidation(string id)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();

            var response = await client.GetAsync(string.Format("Trello?id={0}", id));
            Assert.True(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("12")]
        public async Task TrelloController_IntegrationTest_GetCardStatus__ShouldReturnBadRequest_UnknowCard(string id)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();

            var response = await client.GetAsync(string.Format("Trello?id={0}", id));
            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("5e6e84b7b878a307a3e9f6ca")]
        public async Task TrelloController_IntegrationTest_GetCardStatus__ShouldReturnOkRequest_ActiveCard(string id)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();

            var response = await client.GetAsync(string.Format("Trello?id={0}", id));
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
