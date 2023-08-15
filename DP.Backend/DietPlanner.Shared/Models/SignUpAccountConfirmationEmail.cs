namespace DietPlanner.Shared.Models
{
    public class SignUpAccountConfirmationEmail
    {
        public string Email { get; set; }

        public string AccountConfirmationToken { get; set; }
    }
}
