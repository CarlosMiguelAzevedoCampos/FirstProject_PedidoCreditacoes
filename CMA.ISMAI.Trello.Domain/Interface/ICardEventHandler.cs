using CMA.ISMAI.Trello.Domain.Events;
using System.Threading.Tasks;

namespace CMA.ISMAI.Trello.Domain.Interface
{
    public interface ICardEventHandler
    {
        void Handler(AddCardCompletedEvent request);
        void Handler(AddCardFailedEvent request);
        void Handler(CardStatusCompletedEvent request);
        void Handler(CardStatusIncompletedEvent request);
        void Handler(CardStatusUnableToFindEvent request);
        void Handler(CardHasNotBeenDeletedEvent request);
        void Handler(CardHasBeenDeletedEvent request);
        void Handler(CardDosentHaveAttchmentsEvent request);
        void Handler(ReturnCardAttachmentsEvent request);
        void Handler(UnableToFindCardAttachmentsEvent request);
    }
}
