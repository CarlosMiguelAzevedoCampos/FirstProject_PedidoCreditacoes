using CMA.ISMAI.Engine.Domain.Commands;
using FluentValidation;

namespace CMA.ISMAI.Engine.Domain.Validations
{
    public abstract class WorkFlowValidations<T> : AbstractValidator<T> where T : WorkFlowCommand
    {
        protected void ValidateParametersDictionary()
        {
            RuleFor(c => c.Parameters)
                .NotNull().WithMessage("Please ensure you have entered the Parameters");
        }

        protected void ValidateAssembly()
        {
            RuleFor(c => c.AssemblyName)
               .NotNull().WithMessage("Assembly is null!, Please, contact system admin!");
        }
    }
}
