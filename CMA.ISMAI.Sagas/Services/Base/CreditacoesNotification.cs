using CMA.ISMAI.Core.Notifications;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace CMA.ISMAI.Sagas.Services.Base
{
    public class CreditacoesNotification : ICreditacoesNotification
    {
        public void SendNotification(MessageBody notifications)
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
