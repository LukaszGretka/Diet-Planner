namespace DietPlanner.Domain.Options
{
    public class MessageBrokerOptions
    {
        public string HostName { get; set; }
        public string EmailServiceQueueName { get; set; }
    }
}