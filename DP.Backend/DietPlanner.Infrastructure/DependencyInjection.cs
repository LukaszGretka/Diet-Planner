using DietPlanner.Api.Database.Repository;
using DietPlanner.Api.Services.Core;
using DietPlanner.Api.Services.MessageBroker;
using DietPlanner.Application.Interfaces;
using DietPlanner.Application.Services;
using DietPlanner.Domain.Options;
using DietPlanner.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

#pragma warning disable IDE0130
namespace Microsoft.Extensions.DependencyInjection
#pragma warning restore IDE0130 
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
        {
            IServiceCollection services = builder.Services;

            string? connectionString = builder.Configuration.GetConnectionString("DietPlannerDb") ?? 
                throw new ArgumentNullException("Connection string for DietPlannerDb is not provided.");

            services.AddDbContext<DietPlannerDbContext>(options => options.UseSqlite(connectionString));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<DietPlannerDbContext>()
                .AddSignInManager<SignInManager<IdentityUser>>()
                .AddDefaultTokenProviders();

            services.AddStackExchangeRedisCache(options =>
            {
                string? redisConnectionString = builder.Configuration.GetConnectionString("Redis") ??
                    throw new ArgumentNullException("Connection string for Redis is not provided.");

                options.InstanceName = "DietPlannerRedis";
                options.Configuration = redisConnectionString;
                options.ConfigurationOptions = new ConfigurationOptions
                {
                    ConnectTimeout = 100,
                    SyncTimeout = 100,
                    AbortOnConnectFail = false,
                    EndPoints = { redisConnectionString }
                };
            });

            services.Configure<MessageBrokerOptions>(builder.Configuration.GetSection("MessageBroker"));
            services.AddSingleton<IRedisCacheService, RedisCacheService>();
            services.AddTransient<IMessageBrokerService, MessageBrokerService>();

            services.AddTransient<IMealCalendarRepository, MealCalendarRepository>(); //TODO: should it be here?
        }
    }
}