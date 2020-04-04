using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Core.Events.Store.Interface;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;

namespace CMA.ISMAI.Core.Notifications
{
    public abstract class NotificationsHandler
    {
        private readonly IEventStore _eventStore;

        protected NotificationsHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void SaveEvent(Event @event)
        {
            Task.Run(() => _eventStore.SaveToEventStore(@event));
        }

        public virtual void SendNotification(MessageBody notifications)
        {
            Task.Run(() => SendNotificationToBroker(notifications));
        }
        
        private void SendNotificationToBroker(MessageBody notifications)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "admin",
                Password = "admin"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "NotificationsQueue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(notifications));

                channel.BasicPublish(exchange: "",
                                     routingKey: "NotificationsQueue",
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}