using CMA.ISMAI.Trello.Domain.Events;

namespace CMA.ISMAI.Trello.Domain.Interface
{
    public interface ICardEventHandler
    {
        void Handler(AddCardCompletedEvent request);
        void Handler(AddCardFailedEvent request);
        void Handler(CardStatusCompletedEvent request);
        void Handler(CardStatusIncompletedEvent request);
        void Handler(CardStatusUnableToFindEvent request);
    }
}
