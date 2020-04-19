using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.Domain.CommandHandlers;
using CMA.ISMAI.Trello.Domain.Commands;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;
using CMA.ISMAI.Trello.Engine.Automation;
using CMA.ISMAI.Trello.Engine.Interface;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.UnitTests.Trello.Domain.Commands
{
    public class DomainDeleteCardCommand_Test
    {
        [Trait("CardCommandHandler", "Delete Card")]
        [Fact(DisplayName = "Delete a card - should be deleted")]
        public void CardCommandHandler_DeleteCard_ShouldDeleteTheCard()
        {
            var logMock = new Mock<ILog>();
            var trelloMock = new Mock<ITrello>();
            var cardnotificationMock = new Mock<ICardEventHandler>();
            var engineMock = new Mock<IEngine>();
            var engineEventMock = new Mock<IEngineEventHandler>();
            trelloMock.Setup(x => x.DeleteCard(It.IsAny<string>())).Returns(Task.FromResult(true));
            DeleteCardCommand deleteCardCommand = new DeleteCardCommand(Guid.NewGuid().ToString());
            CardCommandHandler cardCommandHandler = new CardCommandHandler(logMock.Object, trelloMock.Object, cardnotificationMock.Object,
                engineMock.Object, engineEventMock.Object);
            Event result = cardCommandHandler.Handler(deleteCardCommand);
            Assert.True(result is CardHasBeenDeletedEvent);
        }

        [Trait("CardCommandHandler", "Delete Card")]
        [Fact(DisplayName = "Delete a card - should fail to delete the card")]
        public void CardCommandHandler_DeleteCard_ShouldFailToDeleteTheCard()
        {
            var logMock = new Mock<ILog>();
            var trelloMock = new Mock<ITrello>();
            var cardnotificationMock = new Mock<ICardEventHandler>();
            var engineMock = new Mock<IEngine>();
            var engineEventMock = new Mock<IEngineEventHandler>();
            trelloMock.Setup(x => x.DeleteCard(It.IsAny<string>())).Returns(Task.FromResult(false));
            DeleteCardCommand deleteCardCommand = new DeleteCardCommand(Guid.NewGuid().ToString());
            CardCommandHandler cardCommandHandler = new CardCommandHandler(logMock.Object, trelloMock.Object, cardnotificationMock.Object,
                engineMock.Object, engineEventMock.Object);
            Event result = cardCommandHandler.Handler(deleteCardCommand);
            Assert.True(result is CardHasNotBeenDeletedEvent);
        }
    }
}
