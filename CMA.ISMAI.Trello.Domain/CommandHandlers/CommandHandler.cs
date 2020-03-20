using CMA.ISMAI.Core.Commands;
using CMA.ISMAI.Core.Notifications;
using System.Collections.Generic;

namespace CMA.ISMAI.Trello.Domain.CommandHandlers
{
    public class CommandHandler
    {
        protected List<DomainNotification> NotifyValidationErrors(Command message)
        {
            List<DomainNotification> domainNotification = new List<DomainNotification>();
            foreach (var error in message.ValidationResult.Errors)
            {
                domainNotification.Add(new DomainNotification(message.MessageType, error.ErrorMessage));
            }
            return domainNotification;
        }

        protected List<DomainNotification> NotifyDomainErros(string key, string value)
        {
            List<DomainNotification> domainNotification = new List<DomainNotification>();
            domainNotification.Add(new DomainNotification(key, value));
            return domainNotification;
        }
    }
}
