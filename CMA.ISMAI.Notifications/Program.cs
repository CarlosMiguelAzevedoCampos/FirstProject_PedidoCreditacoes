using CMA.ISMAI.Core.Notifications;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace CMA.ISMAI.Notifications
{
    class Program
    {
        static void Main(string[] args)
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

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += Consumer_Received;
                channel.BasicConsume(queue: "NotificationsQueue",
                     autoAck: true,
                     consumer: consumer);

                Console.WriteLine("Processing Notifications..");
                Console.ReadKey();
            }
        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            MessageBody notification = JsonConvert.DeserializeObject<MessageBody>(Encoding.UTF8.GetString(e.Body));
            Console.WriteLine($"New notification recived! Message Body {notification.Message}");

        }
    }
}
