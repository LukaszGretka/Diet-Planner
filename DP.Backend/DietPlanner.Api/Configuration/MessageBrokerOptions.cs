namespace DietPlanner.Api.Configuration
{
    public class MessageBrokerOptions
    {
        public string HostName { get; set; }
        public string EmailServiceQueueName { get; set; }
    }
}