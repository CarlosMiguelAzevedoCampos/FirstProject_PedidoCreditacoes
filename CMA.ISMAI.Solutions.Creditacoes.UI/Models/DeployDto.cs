using System.Collections.Generic;

namespace CMA.ISMAI.Solutions.Creditacoes.UI.Models
{
    public class DeployDto
    {
        public DeployDto(string workFlowName, Dictionary<string, object> parameters)
        {
            WorkFlowName = workFlowName;
            Parameters = parameters;
        }

        public string WorkFlowName { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}
