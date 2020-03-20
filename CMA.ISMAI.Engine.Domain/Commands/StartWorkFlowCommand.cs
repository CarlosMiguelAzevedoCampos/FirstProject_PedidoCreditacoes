using CMA.ISMAI.Engine.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CMA.ISMAI.Engine.Domain.Commands
{
    public class StartWorkFlowCommand : WorkFlowCommand
    {
        public StartWorkFlowCommand(string workFlowName, Assembly assembly, Dictionary<string, object> parameters)
        {
            WorkFlowName = workFlowName;
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
