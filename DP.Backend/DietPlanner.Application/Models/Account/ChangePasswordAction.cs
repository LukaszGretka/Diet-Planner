namespace DietPlanner.Application.Models.Account
{
    public class ChangePasswordAction
    {
        public required string UserId { get; init; }

        public required string CurrentPassword { get; init; }

        public required string NewPassword { get; init; }

        public required string ConfirmedNewPassword { get; init; }
    }
}
