using System.Collections.Generic;
using System.Reflection;

namespace CMA.ISMAI.Engine.API.Model
{
    public class DeployDto
    {
        public DeployDto(string workFlowName, Dictionary<string, object> parameters)
        {
            WorkFlowName = workFlowName;
            Parameters = parameters;
        }

        protected DeployDto()
        {
        }

        public string WorkFlowName { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}
