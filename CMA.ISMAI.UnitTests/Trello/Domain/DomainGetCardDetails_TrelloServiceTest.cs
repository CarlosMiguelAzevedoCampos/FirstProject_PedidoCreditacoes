using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.Domain.CommandHandlers;
using CMA.ISMAI.Trello.Domain.Commands;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;
using CMA.ISMAI.Trello.Engine.Automation;
using CMA.ISMAI.Trello.Engine.Interface;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.UnitTests.Trello.Domain
{
    public class DomainGetCardDetails_TrelloServiceTest
    {
        [Theory]
        [InlineData("44454a2sda3s562a")]
        [InlineData("44454a34sda3s562a")]
        [InlineData("444513a2sda3s562a")]
        public void TrelloService_GetCardAttachments_ShouldReturn_TheAttachments(string cardId)
        {
            var logMock = new Mock<ILog>();
            var trelloMock = new Mock<ITrello>();
            var cardnotificationMock = new Mock<ICardEventHandler>();
            var engineMock = new Mock<IEngine>();
            var engineEventMock = new Mock<IEngineEventHandler>();

            var urls = new List<string>();
            urls.Add("google.pt");

            trelloMock.Setup(x => x.ReturnCardAttachmenets(It.IsAny<string>()))
               .Returns(Task.FromResult(urls));

            GetCardAttachmentsCommand getCardAttachmentsCommand = new GetCardAttachmentsCommand(cardId);
            CardCommandHandler cardCommandHandler = new CardCommandHandler(logMock.Object, trelloMock.Object, cardnotificationMock.Object,
                 engineMock.Object, engineEventMock.Object);

            Event result = cardCommandHandler.Handler(getCardAttachmentsCommand);
            trelloMock.Verify(x => x.ReturnCardAttachmenets(It.IsAny<string>()), Times.Once);
            Assert.True(result is ReturnCardAttachmentsEvent);
        }



        [Theory]
        [InlineData("44454a2sda3s562a")]
        [InlineData("44454a34sda3s562a")]
        [InlineData("444513a2sda3s562a")]
        public void TrelloService_GetCardStatus_ShouldReturn_CompletedStatus(string cardId)
        {
            var logMock = new Mock<ILog>();
            var trelloMock = new Mock<ITrello>();
            var cardnotificationMock = new Mock<ICardEventHandler>();
            var engineMock = new Mock<IEngine>();
            var engineEventMock = new Mock<IEngineEventHandler>();


            trelloMock.Setup(x => x.IsTheProcessFinished(It.IsAny<string>()))
               .Returns(Task.FromResult(1));

            GetCardStatusCommand getCardStatusCommand = new GetCardStatusCommand(cardId);
            CardCommandHandler cardCommandHandler = new CardCommandHandler(logMock.Object, trelloMock.Object, cardnotificationMock.Object,
                   engineMock.Object, engineEventMock.Object);

            Event result = cardCommandHandler.Handler(getCardStatusCommand);
            Assert.True(result is CardStatusCompletedEvent);
        }

        [Theory]
        [InlineData("44454a2sda3s562a")]
        [InlineData("44454a34sda3s562a")]
        [InlineData("444513a2sda3s562a")]
        public void TrelloService_GetCardStatus_ShouldReturnInCompleted(string cardId)
        {
            var logMock = new Mock<ILog>();
            var trelloMock = new Mock<ITrello>();
            var cardnotificationMock = new Mock<ICardEventHandler>();
            var engineMock = new Mock<IEngine>();
            var engineEventMock = new Mock<IEngineEventHandler>();

            trelloMock.Setup(x => x.IsTheProcessFinished(It.IsAny<string>()))
                       .Returns(Task.FromResult(0));

            GetCardStatusCommand getCardStatusCommand = new GetCardStatusCommand(cardId);
            CardCommandHandler cardCommandHandler = new CardCommandHandler(logMock.Object, trelloMock.Object, cardnotificationMock.Object,
              engineMock.Object, engineEventMock.Object);

            Event result = cardCommandHandler.Handler(getCardStatusCommand);
            Assert.True(result is CardStatusIncompletedEvent);
        }

        [Theory]
        [InlineData("44454a2sda3s562a")]
        [InlineData("44454a34sda3s562a")]
        [InlineData("444513a2sda3s562a")]
        public void TrelloService_GetCardStatus_ShouldReturnUnableToFind(string cardId)
        {
            var logMock = new Mock<ILog>();
            var trelloMock = new Mock<ITrello>();
            var cardnotificationMock = new Mock<ICardEventHandler>();
            var engineMock = new Mock<IEngine>();
            var engineEventMock = new Mock<IEngineEventHandler>();

            trelloMock.Setup(x => x.IsTheProcessFinished(It.IsAny<string>()))
               .Returns(Task.FromResult(2));

            GetCardStatusCommand getCardStatusCommand = new GetCardStatusCommand(cardId);
            CardCommandHandler cardCommandHandler = new CardCommandHandler(logMock.Object, trelloMock.Object, cardnotificationMock.Object,
              engineMock.Object, engineEventMock.Object);

            Event result = cardCommandHandler.Handler(getCardStatusCommand);
            Assert.True(result is CardStatusUnableToFindEvent);
        }
    }
}
