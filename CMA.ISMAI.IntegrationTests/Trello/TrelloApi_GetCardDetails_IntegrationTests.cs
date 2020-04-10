using CMA.ISMAI.IntegrationTests.Trello.Model;
using CMA.ISMAI.Trello.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.IntegrationTests.Trello
{
    public class TrelloApi_GetCardDetails_IntegrationTests
    {
        [Trait("TrelloController API - Integration Tests", "Get card Details")]
        [Theory(DisplayName = "Get card status should fail because of empty card Id")]
        [InlineData("")]
        public async Task TrelloController_IntegrationTest_GetCardStatus_ShouldFail_EmptyCardId(string id)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();
            TestServer testServer = new TestServer(builder);
            HttpClient client = testServer.CreateClient();

            var response = await client.GetAsync(string.Format("Trello/GetCardStatus?cardId={0}", id));
            Assert.False(response.IsSuccessStatusCode);
        }

        [Trait("TrelloController API - Integration Tests", "Get card Details")]
        [Theory(DisplayName = "Get card status should return card status incompleted")]
        [InlineData("5e8a3eb3a6ef1f3eed24b319")]
        public async Task TrelloController_IntegrationTest_GetCardStatus__ShouldReturnOkRequest_ActiveCard(string id)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();
            TestServer testServer = new TestServer(builder);
            HttpClient client = testServer.CreateClient();

            var response = await client.GetAsync(string.Format("Trello/GetCardStatus?cardId={0}", id));
            var result = await response.Content.ReadAsStringAsync();

            ResponseSucess<CardStatusIncompletedEvent> cardStatusIncompleted = JsonConvert.DeserializeObject<ResponseSucess<CardStatusIncompletedEvent>>(result);
            Assert.True(cardStatusIncompleted.Success);
            Assert.NotEmpty(cardStatusIncompleted.Data.Id);
            Assert.Equal("CardStatusIncompletedEvent", cardStatusIncompleted.Data.MessageType);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Trait("TrelloController API - Integration Tests", "Get card Details")]
        [Theory(DisplayName = "Get card status should return card status completed")]
        [InlineData("5e8a3eb3a6ef1f3eed24b319")]
        public async Task TrelloController_IntegrationTest_GetCardStatus__ShouldReturnOkStatus_CompletedCard(string id)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();
            TestServer testServer = new TestServer(builder);
            HttpClient client = testServer.CreateClient();

            var response = await client.GetAsync(string.Format("Trello/GetCardStatus?cardId={0}", id));
            var result = await response.Content.ReadAsStringAsync();

            ResponseSucess<CardStatusCompletedEvent> cardStatusCompleted = JsonConvert.DeserializeObject<ResponseSucess<CardStatusCompletedEvent>>(result);
            Assert.True(cardStatusCompleted.Success);
            Assert.NotEmpty(cardStatusCompleted.Data.Id);
            Assert.Equal("CardStatusCompletedEvent", cardStatusCompleted.Data.MessageType);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Trait("TrelloController API - Integration Tests", "Get card Details")]
        [Theory(DisplayName = "Get card status should return unable to find")]
        [InlineData("12")]
        public async Task TrelloController_IntegrationTest_GetCardStatus__ShouldReturnBadRequest_UnknowCard(string id)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();
            TestServer testServer = new TestServer(builder);
            HttpClient client = testServer.CreateClient();

            var response = await client.GetAsync(string.Format("Trello/GetCardStatus?cardId={0}", id));
            var result = await response.Content.ReadAsStringAsync();

            ResponseErrors<CardStatusUnableToFindEvent> cardStatusUnkwon = JsonConvert.DeserializeObject<ResponseErrors<CardStatusUnableToFindEvent>>(result);
            Assert.False(cardStatusUnkwon.Success);
            Assert.Equal("CardStatusUnableToFindEvent", cardStatusUnkwon.Errors.MessageType);
            Assert.NotEmpty(cardStatusUnkwon.Errors.Id);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Trait("TrelloController API - Integration Tests", "Get card Details")]
        [Theory(DisplayName = "Get card status should return the card attachments")]
        [InlineData("5e8a3eb3a6ef1f3eed24b319")]
        public async Task TrelloController_IntegrationTest_GetCardAttachments__ShouldReturnOkRequest_CardHasAttachments(string cardId)
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();
            TestServer testServer = new TestServer(builder);
            HttpClient client = testServer.CreateClient();

            var response = await client.GetAsync(string.Format("Trello/GetCardAttachments?cardId={0}", cardId));
            var result = await response.Content.ReadAsStringAsync();
            ResponseSucess<ReturnCardAttachmentsEvent> cardAttachments = JsonConvert.DeserializeObject<ResponseSucess<ReturnCardAttachmentsEvent>>(result);
            Assert.True(cardAttachments.Success);
            Assert.True(cardAttachments.Data.Attachments.Count > 0);
            Assert.Equal("ReturnCardAttachmentsEvent", cardAttachments.Data.MessageType);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Trait("TrelloController API - Integration Tests", "Get card Details")]
        [Theory(DisplayName = "Get card status should return card dosen't have attachments")]
        [InlineData("5e8a3eb3a6ef1f3eed24b319")]
        public async Task TrelloController_IntegrationTest_GetCardAttachments__ShouldReturnOkRequest_CardDosentHaveAttachments(string cardId)
        {
            var builder = new WebHostBuilder()
                           .UseEnvironment("Development")
                           .UseStartup<Startup>();
            TestServer testServer = new TestServer(builder);
            HttpClient client = testServer.CreateClient();

            var response = await client.GetAsync(string.Format("Trello/GetCardAttachments?cardId={0}", cardId));
            var result = await response.Content.ReadAsStringAsync();
            ResponseSucess<CardDosentHaveAttchmentsEvent> cardAttachments = JsonConvert.DeserializeObject<ResponseSucess<CardDosentHaveAttchmentsEvent>>(result);
            Assert.True(cardAttachments.Success);
            Assert.NotEmpty(cardAttachments.Data.Id);
            Assert.Equal("CardDosentHaveAttchmentsEvent", cardAttachments.Data.MessageType);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Trait("TrelloController API - Integration Tests", "Get card Details")]
        [Theory(DisplayName = "Get card status should return unable to find attachments")]
        [InlineData("2212")]
        public async Task TrelloController_IntegrationTest_GetCardAttachments__ShouldReturnBadRequest_InvalidBoardOrCardId(string cardId)
        {
            var builder = new WebHostBuilder()
                         .UseEnvironment("Development")
                         .UseStartup<Startup>();
            TestServer testServer = new TestServer(builder);
            HttpClient client = testServer.CreateClient();

            var response = await client.GetAsync(string.Format("Trello/GetCardAttachments?cardId={0}", cardId));
            var result = await response.Content.ReadAsStringAsync();
            ResponseErrors<UnableToFindCardAttachmentsEvent> cardAttachments = JsonConvert.DeserializeObject<ResponseErrors<UnableToFindCardAttachmentsEvent>>(result);
            Assert.False(cardAttachments.Success);
            Assert.Equal("UnableToFindCardAttachmentsEvent", cardAttachments.Errors.MessageType);
            Assert.False(response.IsSuccessStatusCode);
        }
    }
}
