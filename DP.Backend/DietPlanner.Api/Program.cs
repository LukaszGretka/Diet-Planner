using DietPlanner.Api.Database.Repository;
using DietPlanner.Api.Extensions;
using DietPlanner.Api.Requests.Account;
using DietPlanner.Api.Services;
using DietPlanner.Api.Services.Dashboard;
using DietPlanner.Api.Services.DishService;
using DietPlanner.Api.Services.MealProductService;
using DietPlanner.Api.Services.MealsCalendar;
using DietPlanner.Api.Services.UserProfileService;
using DietPlanner.Api.Validators;
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

//TODO move all repositories to Infrastructure layer and register them there
#region should be moved to interfaces layer
builder.Services.AddTransient<IMealCalendarRepository, MealCalendarRepository>();
#endregion

builder.AddApplicationServices(); // TODO register it and move all required services to Application layer

//TODO move to Application layer and register them there
#region should be moved to application services (DI) later

builder.Services.AddTransient<IDishService, DishService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IMeasurementService, MeasurementService>();
builder.Services.AddTransient<IMealService, MealService>();
builder.Services.AddTransient<IUserProfileService, UserProfileService>();
builder.Services.AddTransient<IDashboardService, DashboardService>();
builder.Services.AddTransient<IGoalService, GoalService>();
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