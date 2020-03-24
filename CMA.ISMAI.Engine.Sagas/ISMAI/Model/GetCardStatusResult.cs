using System;

namespace CMA.ISMAI.Sagas.Engine.ISMAI.Model
{
    internal class GetCardStatusResult
    {
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string MessageType { get; set; }
        public Guid AggregateId { get; set; }
    }
}
