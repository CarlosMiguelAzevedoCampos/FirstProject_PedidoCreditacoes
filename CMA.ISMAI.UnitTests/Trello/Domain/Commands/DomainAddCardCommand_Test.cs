using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.Domain.CommandHandlers;
using CMA.ISMAI.Trello.Domain.Commands;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;
using CMA.ISMAI.Trello.Engine.Automation;
using CMA.ISMAI.Trello.Engine.Interface;
using CMA.ISMAI.Trello.FileReader.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.UnitTests.Trello.Domain.Commands
{
    public class DomainAddCardCommand_Test
    {
        [Trait("CardCommandHandler", "Add Card")]
        [Theory(DisplayName = "New card creation, but should fail because of invalid parameters")]
        [InlineData("ISMAI - Informatica - Carlos Campos", "", -1, "ISMAI", "Informática", "", true)]
        [InlineData("", "Carlos Campos", 1, "ISMAI", "", "Carlos Campos", true)]
        [InlineData("", "", 2, "ISMAI", "Informática", "Carlos Campos", false)]
        [InlineData(null, null, 1, "", "Informática", "Carlos Campos", true)]
        public void CardCommandHandler_AddCard_ShouldFail_BecauseOfNullOrEmptyParameters(string name, string description, int boardId, string instituteName, string courseName, string studentName, bool IsCetOrOtherCondition)
        {
            var logMock = new Mock<ILog>();
            var trelloMock = new Mock<ITrello>();
            var cardnotificationMock = new Mock<ICardEventHandler>();
            var engineMock = new Mock<IEngine>();
            var engineEventMock = new Mock<IEngineEventHandler>();
            var fileReader = new Mock<IFileReader>();

            AddCardCommand addCard = new AddCardCommand(name, DateTime.Now, description, boardId, new List<string>(),
                instituteName, courseName, studentName, IsCetOrOtherCondition);
            CardCommandHandler cardCommandHandler = new CardCommandHandler(logMock.Object, trelloMock.Object, cardnotificationMock.Object,
              engineMock.Object, engineEventMock.Object, fileReader.Object);

            Event result = cardCommandHandler.Handler(addCard);
            Assert.True(result is AddCardFailedEvent);
        }


        [Trait("CardCommandHandler", "Add Card")]
        [Theory(DisplayName = "New card creation, but should fail because of invalid attachments")]
        [InlineData("ISMAI - Informatica - Carlos Campos", "Informática", 0, "ISMAI", "Informática", "Carlos Campos", true)]
        [InlineData("ISMAI - Informatica - Carlos Campos", "Informática", 0, "ISMAI", "Informática", "Carlos Campos", false)]
        public void CardCommandHandler_AddCard_ShouldFail_BecauseOfNullUrlAttachments(string name, string description, int boardId, string instituteName, string courseName, string studentName, bool IsCetOrOtherCondition)
        {
            var logMock = new Mock<ILog>();
            var trelloMock = new Mock<ITrello>();
            var cardnotificationMock = new Mock<ICardEventHandler>();
            var engineMock = new Mock<IEngine>();
            var engineEventMock = new Mock<IEngineEventHandler>();
            var fileReader = new Mock<IFileReader>();


            AddCardCommand addCard = new AddCardCommand(name, DateTime.Now, description, boardId, null,
               instituteName, courseName, studentName, IsCetOrOtherCondition);
            CardCommandHandler cardCommandHandler = new CardCommandHandler(logMock.Object, trelloMock.Object, cardnotificationMock.Object,
                engineMock.Object, engineEventMock.Object, fileReader.Object);

            Event result = cardCommandHandler.Handler(addCard);
            Assert.True(result is AddCardFailedEvent);
        }

        [Trait("CardCommandHandler", "Add Card")]
        [Theory(DisplayName = "New card creation, add the Card")]
        [InlineData("ISMAI - Informática - Carlos Campos", "Informática", 0, "ISMAI", "Informática", "Carlos Campos", true)]
        [InlineData("ISMAI - Informática - Carlos Campos", "Informática", 1, "ISMAI", "Informática", "Carlos Campos", false)]
        [InlineData("ISMAI - Informática - Carlos Campos", "Informática", 2, "ISMAI", "Informática", "Carlos Campos", false)]
        [InlineData("ISMAI - Informática - Carlos Campos", "Informática", 3, "ISMAI", "Informática", "Carlos Campos", false)]

        public void CardCommandHandler_AddCard_ShouldReturn_AddCardCompletedEvent(string name, string description, int boardId, string instituteName, string courseName, string studentName, bool IsCetOrOtherCondition)
        {
            var logMock = new Mock<ILog>();
            var trelloMock = new Mock<ITrello>();
            var cardnotificationMock = new Mock<ICardEventHandler>();
            var engineMock = new Mock<IEngine>();
            var engineEventMock = new Mock<IEngineEventHandler>();
            var fileReader = new Mock<IFileReader>();

            trelloMock.Setup(x => x.AddCard(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(Task.FromResult(Guid.NewGuid().ToString()));

            AddCardCommand addCard = new AddCardCommand(name, DateTime.Now.AddDays(20), description, boardId, new List<string>(),
               instituteName, courseName, studentName, IsCetOrOtherCondition);
            CardCommandHandler cardCommandHandler = new CardCommandHandler(logMock.Object, trelloMock.Object, cardnotificationMock.Object,
                engineMock.Object, engineEventMock.Object, fileReader.Object);

            Event result = cardCommandHandler.Handler(addCard);
            Assert.True(result is AddCardCompletedEvent);
        }
    }
}
