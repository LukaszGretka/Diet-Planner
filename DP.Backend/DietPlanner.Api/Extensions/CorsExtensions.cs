using DietPlanner.Domain.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DietPlanner.Api.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCorsPolicies(this IServiceCollection services, IConfiguration configuration) 
        {
            string spaHostAddress = configuration.GetSection("SpaConfig:HostAddress").Value;

            services.AddCors(options =>
                {
                    options.AddPolicy(name: CorsPolicy.Name, policy =>
                    {
                        policy.WithOrigins(
                            spaHostAddress,
                            "http://192.168.0.51",
                            "http://192.168.0.51:4200",
                            "http://192.168.0.51:5000")
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
