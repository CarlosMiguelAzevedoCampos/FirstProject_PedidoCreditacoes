using CMA.ISMAI.Core.Events;
using System;

namespace CMA.ISMAI.Trello.Domain.Events
{
    public class CardStatusIncompletedEvent : Event
    {
        public CardStatusIncompletedEvent(string id)
        {
            this.Id = id;
            AggregateId = Guid.NewGuid();
        }

        public string Id { get; set; }
    }
}
