using DietPlanner.Api.Services;
using DietPlanner.Api.Services.Dashboard;
using DietPlanner.Api.Services.DishService;
using DietPlanner.Api.Services.MealsCalendar;
using DietPlanner.Api.Services.UserProfileService;
using DietPlanner.Api.Validators;
using DietPlanner.Application.Interfaces;
using DietPlanner.Application.Services;
using FluentValidation;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
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