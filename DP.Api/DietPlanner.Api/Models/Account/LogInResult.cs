using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace DietPlanner.Api.Models.Account
{
    public class LogInResult : StatusCodeResult
    {
        public LogInResult(HttpStatusCode statusCode) : base((int)statusCode)
        {
        }

        public bool IsSuccess { get; set; }

        public Exception Exception { get; set; }
    }
}
