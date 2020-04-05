using CMA.ISMAI.Core.Commands;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Trello.Domain.Commands
{
    public abstract class CardCommand : Command
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DueTime { get; set; }
        public string Description { get; set; }
        public List<string> FilesUrl { get; set; }
        public int BoardId { get; set; }
        public string StudentName { get; set; }
        public string InstituteName { get; set; }
        public string CourseName { get; set; }
        public bool IsCet { get; set; }
    }
}
