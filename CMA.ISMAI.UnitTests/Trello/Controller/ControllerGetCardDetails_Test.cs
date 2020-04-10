using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.API.Controllers;
using CMA.ISMAI.Trello.Domain.Commands;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace CMA.ISMAI.UnitTests.Trello.Controller
{
    public class ControllerGetCardDetails_Test
    {
        [Trait("TrelloController API", "Get card details")]
        [Fact(DisplayName = "Get card status should fail because of null parameters")]
        public void TrelloController_GetCardStatus_ShouldReturnBadRequest_BecauseOfEmptyOrNullParameters()
        {
            var logMock = new Mock<ILog>();
            var eventMock = new Mock<ICardCommandHandler>();
            TrelloController trelloController = new TrelloController(logMock.Object, eventMock.Object);

            IActionResult result = trelloController.GetCardStatus("");
            var resultCode = result as BadRequestResult;
            Assert.IsType<BadRequestResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }

        [Trait("TrelloController API", "Get card details")]
        [Theory(DisplayName = "Get card status should return active card.")]
        [InlineData("ifh2i992h2b-asfa-1w")]
        [InlineData("cjckamrb222-we-1w")]
        public void TrelloController_GetCardStatus_ShouldReturnOkStatus_ActiveCard(string id)
        {
            var logMock = new Mock<ILog>();
            var eventMock = new Mock<ICardCommandHandler>();
            eventMock.Setup(x => x.Handler(It.IsAny<GetCardStatusCommand>()))
                .Returns(new CardStatusIncompletedEvent(id));

            TrelloController trelloController = new TrelloController(logMock.Object, eventMock.Object);

            IActionResult result = trelloController.GetCardStatus(id);
            var resultCode = result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(resultCode.StatusCode == 200);
        }

        [Trait("TrelloController API", "Get card details")]
        [Theory(DisplayName = "Get card status should return completed card.")]
        [InlineData("ifh2i992h2b-asfa-1w")]
        [InlineData("cjckamrb222-we-1w")]
        public void TrelloController_GetCardStatus_ShouldReturnOkStatus_CompletedCard(string id)
        {
            var logMock = new Mock<ILog>();
            var eventMock = new Mock<ICardCommandHandler>();
            eventMock.Setup(x => x.Handler(It.IsAny<GetCardStatusCommand>()))
                .Returns(new CardStatusCompletedEvent(id));

            TrelloController trelloController = new TrelloController(logMock.Object, eventMock.Object);

            IActionResult result = trelloController.GetCardStatus(id);
            var resultCode = result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(resultCode.StatusCode == 200);
        }


        [Trait("TrelloController API", "Get card details")]
        [Theory(DisplayName = "Get card status should return unknown card.")]
        [InlineData("ifh2i992h2b-asfa-1w")]
        [InlineData("cjckamrb222-we-1w")]
        public void TrelloController_GetCardStatus_ShouldReturnOkStatus_UnknownCard(string id)
        {
            var logMock = new Mock<ILog>();
            var eventMock = new Mock<ICardCommandHandler>();
            eventMock.Setup(x => x.Handler(It.IsAny<GetCardStatusCommand>()))
                .Returns(new CardStatusUnableToFindEvent(id));

            TrelloController trelloController = new TrelloController(logMock.Object, eventMock.Object);

            IActionResult result = trelloController.GetCardStatus(id);
            var resultCode = result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }


        [Trait("TrelloController API", "Get card details")]
        [Theory(DisplayName = "Get card status should return bad request because of empty cardId.")]
        [InlineData("")]
        public void TrelloController_GetCardStatus_ShouldReturnBadRequest_BecauseOfEmptyCardIdAndBadBoardId(string cardId)
        {
            var logMock = new Mock<ILog>();
            var eventMock = new Mock<ICardCommandHandler>();
            TrelloController trelloController = new TrelloController(logMock.Object, eventMock.Object);

            IActionResult result = trelloController.GetCardStatus(cardId);
            var resultCode = result as BadRequestResult;
            Assert.IsType<BadRequestResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }

        [Trait("TrelloController API", "Get card details")]
        [Theory(DisplayName = "Get card attachmenets should Ok status")]
        [InlineData("ifh2i992h2b-asfa-1w")]
        [InlineData("cjckamrb222-we-1w")]
        public void TrelloController_GetCardAttachments_ShouldReturnOkStatus(string cardId)
        {
            List<string> urls = new List<string>();
            urls.Add("google.pt");
            var logMock = new Mock<ILog>();
            var eventMock = new Mock<ICardCommandHandler>();
            eventMock.Setup(x => x.Handler(It.IsAny<GetCardAttachmentsCommand>()))
                .Returns(new ReturnCardAttachmentsEvent(cardId, urls));

            TrelloController trelloController = new TrelloController(logMock.Object, eventMock.Object);

            IActionResult result = trelloController.GetCardAttachments(cardId);
            var resultCode = result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(resultCode.StatusCode == 200);
        }

        [Trait("TrelloController API", "Get card details")]
        [Fact(DisplayName = "Get card attachmenets should fail because of empty cardid")]
        public void TrelloController_GetCardAttachments_ShouldReturnBadStatus_EmptyCardId()
        {
            List<string> urls = new List<string>();
            urls.Add("google.pt");
            var logMock = new Mock<ILog>();
            var eventMock = new Mock<ICardCommandHandler>();

            TrelloController trelloController = new TrelloController(logMock.Object, eventMock.Object);

            IActionResult result = trelloController.GetCardAttachments("");
            var resultCode = result as BadRequestResult;
            Assert.IsType<BadRequestResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }

    }
}