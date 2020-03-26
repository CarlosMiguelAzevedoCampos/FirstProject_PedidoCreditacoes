using CMA.ISMAI.Core.Events;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Trello.Domain.Events
{
    public class ReturnCardAttachmentsEvent : Event
    {
        public ReturnCardAttachmentsEvent(List<string> attachments)
        {
            this.Id = Guid.NewGuid();
            AggregateId = Guid.NewGuid();
            Attachments = attachments;
        }

        public Guid Id { get; set; }
        public List<string> Attachments { get; set; }
    }
}
