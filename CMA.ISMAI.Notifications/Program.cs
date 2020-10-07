using CMA.ISMAI.Core;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Logging.Service;
using CMA.ISMAI.Notifications.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CMA.ISMAI.Notifications
{
    class Program
    {
        private static ServiceProvider serviceProvider;
        static async Task Main(string[] args)
        {
            var hostBuilder = new HostBuilder();
            var services = ConfigureServices();

            serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddSerilog();

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

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += Consumer_Received;
                    channel.BasicConsume(queue: BaseConfiguration.ReturnSettingsValue("RabbitMq", "Queue"),
                         autoAck: true,
                         consumer: consumer);

                    await hostBuilder.RunConsoleAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0}, RabbitMQ starting..? ", ex.ToString()));
                Console.WriteLine("Retrying in 30 seconds..");
                serviceProvider.GetService<ILog>().Fatal(string.Format("{0}, RabbitMQ starting..? ", ex.ToString()));
                Thread.Sleep(30000);
                await Main(args);
            }
        }


        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
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
                message.Subject = "ISMAI - Trello";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = notification.Message;
                smtp.Port = Convert.ToInt32(BaseConfiguration.ReturnSettingsValue("Notification", "Port"));
                smtp.Host = BaseConfiguration.ReturnSettingsValue("Notification", "Host");  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(email,
                    BaseConfiguration.ReturnSettingsValue("Notification", "Password"));
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                serviceProvider.GetService<ILog>().Fatal(ex.ToString());
            }
        }


        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(BaseConfiguration.ReturnSettingsValue("ElasticConfiguration", "Uri")))
               {
                   AutoRegisterTemplate = true,
                   IndexFormat = "TrelloISMAINotification"
               })
            .CreateLogger();

            services.AddLogging();
            services.AddScoped<ILog, LoggingService>();
            return services;
        }
    }
}
