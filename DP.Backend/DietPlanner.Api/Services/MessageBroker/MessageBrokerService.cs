using DietPlanner.Api.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Newtonsoft.Json;
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

        public async void BroadcastSignUpEmail(string email, string emailConfirmationLink)
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: _emailServiceQueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            string serializedMessage = JsonConvert.SerializeObject(new { Email = email, ConfirmationLink = emailConfirmationLink });
            byte[] body = Encoding.UTF8.GetBytes(serializedMessage);

            await channel.BasicPublishAsync(exchange: string.Empty,
                                 routingKey: _emailServiceQueueName,
                                 body: body);
        }
    }
}
