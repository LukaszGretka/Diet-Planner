using DietPlanner.Api.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using System;
using Microsoft.Extensions.Configuration;

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

        public void BroadcastSignUpEmail(string email, string emailConfirmationLink)
        {
            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _emailServiceQueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            string serializedMessage = JsonConvert.SerializeObject(new { Email = email, ConfirmationLink = emailConfirmationLink });
            byte[] body = Encoding.UTF8.GetBytes(serializedMessage);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: _emailServiceQueueName,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
