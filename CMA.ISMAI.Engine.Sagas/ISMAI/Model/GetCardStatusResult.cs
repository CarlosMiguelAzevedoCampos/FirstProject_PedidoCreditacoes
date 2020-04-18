using System;

namespace CMA.ISMAI.Sagas.Service.Model
{
    internal class GetCardStatusResult
    {
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string MessageType { get; set; }
        public Guid AggregateId { get; set; }
    }
}
