using DietPlanner.Application.Interfaces;
using DietPlanner.Application.Interfaces.Common;
using DietPlanner.Infrastructure.Adapters;
using DietPlanner.Infrastructure.Database;
using DietPlanner.Infrastructure.Options;
using DietPlanner.Infrastructure.Services;
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

            string? databaseConnectionString = builder.Configuration.GetConnectionString("DietPlannerDb");

            if (string.IsNullOrEmpty(databaseConnectionString))
            {
                throw new ArgumentNullException("Connection string for DietPlannerDb is not provided.");
            }

            services.AddDbContext<DietPlannerDbContext>(options => options.UseSqlite(databaseConnectionString));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<DietPlannerDbContext>()
                .AddSignInManager<SignInManager<IdentityUser>>()
                .AddDefaultTokenProviders();

            string? redisConnectionString = builder.Configuration.GetConnectionString("Redis");

            if(string.IsNullOrEmpty(redisConnectionString))
            {
                throw new ArgumentNullException("Connection string for Redis is not provided.");
            }

            services.AddStackExchangeRedisCache(options =>
            {
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
            services.AddTransient<IAccountManagerAdapter, AccountManagerAdapter>();
        }
    }
}