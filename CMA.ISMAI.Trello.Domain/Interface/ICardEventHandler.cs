using CMA.ISMAI.Trello.Domain.Events;
using System.Threading.Tasks;

namespace CMA.ISMAI.Trello.Domain.Interface
{
    public interface ICardEventHandler
    {
        Task Handler(AddCardCompletedEvent request);
        Task Handler(AddCardFailedEvent request);
        Task Handler(CardStatusCompletedEvent request);
        Task Handler(CardStatusIncompletedEvent request);
        Task Handler(CardStatusUnableToFindEvent request);
        Task Handler(CardHasNotBeenDeletedEvent request);
        Task Handler(CardHasBeenDeletedEvent request);
        Task Handler(CardDosentHaveAttchmentsEvent request);
        Task Handler(ReturnCardAttachmentsEvent request);
        Task Handler(UnableToFindCardAttachmentsEvent request);
    }
}
