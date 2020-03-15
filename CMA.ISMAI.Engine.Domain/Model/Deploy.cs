using System.Reflection;

namespace CMA.ISMAI.Engine.Domain.Model
{
    public class Deploy
    {
        public Deploy(string workFlowName, string processName)
        {
            WorkFlowName = workFlowName;
            ProcessName = processName;
        }

        public string WorkFlowName { get; set; }
        public string ProcessName { get; set; }

        public bool IsValid(Deploy deploy)
        {
            if (string.IsNullOrEmpty(deploy.ProcessName) || string.IsNullOrEmpty(deploy.WorkFlowName))
                return false;
            return true;
        }
    }
}
