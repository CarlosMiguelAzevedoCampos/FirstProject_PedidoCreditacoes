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
        [InlineData("", "Carlos Miguel Campos")]
        [InlineData(null, "Carlos Miguel Campos")]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("ISEP - Engenharia informática", "")]
        [InlineData("ISEP - Engenharia informática", null)]
        public async Task TrelloController_IntegrationTest_AddCard_ShouldFailTheCreationDueToNullOrEmtpyParameters(string name, string description)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            var myContent = new CardDto(name, DateTime.Now.AddDays(2), description);
            var json = JsonConvert.SerializeObject(myContent);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Trello", stringContent);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("Informática - ISMAI", "Carlos Miguel Campos")]
        [InlineData("ISEP - Engenharia informática", "Miguel Azevedo Silva")]
        public async Task TrelloController_IntegrationTest_AddCard_ShouldCreateANewCard(string name, string description)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            var myContent = new CardDto(name, DateTime.Now.AddDays(2), description);
            var json = JsonConvert.SerializeObject(myContent);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Trello", stringContent);
            Assert.True(response.IsSuccessStatusCode);
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
        [InlineData("23423423423")]
        [InlineData("1234422")]
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
    }
}
