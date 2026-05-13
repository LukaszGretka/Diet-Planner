using DietPlanner.Application.Interfaces;
using DietPlanner.Application.Interfaces.Services;
using DietPlanner.Application.Services;
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

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserProfileService, UserProfileService>();
            services.AddScoped<IMealService, MealService>();
            services.AddTransient<IGoalService, GoalService>();
            services.AddScoped<IProductService, ProductService>();
            //services.AddTransient<IDishService, DishService>();
            //services.AddTransient<IMeasurementService, MeasurementService>();
            //services.AddValidators();
        }

        //private static void AddValidators(this IServiceCollection services)
        //{
        //    services.AddScoped<IValidator<SignUpRequest>, SignUpValidator>();
        //}
    }
}

