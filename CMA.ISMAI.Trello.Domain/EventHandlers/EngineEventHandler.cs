using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;

namespace CMA.ISMAI.Trello.Domain.EventHandlers
{
    public class EngineEventHandler : NotificationsHandler, IEngineEventHandler
    {
        public void Handler(WorkFlowStartFailedEvent notification)
        {
            SendNotification(new MessageBody("", $"An new deploy failed!!, {notification.Motive} - {notification.Timestamp} - {notification.MessageType}"));
        }
        public void Handler(WorkFlowStartCompletedEvent notification)
        {
            SendNotification(new MessageBody("", $"An new deploy was submited!, {notification.Id} - {notification.Timestamp} - {notification.MessageType}"));
        }
    }
}
