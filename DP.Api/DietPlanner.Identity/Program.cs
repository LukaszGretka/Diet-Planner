using DietPlanner.Identity.Database;
using DietPlanner.Identity.Services.SignUp;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDefaultIdentity<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = true)
.AddEntityFrameworkStores<IdentityDatabaseContext>();

services.AddIdentityServer()
    .AddApiAuthorization<IdentityUser, IdentityDatabaseContext>();

services.AddTransient<ISignUpService, SignUpService>();

var app = builder.Build();
app.UseIdentityServer();

app.Run();