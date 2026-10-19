using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Application.Requests.Account;

public class SignInRequest
{
    [Required]
    public required string UserName { get; set; }

    [Required]
    public required string Password { get; set; }

    // Navigates user after successful sign-in. If not defined, navigates to dashboard
    public string? ReturnUrl { get; set; }
}
