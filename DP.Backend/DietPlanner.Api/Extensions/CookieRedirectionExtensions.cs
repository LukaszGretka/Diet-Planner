using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DietPlanner.Api.Extensions
{
    public static class CookieRedirectionExtensions
    {
        /// <summary>
        ///  Overrides cookie options to work with SPA
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCookieRedirection(this IServiceCollection services)
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
