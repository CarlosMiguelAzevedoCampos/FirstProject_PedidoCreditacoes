using CMA.ISMAI.Core.Events;
using System;

namespace CMA.ISMAI.Trello.Domain.Events
{
    public class CardCompletedStatusEvent : Event
    {
        public CardCompletedStatusEvent(string id)
        {
            Id = id;
            AggregateId = Guid.NewGuid();
        }

        public string Id { get; set; }
    }
}
