using CMA.ISMAI.Core.Events.Store.Interface;
using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;
using System.Threading.Tasks;

namespace CMA.ISMAI.Trello.Domain.EventHandlers
{
    public class CardEventHandler : NotificationsHandler, ICardEventHandler
    {
        public CardEventHandler(IEventStore eventStore, ILog log) : base(eventStore, log)
        {
        }

        public Task Handler(AddCardCompletedEvent notification)
        {
            Task.Run(() => SaveEvent(notification));
            Task.Run(() => SendNotification(new MessageBody("trelloismai@gmail.com", $"Card was added with success!, {notification.Id} - {notification.Timestamp} - {notification.MessageType} - {notification.AggregateId} - {notification.DueTime} - {notification.Name} - {notification.Description} ")));
            return Task.CompletedTask;
        }

        public Task Handler(AddCardFailedEvent notification)
        {
            Task.Run(() => SaveEvent(notification));
            Task.Run(() => SendNotification(new MessageBody("trelloismai@gmail.com", $"Card adition failed!!, with  {notification.DomainNotifications.Count} errors -  {notification.Id} - {notification.Timestamp} - {notification.MessageType} - {notification.AggregateId} - {notification.DueTime} - {notification.Name} - {notification.Description} ")));
            return Task.CompletedTask;

        }

        public Task Handler(CardStatusCompletedEvent notification)
        {
            Task.Run(() => SaveEvent(notification));
            Task.Run(() => SendNotification(new MessageBody("trelloismai@gmail.com", $"Card was completed with success!, {notification.Id} - {notification.Timestamp} - {notification.MessageType} - {notification.AggregateId}")));
            return Task.CompletedTask;
        }

        public Task Handler(CardStatusIncompletedEvent notification)
        {
            Task.Run(() => SaveEvent(notification));
            Task.Run(() => SendNotification(new MessageBody("trelloismai@gmail.com", $"Card need's to be completed!, {notification.Id} - {notification.Timestamp} - {notification.MessageType} - {notification.AggregateId}")));
            return Task.CompletedTask;
        }

        public Task Handler(CardStatusUnableToFindEvent notification)
        {
            Task.Run(() => SaveEvent(notification));
            Task.Run(() => SendNotification(new MessageBody("trelloismai@gmail.com", $"Card not found!, {notification.Id} - {notification.Timestamp} - {notification.MessageType} - {notification.AggregateId}")));
            return Task.CompletedTask;
        }
    }
}
