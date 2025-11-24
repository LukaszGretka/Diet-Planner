using DietPlanner.Domain.Entities.Account;
using Microsoft.AspNetCore.Identity;


namespace DietPlanner.Infrastructure.Extensions
{
    internal static class IdentityUserExtensions
    {
        internal static ApplicationUser? ToApplicationUser(this IdentityUser identityUser)
        {
            if (identityUser is null)
            {
                return null;
            }

            return new ApplicationUser
            {
                Id = identityUser.Id,
                UserName = identityUser.UserName,
                Email = identityUser.Email
            };
        }
    }
}
