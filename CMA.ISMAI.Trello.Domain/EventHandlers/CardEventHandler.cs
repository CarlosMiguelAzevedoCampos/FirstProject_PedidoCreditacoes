using CMA.ISMAI.Trello.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CMA.ISMAI.Trello.Domain.EventHandlers
{
    public class CardEventHandler : INotificationHandler<AddCardCompletedEvent>,
        INotificationHandler<CardCompletedStatusEvent>, INotificationHandler<CardIncompletedStatusEvent>
    {
        public Task Handle(AddCardCompletedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(CardIncompletedStatusEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(CardCompletedStatusEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
