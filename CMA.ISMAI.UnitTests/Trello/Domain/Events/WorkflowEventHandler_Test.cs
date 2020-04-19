using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Core.Events.Store.Interface;
using CMA.ISMAI.Trello.Domain.EventHandlers;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.MessageBroker.Interface;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CMA.ISMAI.UnitTests.Trello.Domain.Events
{
    public class WorkflowEventHandler_Test
    {
        [Trait("CardEventHandler", "Add card and Process")]
        [Fact(DisplayName = "Workflow engine fail should generate this event")]
        public void WorkFlowEventHandler_EngineEventHandler_WorkFlowStartFailedEventNotification()
        {
            var serviceNotificationMock = new Mock<ISendNotificationService>();
            var eventStoreMock = new Mock<IEventStore>();

            EngineEventHandler engineEventHandler = new EngineEventHandler(eventStoreMock.Object, serviceNotificationMock.Object);
            Task result = engineEventHandler.Handler(new WorkFlowStartFailedEvent(It.IsAny<string>()));
            eventStoreMock.Verify(x => x.SaveToEventStore(It.IsAny<Event>()), Times.Once);
            serviceNotificationMock.Verify(x => x.SendNotificationToBroker(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.True(result.IsCompleted);

        }

        [Trait("CardEventHandler", "Add card and Process")]
        [Fact(DisplayName = "Workflow engine start should generate this event")]
        public void WorkFlowEventHandler_EngineEventHandler_WorkFlowStartCompletedEventNotification()
        {
            var serviceNotificationMock = new Mock<ISendNotificationService>();
            var eventStoreMock = new Mock<IEventStore>();

            EngineEventHandler engineEventHandler = new EngineEventHandler(eventStoreMock.Object, serviceNotificationMock.Object);
            Task result = engineEventHandler.Handler(new WorkFlowStartCompletedEvent(It.IsAny<string>(), It.IsAny<string>()));

            serviceNotificationMock.Verify(x => x.SendNotificationToBroker(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            eventStoreMock.Verify(x => x.SaveToEventStore(It.IsAny<Event>()), Times.Once);
            Assert.True(result.IsCompleted);
        }
    }
}
