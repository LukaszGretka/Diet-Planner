using DietPlanner.Api.Services.Core;
using DietPlanner.Api.Services.MessageBroker;
using DietPlanner.Domain.Options;
using DietPlanner.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
        {
            string? connectionString = builder.Configuration.GetConnectionString("DietPlannerDb") ?? 
                throw new ArgumentNullException("Connection string for DietPlannerDb is not provided.");

            builder.Services.AddDbContext<DietPlannerDbContext>(options => options.UseSqlite(connectionString));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<DietPlannerDbContext>()
                .AddSignInManager<SignInManager<IdentityUser>>()
                .AddDefaultTokenProviders();

            builder.Services.AddStackExchangeRedisCache(options =>
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

            builder.Services.Configure<MessageBrokerOptions>(builder.Configuration.GetSection("MessageBroker"));
            builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();
            builder.Services.AddTransient<IMessageBrokerService, MessageBrokerService>();
        }
    }
}