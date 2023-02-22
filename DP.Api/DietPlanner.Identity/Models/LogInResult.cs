using Duende.IdentityServer.Endpoints.Results;
using System.Net;

namespace DietPlanner.Api.Models
{
    public class LogInResult : StatusCodeResult
    {
        public LogInResult(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public bool IsSuccess { get; set; }

        public Exception Exception { get; set; }
    }
}
