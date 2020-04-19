using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Sagas.Service.Model
{
    public class ReturnCardAttachmentsEvent
    {
        public string Id { get; set; }
        public List<string> Attachments { get; set; }
        public DateTime Timestamp { get; set; }
        public string MessageType { get; set; }
        public Guid AggregateId { get; set; }
    }
}
