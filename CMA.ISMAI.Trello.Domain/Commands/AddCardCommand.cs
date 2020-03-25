using CMA.ISMAI.Trello.Domain.Validations;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Trello.Domain.Commands
{
    public class AddCardCommand : CardCommand
    {
        public AddCardCommand(string name, DateTime dueTime, string description, int boardId, List<string> filesUrl)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            DueTime = dueTime;
            BoardId = boardId;
            FilesUrl = filesUrl;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddCardCommandValidations().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
