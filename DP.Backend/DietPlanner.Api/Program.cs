using DietPlanner.Api.Extensions;
using DietPlanner.Api.Requests.Account;
using DietPlanner.Api.Services;
using DietPlanner.Api.Services.DishService;
using DietPlanner.Api.Validators;
using DietPlanner.Application.Interfaces;
using DietPlanner.Application.Services;
using FluentValidation;
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

builder.AddApplicationServices();

//TODO move to Application layer and register them there
#region should be moved to application services (DI) later

builder.Services.AddTransient<IDishService, DishService>();
builder.Services.AddTransient<IMeasurementService, MeasurementService>();
builder.Services.AddScoped<IValidator<SignUpRequest>, SignUpValidator>();

#endregion

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

app.Run();
