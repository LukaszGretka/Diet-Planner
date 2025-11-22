using Microsoft.AspNetCore.Identity;

namespace DietPlanner.Application.DTO.Identity
{
    public class SignupResult : IdentityResult
    {
        public bool RequireEmailConfirmation { get; init; } = false;
    }
}
