using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace DietPlanner.Api.Extensions
{
    public static class UserExtentions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if(httpContext is null)
            {
                return string.Empty;
            }

            return httpContext.User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
        }
    }
}
