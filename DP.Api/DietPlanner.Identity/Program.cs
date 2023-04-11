using DietPlanner.Api.Database;
using DietPlanner.Api.Services.SignUp;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;


services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 12;
});

services.AddAuthentication("cookie").AddCookie("cookie", options =>
{
    options.Cookie.Name = "authCookie";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
});
services.AddAuthorization();
services.AddControllers();

services.AddTransient<IAccountService, AccountService>();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.Run();