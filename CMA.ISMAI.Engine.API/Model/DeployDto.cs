using System.Collections.Generic;
using System.Reflection;

namespace CMA.ISMAI.Engine.API.Model
{
    public class DeployDto
    {
        public DeployDto(string workFlowName, string processName, Dictionary<string, object> parameters)
        {
            WorkFlowName = workFlowName;
            ProcessName = processName;
            Parameters = parameters;
        }

        protected DeployDto()
        {
        }

        public string WorkFlowName { get; set; }
        public string ProcessName { get; set; }
        public Assembly AssemblyName { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}
