using CMA.ISMAI.Engine.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CMA.ISMAI.Engine.Domain.Commands
{
    public class StartWorkFlowCommand : WorkFlowCommand
    {
        public StartWorkFlowCommand(string workFlowName, string processName, Assembly assembly, Dictionary<string, object> parameters)
        {
            Id = Guid.NewGuid();
            WorkFlowName = workFlowName;
            ProcessName = processName;
            Parameters = parameters;
            AssemblyName = assembly;
        }

        public override bool IsValid()
        {
            ValidationResult = new StartWorkFlowCommandValidations().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
