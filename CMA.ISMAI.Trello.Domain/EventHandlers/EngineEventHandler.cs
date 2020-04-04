using CMA.ISMAI.Core.Events.Store.Interface;
using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;

namespace CMA.ISMAI.Trello.Domain.EventHandlers
{
    public class EngineEventHandler : NotificationsHandler, IEngineEventHandler
    {
        public EngineEventHandler(IEventStore eventStore) : base(eventStore)
        {
        }
        public void Handler(WorkFlowStartFailedEvent notification)
        {
            SaveEvent(notification);
            SendNotification(new MessageBody("", $"An new deploy failed!!, {notification.Motive} - {notification.Timestamp} - {notification.MessageType}"));
        }
        public void Handler(WorkFlowStartCompletedEvent notification)
        {
            SaveEvent(notification);
            SendNotification(new MessageBody("", $"An new deploy was submited!, {notification.Id} - {notification.Timestamp} - {notification.MessageType}"));
        }
    }
}
