using CMA.ISMAI.Core.Events;
using System;

namespace CMA.ISMAI.Trello.Domain.Events
{
    public class AddCardCompletedEvent : Event
    {
        public AddCardCompletedEvent(Guid id, string name, string description, DateTime dueTime)
        {
            Id = id;
            Name = name;
            Description = description;
            DueTime = dueTime;
            AggregateId = id;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DueTime { get; set; }
        public string Description { get; set; }
    }
}
