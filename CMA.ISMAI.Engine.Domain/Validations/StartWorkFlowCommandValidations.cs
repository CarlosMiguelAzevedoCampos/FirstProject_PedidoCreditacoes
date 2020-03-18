using CMA.ISMAI.Engine.Domain.Commands;

namespace CMA.ISMAI.Engine.Domain.Validations
{
    public class StartWorkFlowCommandValidations : WorkFlowValidations<StartWorkFlowCommand>
    {
        public StartWorkFlowCommandValidations()
        {
            ValidateProcessName();
            ValidateWorkFlowName();
            ValidateParametersDictionary();
            ValidateAssembly();
        }
    }
}
