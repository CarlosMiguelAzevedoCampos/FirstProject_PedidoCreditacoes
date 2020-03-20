using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.Domain.Commands;
using CMA.ISMAI.Trello.Domain.Enum;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;
using CMA.ISMAI.Trello.Engine.Interface;

namespace CMA.ISMAI.Trello.Domain.CommandHandlers
{
    public class CardCommandHandler : CommandHandler, ICardCommandHandler
    {
        private readonly ILog _log;
        private readonly ITrello _trello;
        private readonly ICardEventHandler _cardEventHandler;

        public CardCommandHandler(ILog log, ITrello trello, ICardEventHandler cardEventHandler)
        {
            _log = log;
            _trello = trello;
            this._cardEventHandler = cardEventHandler;
        }

        public Event Handler(AddCardCommand request)
        {
            Event @event;
            if (!request.IsValid())
            {
                _log.Fatal("A invalid card was been submited in the Domain");
                @event = new AddCardFailedEvent(NotifyValidationErrors(request));
                _cardEventHandler.Handler(@event as AddCardFailedEvent);
                return @event;
            }
            string cardId = _trello.AddCard(request.Name, request.Description, request.DueTime).Result;
            return ReturnEventBasedOnCardId(request, cardId);
        }
        private Event ReturnEventBasedOnCardId(AddCardCommand request, string cardId)
        {
            Event @event;
            if (string.IsNullOrEmpty(cardId))
            {
                _log.Fatal($"The creation of an card failed! - TimeStamp {request.Timestamp} - AggregateId - {request.AggregateId}");
                @event = new AddCardFailedEvent(NotifyDomainErros("CardId", "CardId is null or empty!"));
                _cardEventHandler.Handler(@event as AddCardFailedEvent);
                return @event;
            }
            return new AddCardCompletedEvent(cardId, request.Name, request.Description, request.DueTime);
        }

        public Event Handler(GetCardStatusCommand request)
        {
            Event @event;
            int result = _trello.IsTheProcessFinished(request.Id).Result;
            if (result == (int)CardStatus.Completed)
            {
                @event = new CardStatusCompletedEvent(request.Id);
                _cardEventHandler.Handler(@event as CardStatusCompletedEvent);
                return @event;
            }
            else if(result == (int)CardStatus.Active)
            {
                @event = new CardStatusIncompletedEvent(request.Id);
                _cardEventHandler.Handler(@event as CardStatusIncompletedEvent);
                return @event;
            }
            else
            {
                @event = new CardStatusUnableToFindEvent(request.Id);
                _cardEventHandler.Handler(@event as CardStatusUnableToFindEvent);
                return @event;
            }
        }
    }
}
