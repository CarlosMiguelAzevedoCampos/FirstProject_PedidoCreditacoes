using CMA.ISMAI.Core.Events.Store.Interface;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;
using CMA.ISMAI.Trello.MessageBroker.Interface;
using System.Threading.Tasks;

namespace CMA.ISMAI.Trello.Domain.EventHandlers
{
    public class CardEventHandler : ICardEventHandler
    {
        private readonly IEventStore _eventStore;
        private readonly ISendNotificationService _sendNotificationService;

        public CardEventHandler(IEventStore eventStore, ISendNotificationService sendNotificationService)
        {
            _eventStore = eventStore;
            _sendNotificationService = sendNotificationService;
        }

        public void Handler(AddCardCompletedEvent notification)
        {
            Task.Run(() => _eventStore.SaveToEventStore(notification));
            Task.Run(() => _sendNotificationService.SendNotificationToBroker("trelloismai@gmail.com", $"Card was added with success!, {notification.Id} - {notification.Timestamp} - {notification.MessageType} - {notification.AggregateId} - {notification.DueTime} - {notification.Name} - {notification.Description} "));
        }

        public void Handler(AddCardFailedEvent notification)
        {
            Task.Run(() => _eventStore.SaveToEventStore(notification));
            Task.Run(() => _sendNotificationService.SendNotificationToBroker("trelloismai@gmail.com", $"Card adition failed!!, with  {notification.DomainNotifications.Count} errors -  {notification.Id} - {notification.Timestamp} - {notification.MessageType} - {notification.AggregateId} - {notification.DueTime} - {notification.Name} - {notification.Description} "));
        }

        public void Handler(CardStatusCompletedEvent notification)
        {
            Task.Run(() => _eventStore.SaveToEventStore(notification));
            Task.Run(() => _sendNotificationService.SendNotificationToBroker("trelloismai@gmail.com", $"Card was completed with success!, {notification.Id} - {notification.Timestamp} - {notification.MessageType} - {notification.AggregateId}"));
        }

        public void Handler(CardStatusIncompletedEvent notification)
        {
            Task.Run(() => _eventStore.SaveToEventStore(notification));
            Task.Run(() => _sendNotificationService.SendNotificationToBroker("trelloismai@gmail.com", $"Card need's to be completed!, {notification.Id} - {notification.Timestamp} - {notification.MessageType} - {notification.AggregateId}"));
        }

        public void Handler(CardStatusUnableToFindEvent notification)
        {
            Task.Run(() => _eventStore.SaveToEventStore(notification));
            Task.Run(() => _sendNotificationService.SendNotificationToBroker("trelloismai@gmail.com", $"Card not found!, {notification.Id} - {notification.Timestamp} - {notification.MessageType} - {notification.AggregateId}"));
        }

        public void Handler(CardHasNotBeenDeletedEvent notification)
        {
            Task.Run(() => _eventStore.SaveToEventStore(notification));
            Task.Run(() => _sendNotificationService.SendNotificationToBroker("trelloismai@gmail.com", $"Card had a probem been deleted!, {notification.CardId} - {notification.Timestamp} - {notification.MessageType} - {notification.AggregateId}"));
        }

        public void Handler(CardHasBeenDeletedEvent notification)
        {
            Task.Run(() => _eventStore.SaveToEventStore(notification));
            Task.Run(() => _sendNotificationService.SendNotificationToBroker("trelloismai@gmail.com", $"Card has been deleted!, {notification.CardId} - {notification.Timestamp} - {notification.MessageType} - {notification.AggregateId}"));
        }

        public void Handler(CardDosentHaveAttchmentsEvent notification)
        {
            Task.Run(() => _eventStore.SaveToEventStore(notification));
        }

        public void Handler(ReturnCardAttachmentsEvent notification)
        {
            Task.Run(() => _eventStore.SaveToEventStore(notification));
        }

        public void Handler(UnableToFindCardAttachmentsEvent notification)
        {
            Task.Run(() => _eventStore.SaveToEventStore(notification));
        }
    }
}
