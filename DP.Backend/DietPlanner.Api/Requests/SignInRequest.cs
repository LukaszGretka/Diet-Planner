using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Api.Requests
{
    public class SignInRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        // Navigates user after successful sign-in. If not defined, navigates to dashboard
        public string ReturnUrl { get; set; }
    }
}
