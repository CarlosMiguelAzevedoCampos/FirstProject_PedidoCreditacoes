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
        [Fact]
        public async Task TrelloController_IntegrationTest_ShouldFailTheCreationDueToBadParameters()
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            var myContent = new CardDto("", DateTime.Now.AddDays(2), Guid.NewGuid().ToString());
            var json = JsonConvert.SerializeObject(myContent);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Trello", stringContent);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task TrelloController_IntegrationTest_ShouldCreateANewCard()
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            var myContent = new CardDto(Guid.NewGuid().ToString(), DateTime.Now.AddDays(2), Guid.NewGuid().ToString());
            var json = JsonConvert.SerializeObject(myContent);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Trello", stringContent);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task TrelloController_IntegrationTest_ShouldFailBecauseOfNullDto()
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            var json = JsonConvert.SerializeObject(null);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Trello", stringContent);
            Assert.False(response.IsSuccessStatusCode);
        }


        [Fact]
        public async Task TrelloController_IntegrationTest_ShouldReturnFailOnCardValidation()
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();

            var response = await client.GetAsync("Trello");
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task TrelloController_IntegrationTest_ShouldReturnTheCardValidation()
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();

            var response = await client.GetAsync("Trello?id=5e6e8509dddc96602b7ac32d");
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
