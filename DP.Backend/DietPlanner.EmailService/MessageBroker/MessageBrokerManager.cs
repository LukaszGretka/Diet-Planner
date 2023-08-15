using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Configuration;
using DietPlanner.EmailService.Emails;
using Newtonsoft.Json;
using DietPlanner.Shared.Models;
using System;

namespace DietPlanner.EmailService.MessageBroker
{
    internal class MessageBrokerManager
    {
        private readonly IConfiguration _configuration;

        public MessageBrokerManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        internal void RegisterEmailServiceConsumer()
        {
            var factory = new ConnectionFactory { HostName = _configuration["MessageBroker:HostName"] ?? throw new ArgumentNullException() };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var emailServiceQueueName = _configuration["MessageBroker:EmailServiceQueueName"] ?? throw new ArgumentNullException();

            channel.QueueDeclare(queue: emailServiceQueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            Console.WriteLine("Waiting to recieve messages");

            var consumer = new EventingBasicConsumer(channel);
            var emailSenderManager = new EmailSenderManager(_configuration);
            consumer.Received += (model, eventArgs) =>
            {
                var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                Console.WriteLine($"Received {message}");
                var deserializedMessage = JsonConvert.DeserializeObject<SignUpAccountConfirmationEmail>(message);

                if(deserializedMessage is null)
                {
                    Console.WriteLine($"Error during message deserialization!");
                    return;
                }
                emailSenderManager.SendRegistrationEmail(deserializedMessage);

            };
            channel.BasicConsume(queue: emailServiceQueueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
