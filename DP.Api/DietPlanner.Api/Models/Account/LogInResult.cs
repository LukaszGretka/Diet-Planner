using Microsoft.AspNetCore.Identity;

namespace DietPlanner.Api.Models.Account
{
    public class LogInResult
    {
        public string Username { get; set; }

        // Navigates user after successful sign-in. If not defined, navigates to dashboard
        public string ReturnUrl { get; set; }
    }
}
