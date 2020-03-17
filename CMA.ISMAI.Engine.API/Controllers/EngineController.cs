using CMA.ISMAI.Core.Bus;
using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Engine.API.Mapper;
using CMA.ISMAI.Engine.API.Model;
using CMA.ISMAI.Logging.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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
        public IActionResult DeployWorkFlow([FromBody]DeployDto model)
        {
            if(model == null)
            {
                return BadRequest();
            }
            model.AssemblyName = Assembly.GetExecutingAssembly();
            
            Bus.SendCommand(Map.ConvertToCommand(model));
            return Response();
        }
    }
}
