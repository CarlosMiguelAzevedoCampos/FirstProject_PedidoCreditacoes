using CMA.ISMAI.Engine.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CMA.ISMAI.Engine.Domain.EventHandlers
{
    public class WorkFlowEventHandler : INotificationHandler<WorkFlowStartCompletedEvent>
    {
        public Task Handle(WorkFlowStartCompletedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
