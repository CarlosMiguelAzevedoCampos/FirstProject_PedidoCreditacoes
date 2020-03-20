using CMA.ISMAI.Engine.Domain.Events;
using CMA.ISMAI.Engine.Domain.Interface;

namespace CMA.ISMAI.Engine.Domain.EventHandlers
{
    public class WorkFlowEventHandler : IWorkflowEventHandler
    {
        public void Handle(WorkFlowStartCompletedEvent notification)
        {
        }

        public void Handle(WorkFlowStartFailedEvent notification)
        {
        }
    }
}
