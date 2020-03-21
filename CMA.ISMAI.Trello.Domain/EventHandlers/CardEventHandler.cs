using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;

namespace CMA.ISMAI.Trello.Domain.EventHandlers
{
    public class CardEventHandler : NotificationsHandler, ICardEventHandler
    {
        public void Handler(AddCardCompletedEvent request)
        {
            SendNotification(new MessageBody("", $"Card was added with success!, {request.Id} - {request.Timestamp} - {request.MessageType}"));
        }

        public void Handler(AddCardFailedEvent request)
        {
            SendNotification(new MessageBody("", $"Card adition failed!!, with {request.DomainNotifications.Count} errors - {request.Timestamp} - {request.MessageType}"));
        }

        public void Handler(CardStatusCompletedEvent request)
        {
            SendNotification(new MessageBody("", $"Card was completed with success!, {request.Id} - {request.Timestamp} - {request.MessageType}"));
        }

        public void Handler(CardStatusIncompletedEvent request)
        {
            SendNotification(new MessageBody("", $"Card need's to be completed!, {request.Id} - {request.Timestamp} - {request.MessageType}"));
        }

        public void Handler(CardStatusUnableToFindEvent request)
        {
            SendNotification(new MessageBody("", $"Card not found!, {request.Id} - {request.Timestamp} - {request.MessageType}"));
        }
    }
}
