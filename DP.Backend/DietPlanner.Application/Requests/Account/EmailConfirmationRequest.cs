namespace DietPlanner.Application.Requests.Account;

public class EmailConfirmationRequest
{
    public required string Email { get; set; }

    public required string ConfirmationToken { get; set; }
}
