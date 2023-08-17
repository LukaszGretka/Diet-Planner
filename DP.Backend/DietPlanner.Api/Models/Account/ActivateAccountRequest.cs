namespace DietPlanner.Api.Models.Account
{
    public class ActivateAccountRequest
    {
        public string Email { get; set; }

        public string ConfirmationToken { get; set; }
    }
}
