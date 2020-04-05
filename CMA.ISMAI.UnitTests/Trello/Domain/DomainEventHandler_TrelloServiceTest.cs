using CMA.ISMAI.Core.Events.Store.Interface;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.Domain.EventHandlers;
using CMA.ISMAI.Trello.Domain.Events;
using Moq;
using System.Threading.Tasks;
using Xunit;
using System;
using CMA.ISMAI.Core.Notifications;
using System.Collections.Generic;

namespace CMA.ISMAI.UnitTests.Trello.Domain
{
    public class DomainEventHandler_TrelloServiceTest
    {
        [Fact]
        public void TrelloService_CardEventHandler_AddCardCompletedEventNotification()
        {
            var logMock = new Mock<ILog>();
            var eventStoreMock = new Mock<IEventStore>();

            CardEventHandler cardCommandHandler = new CardEventHandler(eventStoreMock.Object, logMock.Object);
            Task result = cardCommandHandler.Handler(new AddCardCompletedEvent(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<DateTime>()));

            Assert.True(result.IsCompleted);
        }

        [Fact]
        public void TrelloService_CardEventHandler_AddCardFailedEventNotification()
        {
            var logMock = new Mock<ILog>();
            var eventStoreMock = new Mock<IEventStore>();

            CardEventHandler cardCommandHandler = new CardEventHandler(eventStoreMock.Object, logMock.Object);
            Task result = cardCommandHandler.Handler(new AddCardFailedEvent(new List<DomainNotification>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<DateTime>()));

            Assert.True(result.IsCompleted);
        }

        [Fact]
        public void TrelloService_CardEventHandler_CardStatusCompletedEventNotification()
        {
            var logMock = new Mock<ILog>();
            var eventStoreMock = new Mock<IEventStore>();

            CardEventHandler cardCommandHandler = new CardEventHandler(eventStoreMock.Object, logMock.Object);
            Task result = cardCommandHandler.Handler(new CardStatusCompletedEvent(It.IsAny<string>()));

            Assert.True(result.IsCompleted);
        }

        [Fact]
        public void TrelloService_CardEventHandler_CardStatusIncompletedEventNotification()
        {
            var logMock = new Mock<ILog>();
            var eventStoreMock = new Mock<IEventStore>();

            CardEventHandler cardCommandHandler = new CardEventHandler(eventStoreMock.Object, logMock.Object);
            Task result = cardCommandHandler.Handler(new CardStatusIncompletedEvent(It.IsAny<string>()));

            Assert.True(result.IsCompleted);
        }

        [Fact]
        public void TrelloService_CardEventHandler_CardStatusUnableToFindEventNotification()
        {
            var logMock = new Mock<ILog>();
            var eventStoreMock = new Mock<IEventStore>();

            CardEventHandler cardCommandHandler = new CardEventHandler(eventStoreMock.Object, logMock.Object);
            Task result = cardCommandHandler.Handler(new CardStatusUnableToFindEvent(It.IsAny<string>()));

            Assert.True(result.IsCompleted);
        }

        [Fact]
        public void TrelloService_EngineEventHandler_WorkFlowStartFailedEventNotification()
        {
            var logMock = new Mock<ILog>();
            var eventStoreMock = new Mock<IEventStore>();

            EngineEventHandler engineEventHandler = new EngineEventHandler(eventStoreMock.Object, logMock.Object);
            Task result = engineEventHandler.Handler(new WorkFlowStartFailedEvent(It.IsAny<string>()));

            Assert.True(result.IsCompleted);
        }

        [Fact]
        public void TrelloService_EngineEventHandler_CardStatusUnableToFindEventNotification()
        {
            var logMock = new Mock<ILog>();
            var eventStoreMock = new Mock<IEventStore>();

            EngineEventHandler engineEventHandler = new EngineEventHandler(eventStoreMock.Object, logMock.Object);
            Task result = engineEventHandler.Handler(new WorkFlowStartCompletedEvent(It.IsAny<string>(), It.IsAny<string>()));

            Assert.True(result.IsCompleted);
        }
    }
}
