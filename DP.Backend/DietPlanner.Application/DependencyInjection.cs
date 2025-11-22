using DietPlanner.Application.Interfaces;
using DietPlanner.Application.Services;
using DietPlanner.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.Hosting;

#pragma warning disable IDE0130
namespace Microsoft.Extensions.DependencyInjection
#pragma warning restore IDE0130 
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            IServiceCollection services = builder.Services;

            services.AddTransient<IDishService, DishService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IMeasurementService, MeasurementService>();
            services.AddTransient<IMealService, MealService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserProfileService, UserProfileService>();
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<IGoalService, GoalService>();
            services.AddValidators();
        }

        private static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<SignUpRequest>, SignUpValidator>();
        }
    }
}