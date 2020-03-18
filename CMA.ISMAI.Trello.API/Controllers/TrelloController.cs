using CMA.ISMAI.Core.Bus;
using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.API.Mapper;
using CMA.ISMAI.Trello.API.Model;
using CMA.ISMAI.Trello.Domain.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CMA.ISMAI.Trello.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TrelloController : BaseController
    {
        private readonly ILog _logger;
        private readonly IMediatorHandler Bus;

        public TrelloController(ILog logger, IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications) : base(notifications, bus)
        {
            _logger = logger;
            Bus = bus;
        }

        [HttpPost]
        public IActionResult AddCard([FromBody]CardDto card)
        {
            if (card == null)
            {
                _logger.Fatal("Card Dto is null!");
                return BadRequest();
            }
            Bus.SendCommand(Map.ConverToModel(card));
            return Response();
        }

        [HttpGet]
        public IActionResult GetCardStatus(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.Fatal("Card ID is null!");
                return BadRequest();
            }
            string result = Bus.SendCommand(new ObtainCardStatusCommand(id)).ToString();
            return Response();
        }
    }
}