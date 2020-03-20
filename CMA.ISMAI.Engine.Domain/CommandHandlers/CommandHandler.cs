using CMA.ISMAI.Core.Commands;
using CMA.ISMAI.Core.Notifications;
using System.Collections.Generic;

namespace CMA.ISMAI.Engine.Domain.CommandHandlers
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
    }
}
