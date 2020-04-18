using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Trello.Domain.Commands;

namespace CMA.ISMAI.Trello.Domain.Interface
{
    public interface ICardCommandHandler
    {
        Event Handler(AddCardCommand request);
        Event Handler(AddCardCommandAndProcess request);
        Event Handler(GetCardStatusCommand request);
        Event Handler(GetCardAttachmentsCommand request);
        Event Handler(DeleteCardCommand request);
    }
}
