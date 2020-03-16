using CMA.ISMAI.Engine.API.Model;
using CMA.ISMAI.Engine.Domain.Model;

namespace CMA.ISMAI.Engine.API.Mapper
{
    public static class Map
    {
        public static Deploy ConvertoToModel(DeployDto model)
        {
            return new Deploy(model.WorkFlowName, model.ProcessName, model.IsCet);
        }
    }
}
