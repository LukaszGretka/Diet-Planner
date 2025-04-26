using DietPlanner.Api.Configuration;
using DietPlanner.Api.Database;
using DietPlanner.Api.Database.Repository;
using DietPlanner.Api.Models.Account;
using DietPlanner.Api.Services;
using DietPlanner.Api.Services.AccountService;
using DietPlanner.Api.Services.Dashboard;
using DietPlanner.Api.Services.DishService;
using DietPlanner.Api.Services.MealProductService;
using DietPlanner.Api.Services.MealsCalendar;
using DietPlanner.Api.Services.MessageBroker;
using DietPlanner.Api.Services.UserProfileService;
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
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DietPlanner.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        const string CorsPolicyName = "DefaultCorsPolicy";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy(name: CorsPolicyName, policy =>
                {
                    policy.WithOrigins("http://localhost:4200",
                        "http://192.168.0.51",
                        "http://192.168.0.51:4200",
                        "http://192.168.0.51:5000",
                        "https://diet-planner.azurewebsites.net")
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            services.AddHttpLogging();
            services.AddDbContext<DietPlannerDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DietPlannerDb")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<DietPlannerDbContext>()
                .AddSignInManager<SignInManager<IdentityUser>>()
                .AddDefaultTokenProviders();

            // Override cookie options to work with SPA
            ConfigureCookieRedirection(services);

#if DEBUG
            services.Configure<MessageBrokerOptions>(Configuration.GetSection("MessageBroker"));
#endif
            services.Configure<IdentityOptions>(options =>
            {
                ConfigurePasswordPolicy(options);
                options.SignIn.RequireConfirmedAccount = false; // confirmation by email required
            });

            services.AddAuthorization();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IValidator<SignUpRequest>, SignUpValidator>();
            services.AddTransient<IDishService, DishService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IMeasurementService, MeasurementService>();
            services.AddTransient<IMealService, MealService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddScoped<IValidator<SignUpRequest>, SignUpValidator>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IMessageBrokerService, MessageBrokerService>();
            services.AddTransient<IUserProfileService, UserProfileService>();
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<IGoalService, GoalService>();
            services.AddTransient<IMealCalendarRepository, MealCalendarRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
                
            app.UseHttpLogging();
            app.UseRouting();
            app.UseCors(CorsPolicyName);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void ConfigurePasswordPolicy(IdentityOptions options)
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
        }

        private static void ConfigureCookieRedirection(IServiceCollection services)
        {
            services.ConfigureApplicationCookie(o =>
            {
                o.Cookie.HttpOnly = true;
                o.Cookie.SameSite = SameSiteMode.Lax; // Use Lax instead of None for HTTP
                o.Cookie.SecurePolicy = CookieSecurePolicy.None; // Change to Always if using HTTPS

                o.Events = new CookieAuthenticationEvents() 
                {
                    OnRedirectToLogin = (ctx) =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                        {
                            ctx.Response.StatusCode = 401;
                            ctx.Response.WriteAsJsonAsync(new { redirectUri = ctx.RedirectUri });
                        }

                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = (ctx) =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                        {
                            ctx.Response.StatusCode = 403;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
