namespace DietPlanner.Api.Requests
{
    public class ChangePasswordRequest(string currentPassword, string newPassword, string newPasswordConfirmed)
    {
        public string CurrentPassword { get; } = currentPassword;

        public string NewPassword { get; } = newPassword;

        public string NewPasswordConfirmed { get; } = newPasswordConfirmed;
    }
}
