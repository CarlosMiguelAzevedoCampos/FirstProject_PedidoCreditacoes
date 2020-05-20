using CMA.ISMAI.Core.Events.Store.Interface;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;
using System.Threading.Tasks;

namespace CMA.ISMAI.Trello.Domain.EventHandlers
{
    public class CardEventHandler : ICardEventHandler
    {
        private readonly IEventStore _eventStore;

        public CardEventHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public Task Handler(AddCardCompletedEvent notification)
        {
            _eventStore.SaveToEventStore(notification);
            return Task.CompletedTask;
        }

        public Task Handler(AddCardFailedEvent notification)
        {
            _eventStore.SaveToEventStore(notification);
            return Task.CompletedTask;
        }

        public Task Handler(CardStatusCompletedEvent notification)
        {
            _eventStore.SaveToEventStore(notification);
            return Task.CompletedTask;
        }

        public Task Handler(CardStatusIncompletedEvent notification)
        {
            _eventStore.SaveToEventStore(notification);
            return Task.CompletedTask;
        }

        public Task Handler(CardStatusUnableToFindEvent notification)
        {
            _eventStore.SaveToEventStore(notification);
            return Task.CompletedTask;
        }

        public Task Handler(CardHasNotBeenDeletedEvent notification)
        {
            _eventStore.SaveToEventStore(notification);
            return Task.CompletedTask;
        }

        public Task Handler(CardHasBeenDeletedEvent notification)
        {
            _eventStore.SaveToEventStore(notification);
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
