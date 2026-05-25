namespace DietPlanner.Application.Requests.Account;

public class ChangePasswordRequest()
{
    public required string CurrentPassword { get; init; }

    public required string NewPassword { get; init; }

    public required string NewPasswordConfirmed { get; init; }
}
