using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.API.Controllers;
using CMA.ISMAI.Trello.API.Model;
using CMA.ISMAI.Trello.Domain.Commands;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace CMA.ISMAI.UnitTests.Engine.Domain
{
    public class Controller_TrelloControllerTest
    {

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TrelloController_GetCardStatus_ShouldReturnBadRequest_BecauseOfEmptyOrNullParameters(string id)
        {
            var logMock = new Mock<ILog>();
            var eventMock = new Mock<ICardCommandHandler>();
            TrelloController trelloController = new TrelloController(logMock.Object, eventMock.Object);

            IActionResult result = trelloController.GetCardStatus(id);
            var resultCode = result as BadRequestResult;
            Assert.IsType<BadRequestResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }

        [Theory]
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

        [Theory]
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

        [Theory]
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


        [Fact]
        public void TrelloController_AddCard_ShouldReturnBadStatusBecauseOfNullDto()
        {
            var logMock = new Mock<ILog>();
            var eventMock = new Mock<ICardCommandHandler>();
            TrelloController trelloController = new TrelloController(logMock.Object, eventMock.Object);


            IActionResult result = trelloController.AddCard(null);
            var resultCode = result as BadRequestResult;
            Assert.IsType<BadRequestResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }

        [Theory]
        [InlineData("ISMAI", "Process_00kjdw0", 0)]
        [InlineData("FEUP", "Process_00kjdw0", 1)]
        [InlineData("FEUP", "Process_00kjdw0", 2)]
        public void TrelloController_AddCard_ShouldReturnOkStatus(string cardName, string cardDescription, int boardId)
        {
            var logMock = new Mock<ILog>();
            var eventMock = new Mock<ICardCommandHandler>();
            eventMock.Setup(x => x.Handler(It.IsAny<AddCardCommand>())).Returns(new AddCardCompletedEvent(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()));
            TrelloController trelloController = new TrelloController(logMock.Object, eventMock.Object);

            IActionResult result = trelloController.AddCard(new CardDto(cardName, DateTime.Now.AddDays(1), cardDescription, boardId, new List<string>()));
            var resultCode = result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(resultCode.StatusCode == 200);
        }

        [Theory]
        [InlineData("ISMAI", "Process_00kjdw0", -1)]
        public void TrelloController_AddCard_ShouldReturnBadStatusBasedDomainExceptions(string cardName, string cardDescription, int boardId)
        {
            var logMock = new Mock<ILog>();
            var eventMock = new Mock<ICardCommandHandler>();
            List<DomainNotification> domainNotifications = new List<DomainNotification>();
            eventMock.Setup(x => x.Handler(It.IsAny<AddCardCommand>())).Returns(new AddCardFailedEvent(domainNotifications));
            TrelloController trelloController = new TrelloController(logMock.Object, eventMock.Object);

            IActionResult result = trelloController.AddCard(new CardDto(cardName, DateTime.Now.AddDays(1), cardDescription, boardId, new List<string>()));
            var resultCode = result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }

        [Theory]
        [InlineData("ifh2i992h2b-asfa-1w", 1)]
        [InlineData("cjckamrb222-we-1w", 2)]
        public void TrelloController_GetCardAttachments_ShouldReturnOkStatus(string cardId, int boardId)
        {
            List<string> urls = new List<string>();
            urls.Add("google.pt");
            var logMock = new Mock<ILog>();
            var eventMock = new Mock<ICardCommandHandler>();
            eventMock.Setup(x => x.Handler(It.IsAny<GetCardAttachmentsCommand>()))
                .Returns(new ReturnCardAttachmentsEvent(urls));

            TrelloController trelloController = new TrelloController(logMock.Object, eventMock.Object);

            IActionResult result = trelloController.GetCardAttachments(cardId, boardId);
            var resultCode = result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(resultCode.StatusCode == 200);
        }


        [Theory]
        [InlineData("", -1)]
        [InlineData(null, 1)]
        public void TrelloController_GetCardStatus_ShouldReturnBadRequest_BecauseOfEmptyCardIdAndBadBoardId(string cardId, int boardId)
        {
            var logMock = new Mock<ILog>();
            var eventMock = new Mock<ICardCommandHandler>();
            TrelloController trelloController = new TrelloController(logMock.Object, eventMock.Object);

            IActionResult result = trelloController.GetCardAttachments(cardId, boardId);
            var resultCode = result as BadRequestResult;
            Assert.IsType<BadRequestResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }
    }
}