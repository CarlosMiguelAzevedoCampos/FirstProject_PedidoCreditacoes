using CMA.ISMAI.Engine.API;
using CMA.ISMAI.Engine.API.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("cet", isCet);
            var myContent = new DeployDto(workflowName, processName, parameters);
            var json = JsonConvert.SerializeObject(myContent);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Engine", stringContent);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task EngineController_IntegrationTest_UploadWorkFlowToTheAutomation_ShouldFailBecauseOfNullParameters()
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            var myContent = new DeployDto("ISMAI", "Process_xs747", null);
            var json = JsonConvert.SerializeObject(myContent);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Engine", stringContent);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("ISMAI", "INF_34324", true)]
        [InlineData("ISMAI", "EINF_2334324", false)]
        public async Task EngineController_IntegrationTest_UploadWorkFlowToTheAutomationEngineAndStartTheWorkFlow(string workflowName, string processName, bool isCet)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("cet", isCet);
            var myContent = new DeployDto(workflowName, processName, parameters);
            var json = JsonConvert.SerializeObject(myContent);

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Engine", stringContent);
            Assert.True(response.IsSuccessStatusCode);
        }


        [Theory]
        [InlineData("ISEP", "Process_00kjdw0")]
        [InlineData("FEUP", "Process_00kjd12")]
        public async Task EngineService_StartWorkFlow_ShouldReturnBadStatusBecauseOfNonExistingWorkFlow(string workflowName, string processName)
        {
            var builder = new WebHostBuilder()
                        .UseEnvironment("Development")
                        .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            var json = JsonConvert.SerializeObject(new DeployDto(workflowName, processName, new Dictionary<string, object>()));

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await client.PostAsync("Engine", stringContent);
            Assert.False(response.IsSuccessStatusCode);
        }
    }
}
