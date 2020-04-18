using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Trello.Domain.Commands
{
    public class AddCardCommandAndProcess : AddCardCommand
    {
        public AddCardCommandAndProcess(string name, DateTime dueTime, string description, int boardId, List<string> filesUrl, string instituteName, string courseName, string studentName, bool isCetOrOtherCondition) : base(name, dueTime, description, boardId, filesUrl, instituteName, courseName, studentName, isCetOrOtherCondition)
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
            IsCetOrOtherCondition = isCetOrOtherCondition;
        }
    }
}
