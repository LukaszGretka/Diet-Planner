using DietPlanner.Api.Extensions;
using DietPlanner.Domain.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpLogging();

builder.Services.AddCorsPolicies(builder.Configuration);
builder.Services.ConfigureCookieRedirection();

builder.AddInfrastructureServices();
builder.AddApplicationServices();

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