using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.Domain.Commands;
using CMA.ISMAI.Trello.Domain.Enum;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;
using CMA.ISMAI.Trello.Engine.Automation;
using CMA.ISMAI.Trello.Engine.Interface;
using System.Collections.Generic;

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
                _cardEventHandler.Handler(@event as AddCardFailedEvent);
                return @event;
            }
            string cardId = _trello.AddCard(request.Name, request.Description, request.DueTime, request.BoardId, request.FilesUrl).Result;
           
            return ReturnEventBasedOnCardId(request, cardId);
        }

        private Event ReturnEventBasedOnCardId(AddCardCommand request, string cardId)
        {
            Event @event;
            if (string.IsNullOrEmpty(cardId))
            {
                _log.Fatal($"The creation of an card failed! - TimeStamp {request.Timestamp} - AggregateId - {request.AggregateId}");
                @event = new AddCardFailedEvent(NotifyDomainErrors("CardId", "CardId is null or empty!"), cardId, request.Name, request.Description, request.DueTime);
                _cardEventHandler.Handler(@event as AddCardFailedEvent);
                return @event;
            }
            @event = new AddCardCompletedEvent(cardId, request.Name, request.Description, request.DueTime);
            _cardEventHandler.Handler(@event as AddCardCompletedEvent);
            return @event;
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
            else if (result == (int)CardStatus.Active)
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

        public Event Handler(GetCardAttachmentsCommand request)
        {
            List<string> filesUrl = _trello.ReturnCardAttachmenets(request.CardId).Result;
            return new ReturnCardAttachmentsEvent(request.CardId, filesUrl);
        }

        public Event HandlerProcess(AddCardCommand request)
        {
            Event @event;
            if (!request.IsValid())
            {
                _log.Fatal("A invalid card was been submited in the Domain");
                @event = new AddCardFailedEvent(NotifyValidationErrors(request), "", request.Name, request.Description, request.DueTime);
                _cardEventHandler.Handler(@event as AddCardFailedEvent);
                return @event;
            }
            string cardId = _trello.AddCard(request.Name, request.Description, request.DueTime, request.BoardId, request.FilesUrl).Result;
            DeployProccess(cardId, request.CourseName, request.StudentName, request.InstituteName, request.IsCet);
            return ReturnEventBasedOnCardId(request, cardId);
        }

        private Event DeployProccess(string cardId, string courseName, string studentName, string instituteName, bool isCet)
        {
            Event @event;
            string proccess = _engine.StartWorkFlow(cardId, courseName, studentName, instituteName, isCet);
            if (string.IsNullOrEmpty(proccess))
            {
                _log.Fatal("Proccess Engine couldn't start!");
                @event = new WorkFlowStartFailedEvent("The process engine couldn't start!");
                _workFlowEventHandler.Handler(@event as WorkFlowStartFailedEvent);
                _trello.DeleteCard(cardId);
                return @event;
            }
            @event = new WorkFlowStartCompletedEvent(proccess, "Creditações");
            _workFlowEventHandler.Handler(@event as WorkFlowStartCompletedEvent);
            return @event;
        }
    }
}
