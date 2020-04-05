using CMA.ISMAI.Trello.Domain.Events;
using System.Threading.Tasks;

namespace CMA.ISMAI.Trello.Domain.Interface
{
    public interface IEngineEventHandler
    {
        Task Handler(WorkFlowStartFailedEvent request);
        Task Handler(WorkFlowStartCompletedEvent request);
    }
}
