using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.API.Mapper;
using CMA.ISMAI.Trello.API.Model;
using CMA.ISMAI.Trello.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CMA.ISMAI.Trello.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TrelloController : BaseController
    {
        private readonly ILog _logger;
        private readonly ITrelloService _engineService;

        public TrelloController(ILog logger, ITrelloService engineService)
        {
            _logger = logger;
            _engineService = engineService;
        }

        [HttpPost]
        public IActionResult AddCard([FromBody]CardDto card)
        {
            if(card == null)
                return BadResultAction("A problem happend while deleting the process! Model is null!");
            string result = _engineService.AddCard(Map.ConverToModel(card)).Result;
            return result != string.Empty ? OkAction(result) : BadResultAction("A problem happend while deleting the process!");
        }

        [HttpGet]
        public IActionResult GetCardStatus(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.Fatal("Card ID is null!");
                return BadResultAction("A null or empty card id was passed!");
            }
            bool result = _engineService.IsTheProcessFinished(id).Result;
            return OkAction(result.ToString());
        }
    }
}