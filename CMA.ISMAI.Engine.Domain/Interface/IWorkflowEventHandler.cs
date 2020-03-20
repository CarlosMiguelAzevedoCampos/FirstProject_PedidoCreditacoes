using CMA.ISMAI.Engine.Domain.Events;

namespace CMA.ISMAI.Engine.Domain.Interface
{
    public interface IWorkflowEventHandler
    {
        void Handle(WorkFlowStartCompletedEvent notification);
        void Handle(WorkFlowStartFailedEvent notification);
    }
}
