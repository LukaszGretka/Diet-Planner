namespace DietPlanner.Api.Services.MessageBroker
{
    public interface IMessageBrokerService
    {
        public void BroadcastSignUpEmail(string email);
    }
}
