using CMA.ISMAI.Core.Bus;
using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.API.Controllers;
using CMA.ISMAI.Trello.API.Model;
using CMA.ISMAI.Trello.Domain.Commands;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.UnitTests.Engine.Domain
{
    public class Controller_TrelloControllerTest
    {

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TrelloController_GetCardStatus_ShouldReturnBadRequestBecauseOfEmptyOrNullParameters(string id)
        {
            var logMock = new Mock<ILog>();
            var busMock = new Mock<IMediatorHandler>();

            TrelloController trelloController = new TrelloController(logMock.Object, busMock.Object, new DomainNotificationHandler());

            IActionResult result = trelloController.GetCardStatus(id);
            var resultCode = result as BadRequestResult;
            Assert.IsType<BadRequestResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }

        [Theory]
        [InlineData("ifh2i992h2b-asfa-1w", false)]
        [InlineData("cjckamrb222-we-1w", true)]
        public void TrelloController_GetCardStatus_ShouldReturnOkStatus(string id, bool resultSetup)
        {
            var logMock = new Mock<ILog>();
            var busMock = new Mock<IMediatorHandler>();
            busMock.Setup(x => x.SendCommand(It.IsAny<CardStatusCommand>())).Returns(Task.FromResult(resultSetup));

            TrelloController trelloController = new TrelloController(logMock.Object, busMock.Object, new DomainNotificationHandler());


            IActionResult result = trelloController.GetCardStatus(id);
            var resultCode = result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(resultCode.StatusCode == 200);
        }


        [Fact]
        public void TrelloController_AddCard_ShouldReturnBadStatusBecauseOfNullDto()
        {
            var logMock = new Mock<ILog>();
            var busMock = new Mock<IMediatorHandler>();
            TrelloController trelloController = new TrelloController(logMock.Object, busMock.Object, new DomainNotificationHandler());


            IActionResult result = trelloController.AddCard(null);
            var resultCode = result as BadRequestResult;
            Assert.IsType<BadRequestResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }



        [Theory]
        [InlineData("ISMAI", "Process_00kjdw0")]
        [InlineData("FEUP", "Process_00kjdw0")]
        public void TrelloController_AddCard_ShouldReturnOkStatus(string cardName, string cardDescription)
        {
            var logMock = new Mock<ILog>();
            var busMock = new Mock<IMediatorHandler>();
            TrelloController trelloController = new TrelloController(logMock.Object, busMock.Object, new DomainNotificationHandler());


            IActionResult result = trelloController.AddCard(new CardDto(cardName, DateTime.Now, cardDescription));
            var resultCode = result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(resultCode.StatusCode == 200);
        }

    }
}
