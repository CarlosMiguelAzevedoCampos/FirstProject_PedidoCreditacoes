using CMA.ISMAI.Engine.API.Model;
using CMA.ISMAI.Engine.Domain.Commands;
using System.Reflection;

namespace CMA.ISMAI.Engine.API.Mapper
{
    public static class Map
    {
        public static StartWorkFlowCommand ConvertToCommand(DeployDto model)
        {
            return new StartWorkFlowCommand(model.WorkFlowName, model.ProcessName, Assembly.GetExecutingAssembly(), model.Parameters);
        }
    }
}
