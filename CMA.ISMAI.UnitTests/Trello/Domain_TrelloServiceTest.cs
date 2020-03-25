using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.Domain.CommandHandlers;
using CMA.ISMAI.Trello.Domain.Commands;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;
using CMA.ISMAI.Trello.Engine.Interface;
using Moq;
using System;
using System.Collections.Generic;
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
        [InlineData(null, null, 1)]
        public void TrelloService_AddCard_ShouldFailBecauseOfNullOrEmptyParameters(string name, string description, int boardId)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<ITrello>();
            var notificaitionMock = new Mock<ICardEventHandler>();


            AddCardCommand addCard = new AddCardCommand(name, DateTime.Now, description, boardId, new List<string>());
            CardCommandHandler cardCommandHandler = new CardCommandHandler(logMock.Object, engineMock.Object, notificaitionMock.Object);

            Event result = cardCommandHandler.Handler(addCard);
            Assert.True(result is AddCardFailedEvent);
        }

        [Theory]
        [InlineData("ISMAI - Informatica creditaçâo", "ISMAI", 1)]
        public void TrelloService_AddCard_ShouldFailBecauseOfNullFilesUrl(string name, string description, int boardId)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<ITrello>();
            var notificaitionMock = new Mock<ICardEventHandler>();


            AddCardCommand addCard = new AddCardCommand(name, DateTime.Now, description, boardId, null);
            CardCommandHandler cardCommandHandler = new CardCommandHandler(logMock.Object, engineMock.Object, notificaitionMock.Object);

            Event result = cardCommandHandler.Handler(addCard);
            Assert.True(result is AddCardFailedEvent);
        }

        [Theory]
        [InlineData("ISMAI - Informatica creditaçâo", "Carlos Campos", 1)]
        [InlineData("ISMAI - Multimedia creditaçâo", "Miguel Campos", 0)]
        [InlineData("ISMAI - Multimedia creditaçâo", "Miguel Campos", 2)]
        public void TrelloService_AddCard_ShouldReturnTrue(string name, string description, int boardId)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<ITrello>();
            var notificaitionMock = new Mock<ICardEventHandler>();

            engineMock.Setup(x => x.AddCard(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<List<string>>()))
                .Returns(Task.FromResult(Guid.NewGuid().ToString()));

            AddCardCommand addCard = new AddCardCommand(name, DateTime.Now.AddDays(14), description, boardId,new List<string>());
            CardCommandHandler cardCommandHandler = new CardCommandHandler(logMock.Object, engineMock.Object, notificaitionMock.Object);

            Event result = cardCommandHandler.Handler(addCard);
            Assert.True(result is AddCardCompletedEvent);
        }

        [Theory]
        [InlineData("ISMAI - Informatica creditaçâo", "Carlos Campos", 1)]
        [InlineData("ISMAI - Multimedia creditaçâo", "Miguel Campos", 2)]
        [InlineData("ISMAI - Multimedia creditaçâo", "Miguel Campos", 0)]
        public void TrelloService_AddCard_ShouldFail_EngineCrash(string name, string description, int boardId)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<ITrello>();
            var notificaitionMock = new Mock<ICardEventHandler>();

            engineMock.Setup(x => x.AddCard(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<List<string>>()))
               .Returns(Task.FromResult(string.Empty));
            AddCardCommand addCard = new AddCardCommand(name, DateTime.Now.AddDays(20), description, boardId, new List<string>());
            CardCommandHandler cardCommandHandler = new CardCommandHandler(logMock.Object, engineMock.Object, notificaitionMock.Object);

            Event result = cardCommandHandler.Handler(addCard);
            Assert.True(result is AddCardFailedEvent);
        }

        [Theory]
        [InlineData("44454a2sda3s562a", 1)]
        [InlineData("44454a34sda3s562a", 2)]
        [InlineData("444513a2sda3s562a", 0)]
        public void TrelloService_GetCardAttachments_ShouldReturnTheAttachments(string cardId, int boardId)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<ITrello>();
            var notificaitionMock = new Mock<ICardEventHandler>();
            var urls = new List<string>();
            urls.Add("google.pt");
            engineMock.Setup(x => x.ReturnCardAttachmenets(It.IsAny<string>(), It.IsAny<int>()))
               .Returns(Task.FromResult(urls));

            GetCardAttachmentsCommand getCardAttachmentsCommand = new GetCardAttachmentsCommand(cardId, boardId);
            CardCommandHandler cardCommandHandler = new CardCommandHandler(logMock.Object, engineMock.Object, notificaitionMock.Object);

            Event result = cardCommandHandler.Handler(getCardAttachmentsCommand);
            Assert.True(result is CardHasAttachmentsEvent);
        }

        [Theory]
        [InlineData("44454a2sda3s562a", 1)]
        [InlineData("44454a34sda3s562a", 2)]
        [InlineData("444513a2sda3s562a", 0)]
        public void TrelloService_GetCardAttachments_ShouldReturnAListOfEmptyAttachments(string cardId, int boardId)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<ITrello>();
            var notificaitionMock = new Mock<ICardEventHandler>();
            engineMock.Setup(x => x.ReturnCardAttachmenets(It.IsAny<string>(), It.IsAny<int>()))
               .Returns(Task.FromResult(new List<string>()));

            GetCardAttachmentsCommand getCardAttachmentsCommand = new GetCardAttachmentsCommand(cardId, boardId);
            CardCommandHandler cardCommandHandler = new CardCommandHandler(logMock.Object, engineMock.Object, notificaitionMock.Object);

            Event result = cardCommandHandler.Handler(getCardAttachmentsCommand);
            Assert.True(result is CardDosentHaveAttachmentsEvent);
        }
    }
}
