namespace DietPlanner.Application.Requests.Account;

public class SignUpRequest
{
    public required string Username { get; init; }

    public required string Email { get; init; }

    public required string Password { get; init; }
}
