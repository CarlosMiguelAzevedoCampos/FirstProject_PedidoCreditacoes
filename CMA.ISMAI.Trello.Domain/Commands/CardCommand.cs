using CMA.ISMAI.Core.Commands;
using System;

namespace CMA.ISMAI.Trello.Domain.Commands
{
    public abstract class CardCommand : Command
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DueTime { get; set; }
        public string Description { get; set; }
    }
}
