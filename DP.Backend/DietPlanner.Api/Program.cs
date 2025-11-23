using DietPlanner.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CorsPolicy = DietPlanner.Api.Configuration.CorsPolicy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpLogging();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddCorsPolicies(builder.Configuration);
builder.Services.ConfigureCookieRedirection();

builder.Services.AddAuthorization();

builder.AddInfrastructureServices();
builder.AddApplicationServices(); // TODO register it and move all required services to Application layer

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseCors(CorsPolicy.Name);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseHttpLogging();