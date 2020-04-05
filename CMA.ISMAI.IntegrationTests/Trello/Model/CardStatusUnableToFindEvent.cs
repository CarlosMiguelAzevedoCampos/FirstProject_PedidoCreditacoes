using CMA.ISMAI.Core.Events;
using System;

namespace CMA.ISMAI.IntegrationTests.Trello.Model
{
    public class CardStatusUnableToFindEvent : Event
    {
        public CardStatusUnableToFindEvent(string id)
        {
            this.Id = id;
            AggregateId = Guid.NewGuid();
        }

        public string Id { get; set; }
    }
}
