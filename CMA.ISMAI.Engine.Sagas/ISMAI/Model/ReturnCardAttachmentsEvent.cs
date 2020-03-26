using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Sagas.Engine.ISMAI.Model
{
    public class ReturnCardAttachmentsEvent
    {
        public Guid Id { get; set; }
        public List<string> Attachments { get; set; }
        public DateTime Timestamp { get; set; }
        public string MessageType { get; set; }
        public Guid AggregateId { get; set; }
    }
}
