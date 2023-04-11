using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Api.Models.Account
{
    public class LogInRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        // Navigates user after successful sign-in. If not defined, navigates to dashboard
        public string ReturnUrl { get; set; }
    }
}
