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

namespace CMA.ISMAI.UnitTests.Trello.Controller
{
    public class ControllerAddCardAndProcess_Test
    {
        [Trait("TrelloController API", "Add Card and Process")]
        [Fact(DisplayName = "Card shouldn't be added because of null parameters")]
        public void TrelloController_AddCardAndProcess_ShouldReturnBadStatusBecauseOfNullDto()
        {
            var logMock = new Mock<ILog>();
            var eventMock = new Mock<ICardCommandHandler>();
            TrelloController trelloController = new TrelloController(logMock.Object, eventMock.Object);


            IActionResult result = trelloController.AddCardAndProcess(null);
            var resultCode = result as BadRequestResult;
            Assert.IsType<BadRequestResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }

        [Trait("TrelloController API", "Add Card and Process")]
        [Theory(DisplayName = "Card should add the card and return Ok Result")]
        [InlineData("ISMAI - Informatica - Carlos Campos", "Informática", 0, "ISMAI", "Informática", "Carlos Campos", true)]
        [InlineData("ISMAI - Informatica - Carlos Campos", "Informática", 0, "ISMAI", "Informática", "Carlos Campos", false)]
        public void TrelloController_AddCardAndProcess_ShouldReturnOkStatus(string name, string description, int boardId, string instituteName, string courseName, string studentName, bool IsCetOrOtherCondition)
        {
            var logMock = new Mock<ILog>();
            var eventMock = new Mock<ICardCommandHandler>();
            eventMock.Setup(x => x.Handler(It.IsAny<AddCardCommandAndProcess>())).Returns(new AddCardCompletedEvent(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()));
            TrelloController trelloController = new TrelloController(logMock.Object, eventMock.Object);

            IActionResult result = trelloController.AddCardAndProcess(new CardDto(name, DateTime.Now.AddDays(1), description, boardId, new List<string>(), instituteName, courseName, studentName, IsCetOrOtherCondition));
            var resultCode = result as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
            Assert.True(resultCode.StatusCode == 200);
        }

        [Trait("TrelloController API", "Add Card and Process")]
        [Theory(DisplayName = "Card shouldn't be added because it contains domain notifications")]
        [InlineData("ISMAI - Informatica - Carlos Campos", "Informática", -1, "ISMAI", "Informática", "Carlos Campos", true)]
        [InlineData("ISMAI - Informatica - Carlos Campos", "Informática", 0, "", "Informática", "Carlos Campos", false)]
        public void TrelloController_AddCardAndProcess_ShouldReturnBadStatusBasedDomainExceptions(string name, string description, int boardId, string instituteName, string courseName, string studentName, bool IsCetOrOtherCondition)
        {
            var logMock = new Mock<ILog>();
            var eventMock = new Mock<ICardCommandHandler>();
            List<DomainNotification> domainNotifications = new List<DomainNotification>();

            eventMock.Setup(x => x.Handler(It.IsAny<AddCardCommandAndProcess>())).Returns(new AddCardFailedEvent(domainNotifications, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()));
            TrelloController trelloController = new TrelloController(logMock.Object, eventMock.Object);

            IActionResult result = trelloController.AddCardAndProcess(new CardDto(name, DateTime.Now.AddDays(1), description, boardId, new List<string>(), instituteName, courseName, studentName, IsCetOrOtherCondition));
            var resultCode = result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(resultCode.StatusCode == 400);
        }
    }
}
