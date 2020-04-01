using CMA.ISMAI.Core.Events;
using System;

namespace CMA.ISMAI.Trello.Domain.Events
{
    public class WorkFlowStartCompletedEvent : Event
    {
        public WorkFlowStartCompletedEvent(string id, string workFlowName)
        {
            Id = id;
            WorkFlowName = workFlowName;
            AggregateId = Guid.NewGuid();
        }

        public string Id { get; protected set; }
        public string WorkFlowName { get; set; }
    }
}
