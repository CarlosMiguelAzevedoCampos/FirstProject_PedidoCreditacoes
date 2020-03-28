using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Engine.API.Mapper;
using CMA.ISMAI.Engine.API.Model;
using CMA.ISMAI.Engine.Domain.Events;
using CMA.ISMAI.Engine.Domain.Interface;
using CMA.ISMAI.Logging.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Engine.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EngineController : BaseController
    {
        private readonly ILog _logger;
        private readonly IWorkflowCommandHandler _workflowCommandHandler;

        public EngineController(ILog logger, IWorkflowCommandHandler workflowCommandHandler)
        {
            _logger = logger;
            _workflowCommandHandler = workflowCommandHandler;
        }

        [HttpPost]
        public IActionResult StartWorkFlow([FromBody] DeployDto model)
        {
            model = new DeployDto("ISMAI",new Dictionary<string, object>()
            {
                {"cet" ,true },
                {"cardId","5e7f9cbb414bc74aabdc7a00" },
                {"courseName","Informática" },
                {"studentName","Carlos Campos" },
                {"courseInstitute","ISEP" }
            });
            if (model == null)
            {
                _logger.Fatal("An null DeployDto has recived!");
                return BadRequest();
            }
             Event @event = _workflowCommandHandler.Handle(Map.ConvertToCommand(model));

            if (@event is WorkFlowStartCompletedEvent)
                return Response(true, @event as WorkFlowStartCompletedEvent);

            return Response(false, @event as WorkFlowStartFailedEvent);
        }
    }
}
