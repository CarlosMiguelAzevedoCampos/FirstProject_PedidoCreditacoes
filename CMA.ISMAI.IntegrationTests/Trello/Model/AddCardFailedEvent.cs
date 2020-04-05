using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Core.Notifications;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.IntegrationTests.Trello.Model
{
    internal class AddCardFailedEvent : Event
    {
        public AddCardFailedEvent(List<DomainNotification> domainNotifications,string id, string name, string description, DateTime dueTime)
        {
            DomainNotifications = domainNotifications;
            Id = id;
            AggregateId = Guid.NewGuid();
            Name = name;
            Description = description;
            DueTime = dueTime;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime DueTime { get; set; }
        public string Description { get; set; }
        public List<DomainNotification> DomainNotifications { get; set; }
    }
}
