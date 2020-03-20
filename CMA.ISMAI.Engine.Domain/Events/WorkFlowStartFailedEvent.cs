using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Core.Notifications;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Engine.Domain.Events
{
    public class WorkFlowStartFailedEvent : Event
    {
        public WorkFlowStartFailedEvent(List<DomainNotification> domainNotifications)
        {
            DomainNotifications = domainNotifications;
            AggregateId = Guid.NewGuid();
        }

        public List<DomainNotification> DomainNotifications { get; set; }
    }
}
