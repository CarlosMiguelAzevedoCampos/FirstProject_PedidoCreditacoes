using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Core.Notifications;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Trello.Domain.Events
{
    public class AddCardFailedEvent : Event
    {
        public AddCardFailedEvent(List<DomainNotification> domainNotifications)
        {
            DomainNotifications = domainNotifications;
            AggregateId = Guid.NewGuid();
        }

        public List<DomainNotification> DomainNotifications { get; set; }
    }
}
