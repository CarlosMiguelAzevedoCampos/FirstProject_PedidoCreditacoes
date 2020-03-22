using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.Domain.CommandHandlers;
using CMA.ISMAI.Trello.Domain.Commands;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;
using CMA.ISMAI.Trello.Engine.Interface;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.UnitTests.Engine.Domain
{
    public class Domain_TrelloServiceTest
    {
        [Theory]
        [InlineData("ISMAI - Informatica creditaçâo", "", -1)]
        [InlineData("", "Carlos Campos", 1)]
        [InlineData("", "", 2)]
        [InlineData(null, null, 3)]
        public void TrelloService_AddCard_ShouldFailBecauseOfNullOrEmptyParameters(string name, string description, int boardId)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<ITrello>();
            var notificaitionMock = new Mock<ICardEventHandler>();


            AddCardCommand addCard = new AddCardCommand(name, DateTime.Now, description, boardId);
            CardCommandHandler cardCommandHandler = new CardCommandHandler(logMock.Object, engineMock.Object, notificaitionMock.Object);

            Event result = cardCommandHandler.Handler(addCard);
            Assert.True(result is AddCardFailedEvent);
        }

        [Theory]
        [InlineData("ISMAI - Informatica creditaçâo", "Carlos Campos", 1)]
        [InlineData("ISMAI - Multimedia creditaçâo", "Miguel Campos", 0)]
        [InlineData("ISMAI - Multimedia creditaçâo", "Miguel Campos", 2)]
        [InlineData("ISMAI - Multimedia creditaçâo", "Miguel Campos", 3)]
        public void TrelloService_AddCard_ShouldReturnTrue(string name, string description, int boardId)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<ITrello>();
            var notificaitionMock = new Mock<ICardEventHandler>();

            engineMock.Setup(x => x.AddCard(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(Task.FromResult(Guid.NewGuid().ToString()));

            AddCardCommand addCard = new AddCardCommand(name, DateTime.Now.AddDays(14), description, boardId);
            CardCommandHandler cardCommandHandler = new CardCommandHandler(logMock.Object, engineMock.Object, notificaitionMock.Object);

            Event result = cardCommandHandler.Handler(addCard);
            Assert.True(result is AddCardCompletedEvent);
        }

        [Theory]
        [InlineData("ISMAI - Informatica creditaçâo", "Carlos Campos", 1)]
        [InlineData("ISMAI - Multimedia creditaçâo", "Miguel Campos", 2)]
        [InlineData("ISMAI - Multimedia creditaçâo", "Miguel Campos", 0)]
        [InlineData("ISMAI - Multimedia creditaçâo", "Miguel Campos", 3)]
        public void TrelloService_AddCard_ShouldFail_EngineCrash(string name, string description, int boardId)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<ITrello>();
            var notificaitionMock = new Mock<ICardEventHandler>();

            engineMock.Setup(x => x.AddCard(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>()))
               .Returns(Task.FromResult(string.Empty));
            AddCardCommand addCard = new AddCardCommand(name, DateTime.Now.AddDays(20), description, boardId);
            CardCommandHandler cardCommandHandler = new CardCommandHandler(logMock.Object, engineMock.Object, notificaitionMock.Object);

            Event result = cardCommandHandler.Handler(addCard);
            Assert.True(result is AddCardFailedEvent);
        }
    }
}
