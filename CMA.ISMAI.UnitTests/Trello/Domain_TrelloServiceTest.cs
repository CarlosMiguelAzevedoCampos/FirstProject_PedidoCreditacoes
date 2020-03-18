using CMA.ISMAI.Core.Bus;
using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Engine.Domain.Events;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.Domain.CommandHandlers;
using CMA.ISMAI.Trello.Domain.Commands;
using CMA.ISMAI.Trello.Domain.EventHandlers;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Engine.Interface;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.UnitTests.Engine.Domain
{
    public class Domain_TrelloServiceTest
    {
        [Theory]
        [InlineData("ISMAI - Informatica creditaçâo", "")]
        [InlineData("", "Carlos Campos")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void TrelloService_AddCard_ShouldFailBecauseOfNullOrEmptyParameters(string name, string description)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<ITrello>();
            var busMock = new Mock<IMediatorHandler>();
            engineMock.Setup(x => x.AddCard(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(Task.FromResult(string.Empty));

            AddCardCommand addCard = new AddCardCommand(name, DateTime.Now, description);
            CardCommandHandler cardCommandHandler = new CardCommandHandler(busMock.Object, logMock.Object, new DomainNotificationHandler(), engineMock.Object);

            bool result = cardCommandHandler.Handle(addCard, new CancellationToken()).Result;
            Assert.False(result);
        }

        [Theory]
        [InlineData("ISMAI - Informatica creditaçâo", "Carlos Campos")]
        [InlineData("ISMAI - Multimedia creditaçâo", "Miguel Campos")]
        public void TrelloService_AddCard_ShouldReturnTrue(string name, string description)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<ITrello>();
            var busMock = new Mock<IMediatorHandler>();
            engineMock.Setup(x => x.AddCard(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(Task.FromResult(Guid.NewGuid().ToString()));

            AddCardCommand addCard = new AddCardCommand(name, DateTime.Now.AddDays(1), description);
            CardCommandHandler cardCommandHandler = new CardCommandHandler(busMock.Object, logMock.Object, new DomainNotificationHandler(), engineMock.Object);

            bool result = cardCommandHandler.Handle(addCard, new CancellationToken()).Result;
            Assert.True(result);
        }

        [Theory]
        [InlineData("ISMAI - Informatica creditaçâo", "Carlos Campos")]
        [InlineData("ISMAI - Multimedia creditaçâo", "Miguel Campos")]
        public void TrelloService_AddCard_ShouldFail_EngineCrash(string name, string description)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<ITrello>();
            var busMock = new Mock<IMediatorHandler>();
            engineMock.Setup(x => x.AddCard(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(Task.FromResult(string.Empty));

            AddCardCommand addCard = new AddCardCommand(name, DateTime.Now.AddDays(1), description);
            CardCommandHandler cardCommandHandler = new CardCommandHandler(busMock.Object, logMock.Object, new DomainNotificationHandler(), engineMock.Object);

            bool result = cardCommandHandler.Handle(addCard, new CancellationToken()).Result;
            Assert.False(result);
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TrelloService_IsTheProcessFinished_ShouldFailBecauseOfEmptyOrNullParameters(string id)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<ITrello>();
            var busMock = new Mock<IMediatorHandler>();
            ObtainCardStatusCommand cardStatusCommand = new ObtainCardStatusCommand(id);
            CardCommandHandler cardCommandHandler = new CardCommandHandler(busMock.Object, logMock.Object, new DomainNotificationHandler(), engineMock.Object);

            bool result = cardCommandHandler.Handle(cardStatusCommand, new CancellationToken()).Result;
            Assert.False(result);
        }


        [Theory]
        [InlineData("ifh2i992h2b-asfa-1w")]
        [InlineData("cjckamrb222-we-1w")]
        public void TrelloService_IsTheProcessFinished_ShouldFailObtainingTheStatus(string id)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<ITrello>();
            var busMock = new Mock<IMediatorHandler>();
            engineMock.Setup(x => x.IsTheProcessFinished(It.IsAny<string>())).Returns(Task.FromResult(false));

            ObtainCardStatusCommand cardStatusCommand = new ObtainCardStatusCommand(id);
            CardCommandHandler cardCommandHandler = new CardCommandHandler(busMock.Object, logMock.Object, new DomainNotificationHandler(), engineMock.Object);

            bool result = cardCommandHandler.Handle(cardStatusCommand, new CancellationToken()).Result;
            Assert.False(result);
        }

        [Theory]
        [InlineData("ifh2i992h2b-asfa-1w")]
        [InlineData("cjckamrb222-we-1w")]
        public void TrelloService_IsTheProcessFinished_ShouldPass(string id)
        {
            var logMock = new Mock<ILog>();
            var engineMock = new Mock<ITrello>();
            var busMock = new Mock<IMediatorHandler>();
            engineMock.Setup(x => x.IsTheProcessFinished(It.IsAny<string>())).Returns(Task.FromResult(true));

            ObtainCardStatusCommand cardStatusCommand = new ObtainCardStatusCommand(id);
            CardCommandHandler cardCommandHandler = new CardCommandHandler(busMock.Object, logMock.Object, new DomainNotificationHandler(), engineMock.Object);

            bool result = cardCommandHandler.Handle(cardStatusCommand, new CancellationToken()).Result;
            Assert.True(result);
        }



        [Theory]
        [InlineData("ISMAI", "Process_00kjdw0")]
        [InlineData("ISMAI", "Process_00kjd12")]
        public void TrelloService_AddEventCompleted_ShouldReturnEventCompleted(string name, string description)
        {
            AddCardCompletedEvent addCardCompletedEvent = new AddCardCompletedEvent(Guid.NewGuid(), name, description, DateTime.Now);
            CardEventHandler cardEventHandler = new CardEventHandler();

            bool result = cardEventHandler.Handle(addCardCompletedEvent, new CancellationToken()).IsCompleted;
            Assert.True(result);
        }

        [Fact]
        public void TrelloService_CardStatusCompleted_ShouldReturnEventCompleted()
        { 
            CardCompletedStatusEvent cardCompletedStatusEvent = new CardCompletedStatusEvent(Guid.NewGuid().ToString());
            CardEventHandler cardEventHandler = new CardEventHandler();

            bool result = cardEventHandler.Handle(cardCompletedStatusEvent, new CancellationToken()).IsCompleted;
            Assert.True(result);
        }

        [Fact]
        public void TrelloService_CardIStatusIncompleted_ShouldReturnEventCompleted()
        {
            CardIncompletedStatusEvent cardIncompletedStatusEvent = new CardIncompletedStatusEvent(Guid.NewGuid().ToString());
            CardEventHandler cardEventHandler = new CardEventHandler();

            bool result = cardEventHandler.Handle(cardIncompletedStatusEvent, new CancellationToken()).IsCompleted;
            Assert.True(result);
        }

    }
}
