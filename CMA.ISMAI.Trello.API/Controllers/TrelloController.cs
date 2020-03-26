using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.API.Mapper;
using CMA.ISMAI.Trello.API.Model;
using CMA.ISMAI.Trello.Domain.Commands;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
            card = new CardDto("ISMAI - Informática - Carlos Campos", DateTime.Now.AddDays(2), "ISMAI - Informática - Carlos Campos", 0, new List<string>() { "https://stackoverflow.com/questions/30201170/trello-manatee-adding-attachment",
            "https://stackoverflow.com/questions/30201170/trello-manatee-adding-attachment"
            ,"https://stackoverflow.com/questions/30201170/trello-manatee-adding-attachment"});

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
        [Route("GetCardStatus")]
        public IActionResult GetCardStatus(string cardId)
        {
            if (string.IsNullOrEmpty(cardId))
            {
                _logger.Fatal("Card ID is null!");
                return BadRequest();
            }
            Event @event = _cardHandler.Handler(new GetCardStatusCommand(cardId));

            if (@event is CardStatusCompletedEvent)
                return Response(true, @event as CardStatusCompletedEvent);
            else if (@event is CardStatusIncompletedEvent)
                return Response(true, @event as CardStatusIncompletedEvent);
            else
                return Response(false, @event as CardStatusUnableToFindEvent);
        }

        [HttpGet]
        [Route("GetCardAttachments")]
        public IActionResult GetCardAttachments(string cardId, int boardId)
        {
            if (string.IsNullOrEmpty(cardId) || boardId < 0)
            {
                _logger.Fatal("Card ID is null or boardId is lower than 0!");
                return BadRequest();
            }
            Event @event = _cardHandler.Handler(new GetCardAttachmentsCommand(cardId, boardId));
            return Response(true, @event as ReturnCardAttachmentsEvent);
        }
    }
}