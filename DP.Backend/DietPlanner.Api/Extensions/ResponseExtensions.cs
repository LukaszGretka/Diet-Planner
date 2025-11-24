using DietPlanner.Domain.Constants;

namespace DietPlanner.Api.Extensions
{
    public static class ResponseExtensions
    {
        // Secures error code to prevent leaking sensitive information like user existence
        public static string NormalizeErrorCode(this string _)
        {
           return ErrorCodes.GeneralError;
        }
    }
}
