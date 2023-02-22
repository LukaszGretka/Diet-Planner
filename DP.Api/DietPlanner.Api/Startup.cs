using DietPlanner.Api.Database;
using DietPlanner.Api.Services;
using DietPlanner.Api.Services.Account;
using DietPlanner.Api.Services.MealsCalendar;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

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
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("ProductsDatabase")));

            services.AddDbContext<IdentityDatabaseContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("IdentityDatabase")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDatabaseContext>()
                .AddSignInManager();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            });

            services.AddAuthentication();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
            });
            services.AddAuthorization();

            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IMeasurementService, MeasurementService>();
            services.AddTransient<IMealsCalendarService, MealsCalendarService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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
    }
}
