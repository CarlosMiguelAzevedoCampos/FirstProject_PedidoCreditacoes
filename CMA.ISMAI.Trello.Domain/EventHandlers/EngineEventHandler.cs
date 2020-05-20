using CMA.ISMAI.Core.Events.Store.Interface;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;
using System.Threading.Tasks;

namespace CMA.ISMAI.Trello.Domain.EventHandlers
{
    public class EngineEventHandler : IEngineEventHandler
    {
        private readonly IEventStore _eventStore;

        public EngineEventHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }
        public Task Handler(WorkFlowStartFailedEvent notification)
        {
            _eventStore.SaveToEventStore(notification);
            return Task.CompletedTask;
        }
        public Task Handler(WorkFlowStartCompletedEvent notification)
        {
            _eventStore.SaveToEventStore(notification);
            return Task.CompletedTask;
        }
    }
}
