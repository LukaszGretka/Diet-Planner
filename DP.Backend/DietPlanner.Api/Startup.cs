using DietPlanner.Api.Configuration;
using DietPlanner.Api.Database;
using DietPlanner.Api.Models.Account;
using DietPlanner.Api.Services;
using DietPlanner.Api.Services.AccountService;
using DietPlanner.Api.Services.DishService;
using DietPlanner.Api.Services.MealProductService;
using DietPlanner.Api.Services.MealsCalendar;
using DietPlanner.Api.Services.MessageBroker;
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
                    policy.WithOrigins("http://localhost:4200")
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            services.AddDbContext<DietPlannerDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ProductsDatabase")));

            services.AddDbContext<IdentityDatabaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityDatabase")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDatabaseContext>()
                .AddSignInManager<SignInManager<IdentityUser>>()
                .AddDefaultTokenProviders();

            // Override cookie options to work with SPA
            ConfigureCookieRedirection(services);

            services.Configure<MessageBrokerOptions>(Configuration.GetSection("MessageBroker"));
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
            services.AddTransient<IMealProductService, MealProductService>();
            services.AddScoped<IValidator<SignUpRequest>, SignUpValidator>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IMessageBrokerService, MessageBrokerService>();
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
