using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.Domain.Commands;
using CMA.ISMAI.Trello.Domain.Enum;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;
using CMA.ISMAI.Trello.Engine.Automation;
using CMA.ISMAI.Trello.Engine.Interface;
using System.Threading.Tasks;

namespace CMA.ISMAI.Trello.Domain.CommandHandlers
{
    public class CardCommandHandler : CommandHandler, ICardCommandHandler
    {
        private readonly ILog _log;
        private readonly ITrello _trello;
        private readonly ICardEventHandler _cardEventHandler;
        private readonly IEngineEventHandler _workFlowEventHandler;
        private readonly IEngine _engine;

        public CardCommandHandler(ILog log, ITrello trello, ICardEventHandler cardEventHandler, IEngine engine,
             IEngineEventHandler workFlowEventHandler)
        {
            _log = log;
            _trello = trello;
            _cardEventHandler = cardEventHandler;
            _engine = engine;
            _workFlowEventHandler = workFlowEventHandler;
        }

        public Event Handler(AddCardCommand request)
        {
            Event @event;
            if (!request.IsValid())
            {
                _log.Fatal("A invalid card was been submited in the Domain");
                @event = new AddCardFailedEvent(NotifyValidationErrors(request), "", request.Name, request.Description, request.DueTime);
                CreateCardEvent(@event as AddCardFailedEvent);
                return @event;
            }
            string cardId = _trello.AddCard(request.Name, request.Description, request.DueTime, request.BoardId, request.FilesUrl).Result;

            return ReturnEventBasedOnCardId(request, cardId);
        }

        public Event Handler(AddCardCommandAndProcess request)
        {
            Event @event;
            if (!request.IsValid())
            {
                _log.Fatal("A invalid card was been submited in the Domain");
                @event = new AddCardFailedEvent(NotifyValidationErrors(request), "", request.Name, request.Description, request.DueTime);
                CreateCardEvent(@event as AddCardFailedEvent);
                return @event;
            }
            string cardId = _trello.AddCard(request.Name, request.Description, request.DueTime, request.BoardId, request.FilesUrl).Result;
            cardId = DeployProcess(cardId, request.CourseName, request.StudentName, request.InstituteName, request.IsCetOrOtherCondition);
            return ReturnEventBasedOnCardId(request, cardId);
        }

        private string DeployProcess(string cardId, string courseName, string studentName, string instituteName, bool IsCetOrOtherCondition)
        {
            Event @event;
            string proccess = _engine.StartWorkFlow(cardId, courseName, studentName, instituteName, IsCetOrOtherCondition);
            if (string.IsNullOrEmpty(proccess))
            {
                _log.Fatal("Proccess Engine couldn't start!");
                @event = new WorkFlowStartFailedEvent("The process engine couldn't start!");
                CreateEngineEvent(@event as WorkFlowStartFailedEvent);
                _trello.DeleteCard(cardId);
                return string.Empty;
            }
            @event = new WorkFlowStartCompletedEvent(proccess, "Creditações");
            CreateEngineEvent(@event as WorkFlowStartCompletedEvent);
            return cardId;
        }

        private Event ReturnEventBasedOnCardId(AddCardCommand request, string cardId)
        {
            Event @event;
            if (string.IsNullOrEmpty(cardId))
            {
                _log.Fatal($"The creation of an card failed! - TimeStamp {request.Timestamp} - AggregateId - {request.AggregateId}");
                @event = new AddCardFailedEvent(NotifyDomainErrors("CardId", "CardId is null or empty!"), cardId, request.Name, request.Description, request.DueTime);
                CreateCardEvent(@event as AddCardFailedEvent);
                return @event;
            }
            @event = new AddCardCompletedEvent(cardId, request.Name, request.Description, request.DueTime);
            CreateCardEvent(@event as AddCardCompletedEvent);
            return @event;
        }

        public Event Handler(GetCardStatusCommand request)
        {
            Event @event;
            int result = _trello.IsTheProcessFinished(request.Id).Result;
            if (result == (int)CardStatus.Completed)
            {
                @event = new CardStatusCompletedEvent(request.Id);
                CreateCardEvent(@event as CardStatusCompletedEvent);
            }
            else if (result == (int)CardStatus.Active)
            {
                @event = new CardStatusIncompletedEvent(request.Id);
                CreateCardEvent(@event as CardStatusIncompletedEvent);
            }
            else
            {
                @event = new CardStatusUnableToFindEvent(request.Id);
                CreateCardEvent(@event as CardStatusUnableToFindEvent);
            }
            return @event;
        }

        public Event Handler(GetCardAttachmentsCommand request)
        {
            Event @event;
            var filesUrl = _trello.ReturnCardAttachmenets(request.CardId).Result;
            if (filesUrl == null)
            {
                @event = new UnableToFindCardAttachmentsEvent(request.CardId);
                CreateCardEvent(@event as UnableToFindCardAttachmentsEvent);
            }
            else if (filesUrl.Count == 0)
            {
                @event = new CardDosentHaveAttchmentsEvent(request.CardId);
                CreateCardEvent(@event as CardDosentHaveAttchmentsEvent);
            }
            else
            {
                @event = new ReturnCardAttachmentsEvent(request.CardId, filesUrl);
                CreateCardEvent(@event as ReturnCardAttachmentsEvent);
            }
            return @event;
        }

        public Event Handler(DeleteCardCommand request)
        {
            Event @event;
            if (_trello.DeleteCard(request.CardId).Result)
            {
                @event = new CardHasBeenDeletedEvent(request.CardId);
                CreateCardEvent(@event as CardHasBeenDeletedEvent);
                return @event;
            }
            @event = new CardHasNotBeenDeletedEvent(request.CardId);
            CreateCardEvent(@event as CardHasNotBeenDeletedEvent);
            return @event;
        }

        private void CreateCardEvent(dynamic @event)
        {
            Task.Run(() => _cardEventHandler.Handler(@event));
        }

        private void CreateEngineEvent(dynamic @event)
        {
            Task.Run(() => _workFlowEventHandler.Handler(@event));
        }
    }
}
