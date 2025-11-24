namespace DietPlanner.Application.Interfaces.Common
{
    public interface IMessageBrokerService
    {
        public void BroadcastSignUpEmail(string email, string emailConfirmationLink);
    }
}
