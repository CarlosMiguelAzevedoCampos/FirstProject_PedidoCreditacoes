using CMA.ISMAI.Core.Events.Store.Interface;
using CMA.ISMAI.Trello.Domain.Events;
using CMA.ISMAI.Trello.Domain.Interface;
using CMA.ISMAI.Trello.MessageBroker.Interface;
using System.Threading.Tasks;

namespace CMA.ISMAI.Trello.Domain.EventHandlers
{
    public class EngineEventHandler :  IEngineEventHandler
    {
        private readonly IEventStore _eventStore;
        private readonly ISendNotificationService _sendNotificationService;

        public EngineEventHandler(IEventStore eventStore, ISendNotificationService sendNotificationService)
        {
            _eventStore = eventStore;
            _sendNotificationService = sendNotificationService;
        }
        public void Handler(WorkFlowStartFailedEvent notification)
        {
            Task.Run(()=> _eventStore.SaveToEventStore(notification));
            Task.Run(() => _sendNotificationService.SendNotificationToBroker("trelloismai@gmail.com", $"An new deploy failed!!, {notification.Motive} - {notification.Timestamp} - {notification.MessageType} - {notification.AggregateId}"));
        }
        public void Handler(WorkFlowStartCompletedEvent notification)
        {
            Task.Run(() => _eventStore.SaveToEventStore(notification));
            Task.Run(() => _sendNotificationService.SendNotificationToBroker("trelloismai@gmail.com", $"An new deploy was submited!, {notification.Id} - {notification.Timestamp} - {notification.MessageType} - {notification.AggregateId} -  {notification.WorkFlowName}"));
        }
    }
}
