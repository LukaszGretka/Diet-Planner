namespace DietPlanner.Api.Models.Account
{
    public class SignUpResponse
    {
        public string UserName { get; set; }

        public bool RequireEmailConfirmation { get; set; }
    }
}
