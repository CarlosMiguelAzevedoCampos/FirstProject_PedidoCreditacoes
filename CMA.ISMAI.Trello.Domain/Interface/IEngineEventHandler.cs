using CMA.ISMAI.Trello.Domain.Events;

namespace CMA.ISMAI.Trello.Domain.Interface
{
    public interface IEngineEventHandler
    {
        void Handler(WorkFlowStartFailedEvent request);
        void Handler(WorkFlowStartCompletedEvent request);
    }
}
