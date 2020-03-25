using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Trello.Domain.Commands;
using System.Collections.Generic;

namespace CMA.ISMAI.Trello.Domain.Interface
{
    public interface ICardCommandHandler
    {
        Event Handler(AddCardCommand request);
        Event Handler(GetCardStatusCommand request);
        Event Handler(GetCardAttachmentsCommand request);
    }
}
