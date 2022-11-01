using DietPlanner.Identity.Database;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = true)
.AddEntityFrameworkStores<IdentityDatabaseContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<IdentityUser, IdentityDatabaseContext>();

var app = builder.Build();
app.UseIdentityServer();

app.Run();