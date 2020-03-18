using CMA.ISMAI.Core.Bus;
using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Engine.API.Mapper;
using CMA.ISMAI.Engine.API.Model;
using CMA.ISMAI.Logging.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CMA.ISMAI.Engine.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EngineController : BaseController
    {
        private readonly ILog _logger;
        private readonly IMediatorHandler Bus;

        public EngineController(ILog logger, IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications) : base(notifications, bus)
        {
            _logger = logger;
            Bus = bus;
        }

        [HttpPost]
        public IActionResult StartWorkFlow([FromBody] DeployDto model)
        {
            if (model == null)
            {
                _logger.Fatal("An null DeployDto has recived!");
                return BadRequest();
            }            
            Bus.SendCommand(Map.ConvertToCommand(model));
            return Response();
        }
    }
}
