namespace DietPlanner.Application.Interfaces.Services;

public interface IMessageBrokerService
{
    void BroadcastSignUpEmail(string email, string emailConfirmationLink);
}
