using CMA.ISMAI.Trello.Domain.Validations;
using System;

namespace CMA.ISMAI.Trello.Domain.Commands
{
    public class ObtainCardStatusCommand : CardStatusCommand
    {
        public ObtainCardStatusCommand(string id)
        {
            Id = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new ObtainCardStatusCommandValidations().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
