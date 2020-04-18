using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.API.Controllers;
using CMA.ISMAI.Trello.Domain.Commands;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace CMA.ISMAI.UnitTests.Trello.Controller
{
    public class ControllerDeleteCard_Test
    {
        [Trait("TrelloController API", "Delete Card")]
        [Fact(DisplayName = "CardId is null or empty")]
        public void TrelloController_DeleteCard_ShouldReturnBadStatusBecauseOfCardId()
        {
            var logMock = new Mock<ILog>();
            var eventMock = new Mock<ICardCommandHandler>();
            TrelloController trelloController = new TrelloController(logMock.Object, eventMock.Object);
            IActionResult result = trelloController.DeleteCard(null);
            var resultCode = result as BadRequestResult;
            Assert.IsType<BadRequestResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }

        [Trait("TrelloController API", "Delete Card")]
        [Fact(DisplayName = "Card Should be deleted")]
        public void TrelloController_DeleteCard_ShouldReturnOkStatus_CardHasBeenDeleted()
        {
            var logMock = new Mock<ILog>();
            var eventMock = new Mock<ICardCommandHandler>();
            TrelloController trelloController = new TrelloController(logMock.Object, eventMock.Object);
            eventMock.Setup(x => x.Handler(It.IsAny<DeleteCardCommand>())).Returns(new CardHasBeenDeletedEvent(It.IsAny<string>()));
            IActionResult result = trelloController.DeleteCard(Guid.NewGuid().ToString());
            var resultCode = result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(resultCode.StatusCode == 200);
        }


        [Trait("TrelloController API", "Delete Card")]
        [Fact(DisplayName = "Card had a problem to be deleted")]
        public void TrelloController_DeleteCard_ShouldReturnBadStatus_CardHasNotBeenDeleted()
        {
            var logMock = new Mock<ILog>();
            var eventMock = new Mock<ICardCommandHandler>();
            TrelloController trelloController = new TrelloController(logMock.Object, eventMock.Object);
            eventMock.Setup(x => x.Handler(It.IsAny<DeleteCardCommand>())).Returns(new CardHasNotBeenDeletedEvent(It.IsAny<string>()));
            IActionResult result = trelloController.DeleteCard(Guid.NewGuid().ToString());
            var resultCode = result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }
    }
}
