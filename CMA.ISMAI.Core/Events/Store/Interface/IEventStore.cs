using CMA.ISMAI.Core.Events;

namespace CMA.ISMAI.Core.Events.Store.Interface
{
    public interface IEventStore
    {
        void SaveToEventStore(Event @event);
    }
}
