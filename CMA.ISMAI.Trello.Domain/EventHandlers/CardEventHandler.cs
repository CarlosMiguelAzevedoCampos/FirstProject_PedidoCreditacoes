using CMA.ISMAI.Core.Events.Store.Interface;
using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;

namespace CMA.ISMAI.Trello.Domain.EventHandlers
{
    public class CardEventHandler : NotificationsHandler, ICardEventHandler
    {
        public CardEventHandler(IEventStore eventStore) : base(eventStore)
        {
        }

        public void Handler(AddCardCompletedEvent notification)
        {
            SaveEvent(notification);
            SendNotification(new MessageBody("trelloismai@gmail.com", $"Card was added with success!, {notification.Id} - {notification.Timestamp} - {notification.MessageType}"));
        }

        public void Handler(AddCardFailedEvent notification)
        {
            SaveEvent(notification);
            SendNotification(new MessageBody("trelloismai@gmail.com", $"Card adition failed!!, with {notification.DomainNotifications.Count} errors - {notification.Timestamp} - {notification.MessageType}"));
        }

        public void Handler(CardStatusCompletedEvent notification)
        {
            SaveEvent(notification);
            SendNotification(new MessageBody("trelloismai@gmail.com", $"Card was completed with success!, {notification.Id} - {notification.Timestamp} - {notification.MessageType}"));
        }

        public void Handler(CardStatusIncompletedEvent notification)
        {
            SaveEvent(notification);
            SendNotification(new MessageBody("trelloismai@gmail.com", $"Card need's to be completed!, {notification.Id} - {notification.Timestamp} - {notification.MessageType}"));
        }

        public void Handler(CardStatusUnableToFindEvent notification)
        {
            SaveEvent(notification);
            SendNotification(new MessageBody("trelloismai@gmail.com", $"Card not found!, {notification.Id} - {notification.Timestamp} - {notification.MessageType}"));
        }
    }
}
