using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;

namespace CMA.ISMAI.Trello.Domain.EventHandlers
{
    public class CardEventHandler : ICardEventHandler
    {
        public void Handler(AddCardCompletedEvent request)
        {
        }

        public void Handler(AddCardFailedEvent request)
        {
        }

        public void Handler(CardStatusCompletedEvent request)
        {
        }

        public void Handler(CardStatusIncompletedEvent request)
        {
        }

        public void Handler(CardStatusUnableToFindEvent request)
        {
        }
    }
}
