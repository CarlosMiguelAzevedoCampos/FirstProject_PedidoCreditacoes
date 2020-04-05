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
    }
}
