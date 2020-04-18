using CMA.ISMAI.Core;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.MessageBroker.Interface;
using CMA.ISMAI.Trello.MessageBroker.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace CMA.ISMAI.Trello.MessageBroker.Service
{
    public class SendNotificationService : ISendNotificationService
    {
        private readonly ILog _log;

        public SendNotificationService(ILog log)
        {
            _log = log;
        }
        public void SendNotificationToBroker(string to, string text)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = BaseConfiguration.ReturnSettingsValue("RabbitMqCore", "Uri"),
                    Port = Convert.ToInt32(BaseConfiguration.ReturnSettingsValue("RabbitMqCore", "Port")),
                    UserName = BaseConfiguration.ReturnSettingsValue("RabbitMqCore", "Username"),
                    Password = BaseConfiguration.ReturnSettingsValue("RabbitMqCore", "Password")
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
