namespace DietPlanner.Infrastructure.Options
{
    public class MessageBrokerOptions
    {
        public required string HostName { get; set; }
        public required string EmailServiceQueueName { get; set; }
    }
}