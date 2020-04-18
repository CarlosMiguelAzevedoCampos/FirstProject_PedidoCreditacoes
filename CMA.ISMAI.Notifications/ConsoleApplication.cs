using CMA.ISMAI.Core;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Notifications.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CMA.ISMAI.Notifications
{
    public class ConsoleApplication
    {
        private readonly ILog _log;

        public ConsoleApplication(ILog log)
        {
            this._log = log;
        }
        internal void Run()
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
            catch(Exception ex)
            {
                _log.Fatal(ex.ToString());
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                string email = BaseConfiguration.ReturnSettingsValue("Notification", "Email");
                MessageBody notification = JsonConvert.DeserializeObject<MessageBody>
                    (Encoding.UTF8.GetString(e.Body));
                Console.WriteLine($"New notification recived! Message Body {notification.Message}");
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(email);
                message.To.Add(new MailAddress(notification.To));
                message.Subject = string.Format("ISMAI - {0}", notification.Message.GetType());
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = notification.Message;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(email,
                    BaseConfiguration.ReturnSettingsValue("Notification", "Password"));
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.ToString());
            }
        }
    }
}