using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Engine.Domain.Events;
using CMA.ISMAI.Engine.Domain.Interface;

namespace CMA.ISMAI.Engine.Domain.EventHandlers
{
    public class WorkFlowEventHandler : NotificationsHandler, IWorkflowEventHandler
    {
        public void Handle(WorkFlowStartCompletedEvent notification)
        {
           SendNotification(new MessageBody("", $"An new deploy was submited!, {notification.Id} - {notification.Timestamp} - {notification.MessageType}"));
        }

        public void Handle(WorkFlowStartFailedEvent notification)
        {
            SendNotification(new MessageBody("", $"An new deploy failed!!, with {notification.DomainNotifications.Count} errors! - {notification.Timestamp} - {notification.MessageType}"));
        }
    }
}
