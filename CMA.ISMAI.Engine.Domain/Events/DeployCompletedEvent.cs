using CMA.ISMAI.Core.Events;
using System;

namespace CMA.ISMAI.Engine.Domain.Events
{
    public class DeployCompletedEvent : Event
    {
        public DeployCompletedEvent(Guid id, string workFlowName, string processName, bool isCet)
        {
            Id = id;
            WorkFlowName = workFlowName;
            ProcessName = processName;
            IsCet = isCet;
            AggregateId = id;
        }

        public Guid Id { get; protected set; }
        public string WorkFlowName { get; set; }
        public string ProcessName { get; set; }
        public bool IsCet { get; set; }
    }
}
