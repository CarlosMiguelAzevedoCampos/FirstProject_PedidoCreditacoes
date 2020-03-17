using CMA.ISMAI.Core.Commands;
using CMA.ISMAI.Core.Events;
using System.Threading.Tasks;

namespace CMA.ISMAI.Core.Bus
{
    public interface IMediatorHandler
    {
        Task SendCommand<T>(T command) where T : Command;
        Task RaiseEvent<T>(T @event) where T : Event;
    }
}
