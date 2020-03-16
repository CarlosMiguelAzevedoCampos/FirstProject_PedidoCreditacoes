using CMA.ISMAI.Automation.DomainInterface;
using CMA.ISMAI.Engine.API.Mapper;
using CMA.ISMAI.Engine.API.Model;
using CMA.ISMAI.Logging.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace CMA.ISMAI.Engine.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EngineController : BaseController
    {
        private readonly ILog _logger;
        private readonly IEngineService _engineService;

        public EngineController(ILog logger, IEngineService engineService)
        {
            _logger = logger;
            _engineService = engineService;
        }

        [HttpDelete]
        public IActionResult DeleteDeployment(string id)
        {
            bool result = _engineService.DeleteDeployment(id);

            _logger.Info($"An Deleting order for process {id} has made!. Was deleted? {result.ToString()}.");
            if (result)
                return OkAction();
            return BadResultAction("A problem happend while deleting the process!");
        }

        [HttpPost]
        public IActionResult StartWorkFlow([FromBody]DeployDto model)
        {
            string result = _engineService.StartWorkFlow(Map.ConvertoToModel(model), Assembly.GetExecutingAssembly());
            bool isResultEmptyOrNull = !string.IsNullOrEmpty(result);


            if (isResultEmptyOrNull)
                return OkAction();

            return BadResultAction("Something went wrong in the deployment process!");
        }
    }
}
