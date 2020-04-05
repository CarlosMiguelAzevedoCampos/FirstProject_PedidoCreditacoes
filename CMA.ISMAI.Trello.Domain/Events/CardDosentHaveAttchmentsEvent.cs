using CMA.ISMAI.Core.Events;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Trello.Domain.Events
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
