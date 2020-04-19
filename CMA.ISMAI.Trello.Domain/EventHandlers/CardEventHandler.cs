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

        public Task Handler(AddCardCompletedEvent notification)
        {
            _eventStore.SaveToEventStore(notification);
            _sendNotificationService.SendNotificationToBroker("trelloismai@gmail.com", $"Card was added with success! - {notification.Timestamp}");
            return Task.CompletedTask;
        }

        public Task Handler(AddCardFailedEvent notification)
        {
            _eventStore.SaveToEventStore(notification);
            _sendNotificationService.SendNotificationToBroker("trelloismai@gmail.com", $"Card adition failed!!, with  {notification.DomainNotifications.Count} errors - {notification.Timestamp}");
            return Task.CompletedTask;
        }

        public Task Handler(CardStatusCompletedEvent notification)
        {
            _eventStore.SaveToEventStore(notification);
            _sendNotificationService.SendNotificationToBroker("trelloismai@gmail.com", $"Card was completed with success! - {notification.Timestamp}");
            return Task.CompletedTask;
        }

        public Task Handler(CardStatusIncompletedEvent notification)
        {
            _eventStore.SaveToEventStore(notification);
            _sendNotificationService.SendNotificationToBroker("trelloismai@gmail.com", $"Card need's to be completed! - {notification.Timestamp}");
            return Task.CompletedTask;
        }

        public Task Handler(CardStatusUnableToFindEvent notification)
        {
            _eventStore.SaveToEventStore(notification);
            _sendNotificationService.SendNotificationToBroker("trelloismai@gmail.com", $"Card not found! - {notification.Timestamp}");
            return Task.CompletedTask;
        }

        public Task Handler(CardHasNotBeenDeletedEvent notification)
        {
            _eventStore.SaveToEventStore(notification);
            _sendNotificationService.SendNotificationToBroker("trelloismai@gmail.com", $"Card had a probem been deleted! - {notification.Timestamp}");
            return Task.CompletedTask;
        }

        public Task Handler(CardHasBeenDeletedEvent notification)
        {
            _eventStore.SaveToEventStore(notification);
            _sendNotificationService.SendNotificationToBroker("trelloismai@gmail.com", $"Card has been deleted! - {notification.Timestamp}");
            return Task.CompletedTask;
        }

        public Task Handler(CardDosentHaveAttchmentsEvent notification)
        {
            _eventStore.SaveToEventStore(notification);
            return Task.CompletedTask;
        }

        public Task Handler(ReturnCardAttachmentsEvent notification)
        {
            _eventStore.SaveToEventStore(notification);
            return Task.CompletedTask;
        }

        public Task Handler(UnableToFindCardAttachmentsEvent notification)
        {
            _eventStore.SaveToEventStore(notification);
            return Task.CompletedTask;
        }
    }
}
