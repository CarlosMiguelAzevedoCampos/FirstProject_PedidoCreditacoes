using CMA.ISMAI.Engine.Domain.Validations;
using System.Collections.Generic;
using System.Reflection;

namespace CMA.ISMAI.Engine.Domain.Commands
{
    public class StartDeployCommand : DeployCommand
    {
        public StartDeployCommand(string workFlowName, string processName, Assembly assembly, Dictionary<string, object> parameters)
        {
            WorkFlowName = workFlowName;
            ProcessName = processName;
            Parameters = parameters;
            AssemblyName = assembly;
        }

        public override bool IsValid()
        {
            ValidationResult = new StartDeployCommandValidations().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
