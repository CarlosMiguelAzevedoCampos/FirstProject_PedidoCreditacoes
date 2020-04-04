using CMA.ISMAI.Core.Events;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Trello.Domain.Events
{
    public class ReturnCardAttachmentsEvent : Event
    {
        public ReturnCardAttachmentsEvent(string cardId, List<string> attachments)
        {
            this.Id = cardId;
            AggregateId = Guid.NewGuid();
            Attachments = attachments;
        }

        public string Id { get; set; }
        public List<string> Attachments { get; set; }
    }
}
