using CMA.ISMAI.Trello.Domain.Validations;
using System;

namespace CMA.ISMAI.Trello.Domain.Commands
{
    public class AddCardCommand : CardCommand
    {
        public AddCardCommand(string name, DateTime dueTime, string description, int boardId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            DueTime = dueTime;
            BoardId = boardId;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddCardCommandValidations().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
