using CMA.ISMAI.Core.Events;
using System;

namespace CMA.ISMAI.Trello.Domain.Events
{
    public class CardIncompletedStatusEvent : Event
    {
        public CardIncompletedStatusEvent(string id)
        {
            Id = id;
            AggregateId = Guid.NewGuid();
        }

        public string Id { get; set; }
    }
}
