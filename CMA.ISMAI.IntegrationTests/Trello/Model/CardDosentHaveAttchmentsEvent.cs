using CMA.ISMAI.Core.Events;
using System;

namespace CMA.ISMAI.IntegrationTests.Trello.Model
{
    public class CardDosentHaveAttchmentsEvent : Event
    {
        public CardDosentHaveAttchmentsEvent(string cardId)
        {
            this.Id = cardId;
            AggregateId = Guid.NewGuid();
        }

        public string Id { get; set; }
    }
}
