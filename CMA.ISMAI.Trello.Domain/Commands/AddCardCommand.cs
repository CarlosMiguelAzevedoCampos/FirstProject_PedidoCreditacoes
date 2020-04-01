using CMA.ISMAI.Trello.Domain.Validations;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Trello.Domain.Commands
{
    public class AddCardCommand : CardCommand
    {
        public AddCardCommand(string name, DateTime dueTime, string description, int boardId, List<string> filesUrl, string instituteName, string courseName, string studentName, bool isCet)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            DueTime = dueTime;
            BoardId = boardId;
            FilesUrl = filesUrl;
            CourseName = courseName;
            InstituteName = instituteName;
            StudentName = studentName;
            IsCet = isCet;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddCardCommandValidations().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
