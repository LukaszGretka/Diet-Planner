namespace DietPlanner.Api.Models.Account
{
    public class EmailConfirmationRequest
    {
        public string Email { get; set; }

        public string ConfirmationToken { get; set; }
    }
}
