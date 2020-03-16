using System.Reflection;

namespace CMA.ISMAI.Engine.API.Model
{
    public class DeployDto
    {
        public DeployDto(string workFlowName, string processName, bool isCet)
        {
            WorkFlowName = workFlowName;
            ProcessName = processName;
            IsCet = isCet;
        }

        protected DeployDto()
        {
        }

        public string WorkFlowName { get; set; }
        public string ProcessName { get; set; }

        public bool IsCet { get; set; }
    }
}
