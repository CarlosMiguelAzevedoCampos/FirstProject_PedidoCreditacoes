using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Service.Interface;
using CMA.ISMAI.Sagas.Service.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace CMA.ISMAI.Sagas.Service
{
    public class SagaNotificationService : ISagaNotification
    {
        private readonly ILog _log;

        public SagaNotificationService(ILog log)
        {
            _log = log;
        }

        public void SendNotification(string to, string text)
        {
            try
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

                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new MessageBody(to, text)));

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
