using DietPlanner.Api.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DietPlanner.Api.Services.MessageBroker
{
    public class MessageBrokerService : IMessageBrokerService
    {
        private readonly ConnectionFactory connectionFactory;
        private readonly string _emailServiceQueueName;

        public MessageBrokerService(IOptions<MessageBrokerOptions> options)
        {
            connectionFactory = new ConnectionFactory { HostName = options.Value.HostName };
            _emailServiceQueueName = options.Value.EmailServiceQueueName;
        }

        public void BroadcastSignUpEmail(string email)
        {
            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _emailServiceQueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            string message = $"Sending email to {email} (test)";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: _emailServiceQueueName,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
