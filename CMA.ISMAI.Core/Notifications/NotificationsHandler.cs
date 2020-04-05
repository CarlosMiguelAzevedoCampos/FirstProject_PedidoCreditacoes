using CMA.ISMAI.Core.Events;
using CMA.ISMAI.Core.Events.Store.Interface;
using CMA.ISMAI.Logging.Interface;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CMA.ISMAI.Core.Notifications
{
    public abstract class NotificationsHandler : BaseConfiguration
    {
        private readonly IEventStore _eventStore;
        private readonly ILog _log;

        protected NotificationsHandler(IEventStore eventStore, ILog log)
        {
            _eventStore = eventStore;
            _log = log;
        }

        protected void SaveEvent(Event @event)
        {
            _eventStore.SaveToEventStore(@event);
        }

        protected virtual void SendNotification(MessageBody notifications)
        {
            SendNotificationToBroker(notifications);
        }

        private void SendNotificationToBroker(MessageBody notifications)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = ReturnSettingsValue("RabbitMqCore", "Uri"),
                    Port = Convert.ToInt32(ReturnSettingsValue("RabbitMqCore", "Port")),
                    UserName = ReturnSettingsValue("RabbitMqCore", "Username"),
                    Password = ReturnSettingsValue("RabbitMqCore", "Password")
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
            catch (Exception ex)
            {
                _log.Fatal(ex.ToString());
            }
        }
    }
}