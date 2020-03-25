using CMA.ISMAI.Core.Events;
using System;

namespace CMA.ISMAI.Trello.Domain.Events
{
    public class CardDosentHaveAttachmentsEvent : Event
    {
        public CardDosentHaveAttachmentsEvent()
        {
            this.Id = Guid.NewGuid();
            AggregateId = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}
