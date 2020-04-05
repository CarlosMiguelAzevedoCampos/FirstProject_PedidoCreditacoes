using CMA.ISMAI.Core.Events;
using System;

namespace CMA.ISMAI.IntegrationTests.Trello.Model
{
    public class CardStatusCompletedEvent : Event
    {
        public CardStatusCompletedEvent(string id)
        {
            this.Id = id;
            AggregateId = Guid.NewGuid();
        }

        public string Id { get; set; }
    }
}
