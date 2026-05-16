using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Configuration;
using DietPlanner.EmailService.Emails;
using Newtonsoft.Json;
using DietPlanner.Shared.Models;

namespace DietPlanner.EmailService.MessageBroker;

internal class MessageBrokerManager(IConfiguration configuration)
{
    internal async void RegisterEmailServiceConsumer()
    {
        var factory = new ConnectionFactory { HostName = configuration["MessageBroker:HostName"] ?? throw new ArgumentNullException() };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        var emailServiceQueueName = configuration["MessageBroker:EmailServiceQueueName"] ?? throw new ArgumentNullException();

        await channel.QueueDeclareAsync(queue: emailServiceQueueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        Console.WriteLine("Waiting to recieve messages");

        var consumer = new AsyncEventingBasicConsumer(channel);
        var emailSenderManager = new EmailSenderManager(configuration);
        consumer.ReceivedAsync += (model, eventArgs) =>
        {
            var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            Console.WriteLine($"[{DateTime.UtcNow}] Received message with body: {message}");
            var deserializedMessage = JsonConvert.DeserializeObject<SignUpAccountConfirmationEmail>(message);

            if (deserializedMessage is null)
            {
                Console.WriteLine($"Error during message deserialization!");
                return Task.CompletedTask;
            }
            emailSenderManager.SendRegistrationEmail(deserializedMessage);
            return Task.CompletedTask;
        };
        await channel.BasicConsumeAsync(queue: emailServiceQueueName,
                             autoAck: true,
                             consumer: consumer);

        Console.WriteLine("Press any button to exit.");
        Console.ReadLine();
    }
}
