using DietPlanner.Application.Interfaces;
using DietPlanner.Application.Interfaces.Common;
using DietPlanner.Infrastructure.Options;
using DietPlanner.Infrastructure.Services;
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
        }
    }
}