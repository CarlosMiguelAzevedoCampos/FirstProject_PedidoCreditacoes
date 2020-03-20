using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Engine.Domain.Commands;

namespace CMA.ISMAI.Engine.Domain.Interface
{
    public interface IWorkflowCommandHandler
    {
        Event Handle(StartWorkFlowCommand request);
    }
}
