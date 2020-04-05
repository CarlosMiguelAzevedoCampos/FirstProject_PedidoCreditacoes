using CMA.ISMAI.Core.Events.Store.Interface;
using CMA.ISMAI.Core.Notifications;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;
using System.Threading.Tasks;

namespace CMA.ISMAI.Trello.Domain.EventHandlers
{
    public class EngineEventHandler : NotificationsHandler, IEngineEventHandler
    {
        public EngineEventHandler(IEventStore eventStore, ILog log) : base(eventStore, log)
        {
        }
        public Task Handler(WorkFlowStartFailedEvent notification)
        {
            Task.Run(()=> SaveEvent(notification));
            Task.Run(() => SendNotification(new MessageBody("trelloismai@gmail.com", $"An new deploy failed!!, {notification.Motive} - {notification.Timestamp} - {notification.MessageType} - {notification.AggregateId}")));
            return Task.CompletedTask;
        }
        public Task Handler(WorkFlowStartCompletedEvent notification)
        {
            Task.Run(() => SaveEvent(notification));
            Task.Run(() => SendNotification(new MessageBody("trelloismai@gmail.com", $"An new deploy was submited!, {notification.Id} - {notification.Timestamp} - {notification.MessageType} - {notification.AggregateId} -  {notification.WorkFlowName}")));
            return Task.CompletedTask;
        }
    }
}
