using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Core.Events.Store.Interface;
using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Trello.Domain.EventHandlers;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.MessageBroker.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.UnitTests.Trello.Domain.Events
{
    public class CardEventHandler_Test
    {
        [Trait("CardEventHandler", "Add Card")]
        [Fact(DisplayName = "New card creation should generate this event")]
        public void CardEventHandler_CardEventHandler_AddCardCompletedEventNotification()
        {
            var serviceNotificationMock = new Mock<ISendNotificationService>();
            var eventStoreMock = new Mock<IEventStore>();

            CardEventHandler cardCommandHandler = new CardEventHandler(eventStoreMock.Object, serviceNotificationMock.Object);
            Task result =  cardCommandHandler.Handler(new AddCardCompletedEvent(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<DateTime>()));

            serviceNotificationMock.Verify(x => x.SendNotificationToBroker(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            eventStoreMock.Verify(x => x.SaveToEventStore(It.IsAny<Event>()), Times.Once);
            Assert.True(result.IsCompleted);
        }

        [Trait("CardEventHandler", "Add Card")]
        [Fact(DisplayName = "Card creation error should generate this event")]
        public void CardEventHandler_CardEventHandler_AddCardFailedEventNotification()
        {
            var serviceNotificationMock = new Mock<ISendNotificationService>();
            var eventStoreMock = new Mock<IEventStore>();

            CardEventHandler cardCommandHandler = new CardEventHandler(eventStoreMock.Object, serviceNotificationMock.Object);
           Task result = cardCommandHandler.Handler(new AddCardFailedEvent(new List<DomainNotification>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<DateTime>()));

            serviceNotificationMock.Verify(x => x.SendNotificationToBroker(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            eventStoreMock.Verify(x => x.SaveToEventStore(It.IsAny<Event>()), Times.Once);
            Assert.True(result.IsCompleted);
        }

        [Trait("CardEventHandler", "Card Status")]
        [Fact(DisplayName = "Card complete should generate this event")]
        public void CardEventHandler_CardEventHandler_CardStatusCompletedEventNotification()
        {
            var serviceNotificationMock = new Mock<ISendNotificationService>();
            var eventStoreMock = new Mock<IEventStore>();

            CardEventHandler cardCommandHandler = new CardEventHandler(eventStoreMock.Object, serviceNotificationMock.Object);
           Task result = cardCommandHandler.Handler(new CardStatusCompletedEvent(It.IsAny<string>()));

            serviceNotificationMock.Verify(x => x.SendNotificationToBroker(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            eventStoreMock.Verify(x => x.SaveToEventStore(It.IsAny<Event>()), Times.Once);
            Assert.True(result.IsCompleted);
        }

        [Trait("CardEventHandler", "Card Status")]
        [Fact(DisplayName = "A incompleted should generate this event")]
        public void CardEventHandler_CardEventHandler_CardStatusIncompletedEventNotification()
        {
            var serviceNotificationMock = new Mock<ISendNotificationService>();
            var eventStoreMock = new Mock<IEventStore>();

            CardEventHandler cardCommandHandler = new CardEventHandler(eventStoreMock.Object, serviceNotificationMock.Object);
            Task result = cardCommandHandler.Handler(new CardStatusIncompletedEvent(It.IsAny<string>()));

            serviceNotificationMock.Verify(x => x.SendNotificationToBroker(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            eventStoreMock.Verify(x => x.SaveToEventStore(It.IsAny<Event>()), Times.Once);
            Assert.True(result.IsCompleted);
        }

        [Trait("CardEventHandler", "Card Status")]
        [Fact(DisplayName = "Card unable to find should generate this event")]
        public void CardEventHandler_CardEventHandler_CardStatusUnableToFindEventNotification()
        {
            var serviceNotificationMock = new Mock<ISendNotificationService>();
            var eventStoreMock = new Mock<IEventStore>();

            CardEventHandler cardCommandHandler = new CardEventHandler(eventStoreMock.Object, serviceNotificationMock.Object);
           Task result= cardCommandHandler.Handler(new CardStatusUnableToFindEvent(It.IsAny<string>()));

            serviceNotificationMock.Verify(x => x.SendNotificationToBroker(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            eventStoreMock.Verify(x => x.SaveToEventStore(It.IsAny<Event>()), Times.Once);
            Assert.True(result.IsCompleted);
        }

        [Trait("CardEventHandler", "Card Status")]
        [Fact(DisplayName = "Card has been deleted event")]
        public void CardEventHandler_CardEventHandler_CardBeenDeletedEvent()
        {
            var serviceNotificationMock = new Mock<ISendNotificationService>();
            var eventStoreMock = new Mock<IEventStore>();

            CardEventHandler cardCommandHandler = new CardEventHandler(eventStoreMock.Object, serviceNotificationMock.Object);
            Task result =  cardCommandHandler.Handler(new CardHasBeenDeletedEvent(It.IsAny<string>()));

            serviceNotificationMock.Verify(x => x.SendNotificationToBroker(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            eventStoreMock.Verify(x => x.SaveToEventStore(It.IsAny<Event>()), Times.Once);
            Assert.True(result.IsCompleted);
        }

        [Trait("CardEventHandler", "Card Status")]
        [Fact(DisplayName = "Card has been deleted event")]
        public void CardEventHandler_CardEventHandler_CardHasNotBeenDeletedEvent()
        {
            var serviceNotificationMock = new Mock<ISendNotificationService>();
            var eventStoreMock = new Mock<IEventStore>();

            CardEventHandler cardCommandHandler = new CardEventHandler(eventStoreMock.Object, serviceNotificationMock.Object);
            Task result = cardCommandHandler.Handler(new CardHasNotBeenDeletedEvent(It.IsAny<string>()));

            serviceNotificationMock.Verify(x => x.SendNotificationToBroker(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            eventStoreMock.Verify(x => x.SaveToEventStore(It.IsAny<Event>()), Times.Once);
            Assert.True(result.IsCompleted);
        }


        [Trait("CardEventHandler", "Card Status")]
        [Fact(DisplayName = "Card dosen't have attachments Event")]
        public void CardEventHandler_CardEventHandler_CardDosentHaveAttchmentsEvent()
        {
            var serviceNotificationMock = new Mock<ISendNotificationService>();
            var eventStoreMock = new Mock<IEventStore>();

            CardEventHandler cardCommandHandler = new CardEventHandler(eventStoreMock.Object, serviceNotificationMock.Object);
            Task result = cardCommandHandler.Handler(new CardDosentHaveAttchmentsEvent(It.IsAny<string>()));

            eventStoreMock.Verify(x => x.SaveToEventStore(It.IsAny<Event>()), Times.Once);
            Assert.True(result.IsCompleted);
        }


        [Trait("CardEventHandler", "Card Status")]
        [Fact(DisplayName = "Card has attachments Event")]
        public void CardEventHandler_CardEventHandler_ReturnCardAttachmentsEvent()
        {
            var serviceNotificationMock = new Mock<ISendNotificationService>();
            var eventStoreMock = new Mock<IEventStore>();

            CardEventHandler cardCommandHandler = new CardEventHandler(eventStoreMock.Object, serviceNotificationMock.Object);
            Task result = cardCommandHandler.Handler(new ReturnCardAttachmentsEvent(It.IsAny<string>(), It.IsAny<List<string>>()));

            eventStoreMock.Verify(x => x.SaveToEventStore(It.IsAny<Event>()), Times.Once);
            Assert.True(result.IsCompleted);
        }

        [Trait("CardEventHandler", "Card Status")]
        [Fact(DisplayName = "Unable to find card Attachments Event")]
        public void CardEventHandler_CardEventHandler_UnableToFindCardAttachmentsEvent()
        {
            var serviceNotificationMock = new Mock<ISendNotificationService>();
            var eventStoreMock = new Mock<IEventStore>();

            CardEventHandler cardCommandHandler = new CardEventHandler(eventStoreMock.Object, serviceNotificationMock.Object);
            Task result = cardCommandHandler.Handler(new UnableToFindCardAttachmentsEvent(It.IsAny<string>()));

            eventStoreMock.Verify(x => x.SaveToEventStore(It.IsAny<Event>()), Times.Once);
            Assert.True(result.IsCompleted);
        }
    }
}
