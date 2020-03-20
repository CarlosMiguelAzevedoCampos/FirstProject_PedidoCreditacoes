using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.API.Mapper;
using CMA.ISMAI.Trello.API.Model;
using CMA.ISMAI.Trello.Domain.Commands;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;
using CMA.ISMAI.Trello.Engine.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CMA.ISMAI.Trello.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TrelloController : BaseController
    {
        private readonly ILog _logger;
        private readonly ICardCommandHandler _cardHandler;

        public TrelloController(ILog logger, ICardCommandHandler cardHandler)
        {
            _logger = logger;
            _cardHandler = cardHandler;
        }

        [HttpPost]
        public IActionResult AddCard([FromBody]CardDto card)
        {
            if (card == null)
            {
                _logger.Fatal("Card Dto is null!");
                return BadRequest();
            }

            Event @event = _cardHandler.Handler(Map.ConverToModel(card));
            if (@event is AddCardCompletedEvent)
                return Response(true, @event as AddCardCompletedEvent);
            return Response(false, @event as AddCardFailedEvent);
        }

        [HttpGet]
        public IActionResult GetCardStatus(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.Fatal("Card ID is null!");
                return BadRequest();
            }
            Event @event = _cardHandler.Handler(new GetCardStatusCommand(id));

            if (@event is CardStatusCompletedEvent)
                return Response(true, @event as CardStatusCompletedEvent);
            else if (@event is CardStatusIncompletedEvent)
                return Response(true, @event as CardStatusIncompletedEvent);
            else
                return Response(false, @event as CardStatusUnableToFindEvent);
        }
    }
}