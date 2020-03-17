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
        [Theory]
        [InlineData("", "INF_34324", true)]
        [InlineData("", "", false)]
        [InlineData(null, null, true)]
        [InlineData("ISMAI", "", true)]
        public async Task EngineController_IntegrationTest_UploadWorkFlowToTheAutomation_ShouldFailBecauseOfEmptyOrNullParameters(string workflowName, string processName, bool isCet)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            var myContent = new DeployDto(workflowName, processName, isCet);
            var json = JsonConvert.SerializeObject(myContent);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Engine", stringContent);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("ISMAI", "INF_34324", true)]
        [InlineData("ISEP", "EINF_2334324", false)]
        public async Task EngineController_IntegrationTest_UploadWorkFlowToTheAutomationEngineAndStartTheWorkFlow(string workflowName, string processName, bool isCet)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            var myContent = new DeployDto(workflowName, processName, isCet);
            var json = JsonConvert.SerializeObject(myContent);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Engine", stringContent);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task EngineController_IntegrationTest_UploadWorkFlowToTheAutomationShouldFailBecauseNullDto()
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            var json = JsonConvert.SerializeObject(null);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Engine", stringContent);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task EngineController_IntegrationTest_DeleteWorkFlowDeploy_ShouldFailBecauseOfEmptyOrNullsDeployementId(string id)
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
                Content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("e9a0a61e-66d8-11ea-a7fb-0242ac130002")]
        public async Task EngineController_IntegrationTest_DeleteWorkFlowDeployShouldPass(string id)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();


            var response = await client.DeleteAsync(string.Format("Engine?id={0}", id));
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
