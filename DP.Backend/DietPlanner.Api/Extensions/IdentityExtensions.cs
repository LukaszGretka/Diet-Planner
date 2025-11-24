using DietPlanner.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DietPlanner.Api.Extensions
{
    public static class IdentityExtensions
    {
        public static void AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<DietPlannerDbContext>()
                .AddSignInManager<SignInManager<IdentityUser>>()
                .AddDefaultTokenProviders();
        }

        public static void ConfigurePasswordPolicy(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedAccount = false; // confirmation by email required
            });
        }
    }
}