using CMA.ISMAI.Engine.API.Model;
using CMA.ISMAI.Engine.Domain.Commands;

namespace CMA.ISMAI.Engine.API.Mapper
{
    public static class Map
    {
        public static StartDeployCommand ConvertToCommand(DeployDto model)
        {
            return new StartDeployCommand(model.WorkFlowName, model.ProcessName, model.AssemblyName, model.Parameters);
        }
    }
}
