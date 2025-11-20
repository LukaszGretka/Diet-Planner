using DietPlanner.Api.Configuration;
using DietPlanner.Api.Models.Account;
using DietPlanner.Api.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace DietPlanner.Api
{
    public class Startup(IConfiguration configuration)
    {
        private IConfiguration Configuration { get; init; } = configuration;


        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                ConfigurePasswordPolicy(options);
                options.SignIn.RequireConfirmedAccount = false; // confirmation by email required
            });

            services.AddAuthorization();

            services.AddSingleton<IRedisCacheService, RedisCacheService>();
            services.AddTransient<IMessageBrokerService, MessageBrokerService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IValidator<SignUpRequest>, SignUpValidator>();
            services.AddTransient<IDishService, DishService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IMeasurementService, MeasurementService>();
            services.AddTransient<IMealService, MealService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddScoped<IValidator<SignUpRequest>, SignUpValidator>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUserProfileService, UserProfileService>();
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<IGoalService, GoalService>();
            services.AddTransient<IMealCalendarRepository, MealCalendarRepository>();
        }


        private static void ConfigurePasswordPolicy(IdentityOptions options)
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
        }
    }
}
