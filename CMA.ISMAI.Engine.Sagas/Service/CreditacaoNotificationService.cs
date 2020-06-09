using CMA.ISMAI.Core;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Sagas.Service.Interface;
using CMA.ISMAI.Sagas.Service.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace CMA.ISMAI.Sagas.Service
{
    public class CreditacaoNotificationService : ISagaNotification
    {
        private readonly ILog _log;

        public CreditacaoNotificationService(ILog log)
        {
            _log = log;
        }

        public void SendNotification(string to, string text)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = BaseConfiguration.ReturnSettingsValue("RabbitMq", "Uri"),
                    Port = Convert.ToInt32(BaseConfiguration.ReturnSettingsValue("RabbitMq", "Port")),
                    UserName = BaseConfiguration.ReturnSettingsValue("RabbitMq", "Username"),
                    Password = BaseConfiguration.ReturnSettingsValue("RabbitMq", "Password")
                };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: BaseConfiguration.ReturnSettingsValue("RabbitMq", "Queue"),
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new MessageBody(to, text)));

                    channel.BasicPublish(exchange: "",
                                         routingKey: BaseConfiguration.ReturnSettingsValue("RabbitMq", "Queue"),
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
