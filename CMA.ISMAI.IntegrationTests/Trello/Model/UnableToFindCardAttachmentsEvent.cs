using CMA.ISMAI.Core.Events;
using System;

namespace CMA.ISMAI.IntegrationTests.Trello.Model
{
    public class UnableToFindCardAttachmentsEvent : Event
    {
        public UnableToFindCardAttachmentsEvent(string cardId)
        {
            this.Id = cardId;
            AggregateId = Guid.NewGuid();
        }

        public string Id { get; set; }
    }
}
