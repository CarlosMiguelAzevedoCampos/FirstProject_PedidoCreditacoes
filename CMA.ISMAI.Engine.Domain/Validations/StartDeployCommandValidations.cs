using CMA.ISMAI.Engine.Domain.Commands;

namespace CMA.ISMAI.Engine.Domain.Validations
{
    public class StartDeployCommandValidations : DeployValidations<StartDeployCommand>
    {
        public StartDeployCommandValidations()
        {
            ValidateProcessName();
            ValidateWorkFlowName();
            ValidateParametersDictionary();
            ValidateAssembly();
        }
    }
}
