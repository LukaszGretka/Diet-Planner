using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace DietPlanner.EmailService.MessageBroker
{
    internal class MessageBrokerManager
    {
        private readonly string HostName;
        private readonly string EmailServiceQueueName;

        public MessageBrokerManager(IConfiguration configuration)
        {
            HostName = configuration["MessageBroker:HostName"] ?? throw new ArgumentNullException();
            EmailServiceQueueName = configuration["MessageBroker:EmailServiceQueueName"] ?? throw new ArgumentNullException();
        }

        internal void RegisterEmailServiceConsumer()
        {
            var factory = new ConnectionFactory { HostName = HostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: EmailServiceQueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                // send email logic goes here...
                byte[] body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received {message}");
            };
            channel.BasicConsume(queue: EmailServiceQueueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine("Waiting to recieve messages");
        }
    }
}
