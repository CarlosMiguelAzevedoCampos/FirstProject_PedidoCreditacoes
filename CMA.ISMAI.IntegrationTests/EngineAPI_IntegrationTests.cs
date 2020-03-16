using CMA.ISMAI.Engine.API;
using CMA.ISMAI.Engine.API.Model;
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
    public class EngineAPI_IntegrationTests
    {
        [Fact]
        public async Task EngineController_IntegrationTestUploadWorkFlowToTheAutomationShouldFailAndReturnBadRequest()
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            var myContent = new DeployDto("", Guid.NewGuid().ToString(), true);
            var json = JsonConvert.SerializeObject(myContent);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Engine", stringContent);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task EngineController_IntegrationTestUploadWorkFlowToTheAutomationEngineAndStartTheCETWorkFlow()
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            var myContent = new DeployDto("ISMAI", Guid.NewGuid().ToString(), true);
            var json = JsonConvert.SerializeObject(myContent);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Engine", stringContent);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task EngineController_IntegrationTestUploadWorkFlowToTheAutomationEngineAndStartTheNonCETWorkFlow()
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            var myContent = new DeployDto("ISMAI", Guid.NewGuid().ToString(), false);
            var json = JsonConvert.SerializeObject(myContent);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Engine", stringContent);
            Assert.True(response.IsSuccessStatusCode);
        }
        
        [Fact]
        public async Task EngineController_IntegrationTest_DeleteWorkFlowDeployShouldFailBecauseOfEmptyDeployementId()
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("http://localhost/Engine"),
                Content = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task EngineController_IntegrationTest_DeleteWorkFlowDeployShouldPass()
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            

            var response = await client.DeleteAsync("Engine?id=e9a0a61e-66d8-11ea-a7fb-0242ac130002");
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
