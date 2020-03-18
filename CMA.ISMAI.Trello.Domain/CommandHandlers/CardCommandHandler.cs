using CMA.ISMAI.Core.Bus;
using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.Domain.Commands;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Engine.Interface;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CMA.ISMAI.Trello.Domain.CommandHandlers
{
    public class CardCommandHandler : CommandHandler,
        IRequestHandler<AddCardCommand, bool>,
        IRequestHandler<CardStatusCommand, bool>
    {
        private readonly IMediatorHandler Bus;
        private readonly ILog _log;
        private readonly ITrello _trello;


        public CardCommandHandler(IMediatorHandler bus, ILog log, INotificationHandler<DomainNotification> notifications, ITrello trello) : base(bus, notifications)
        {
            Bus = bus;
            _log = log;
            _trello = trello;
        }

        public Task<bool> Handle(AddCardCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                _log.Fatal("A invalid card was been submited in the Domain");
                NotifyValidationErrors(request);
                return Task.FromResult(false);
            }
            string cardId = _trello.AddCard(request.Name, request.Description, request.DueTime).Result;
            if (string.IsNullOrEmpty(cardId))
            {
                _log.Fatal($"The creation of an card failed! - TimeStamp {request.Timestamp} - AggregateId - {request.AggregateId}");
                Bus.RaiseEvent(new DomainNotification(request.MessageType, "The creation of an card failed"));
                return Task.FromResult(false);
            }
            Bus.RaiseEvent(new AddCardCompletedEvent(request.Id, request.Name, request.Description, request.DueTime));
            return Task.FromResult(true);
        }

        public Task<bool> Handle(CardStatusCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                _log.Fatal("A invalid id was been submited in the Domain");
                NotifyValidationErrors(request);
                return Task.FromResult(false);
            }
            if (_trello.IsTheProcessFinished(request.Id).Result)
            {
                Bus.RaiseEvent(new CardCompletedStatusEvent(request.Id));
                return Task.FromResult(true);
            }
            Bus.RaiseEvent(new CardIncompletedStatusEvent(request.Id));
            return Task.FromResult(false);
        }
    }
}
