using System.Reflection;

namespace CMA.ISMAI.Engine.API.Model
{
    public class DeployDto
    {
        public DeployDto(string workFlowName, string processName)
        {
            WorkFlowName = workFlowName;
            ProcessName = processName;
        }

        protected DeployDto()
        {
        }

        public string WorkFlowName { get; set; }
        public string ProcessName { get; set; }
    }
}
