using CMA.ISMAI.Core.Events;
using System;

namespace CMA.ISMAI.Trello.Domain.Events
{
    public class WorkFlowStartFailedEvent : Event
    {
        public WorkFlowStartFailedEvent(string motive)
        {
            Motive = motive;
            AggregateId = Guid.NewGuid();
        }

        public string Motive { get; set; }
    }
}
