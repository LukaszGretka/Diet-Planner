using DietPlanner.Application.Models.Dishes;
using DietPlanner.Application.Models.Dashboard;
using DietPlanner.Application.Interfaces.Services;
using DietPlanner.Domain.Enums;
using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Application.Models.UserMeasurement;
using DietPlanner.Application.Models.Goal;
using DietPlanner.Domain.Entities;

namespace DietPlanner.Application.Services;

public class DashboardService(
    IMeasurementService measurementService,
    IGoalService goalService,
    IMealRepository mealRepository,
    IMealDishRepository mealDishRepository,
    IDishProductRepository dishProductRepository,
    IProductRepository productRepository,
    ICustomizedMealDishRepository customizedMealDishRepository) : IDashboardService
{
    public async Task<DashboardData> GetDashboardData(string userId, CancellationToken ct)
    {
        DateTime dataTimeNow = DateTime.Now.Date;
        DateTime fromDate = dataTimeNow.AddDays(-6); // 7 days including today

        List<Meal> meals = await mealRepository.GetMealsByUserAndDateRangeAsync(userId, fromDate, dataTimeNow, ct);
        var mealIds = meals.Select(m => m.Id).ToList();

        var mealDishes = await mealDishRepository.GetMealDishesByMealIdsAsync(mealIds, ct);

        // Use GetQuery() to fetch all necessary data
        var allDishProducts = dishProductRepository.GetQuery().ToList();
        var allProducts = productRepository.GetQuery().ToList();
        var allCustomizedMealDishes = customizedMealDishRepository.GetQuery().ToList();

        // Group by date, then get dish products for that meal
        var datedDishProducts = meals
            .GroupBy(m => m.Date.Date)
            .Select(mealsGroup => new DatedDishProductsDto
            {
                Date = mealsGroup.Key,
                DishProducts = mealsGroup
                    .SelectMany(meal => mealDishes.Where(md => md.MealId == meal.Id))
                    .SelectMany(mealDish => allDishProducts.Where(dp => dp.DishId == mealDish.DishId)
                        .Select(dp => new DishProductsDTO
                        {
                            Product = allProducts.FirstOrDefault(p => p.Id == dp.ProductId),
                            PortionMultiplier = dp.PortionMultiplier,
                            CustomizedPortionMultiplier = allCustomizedMealDishes
                                .FirstOrDefault(cmd => cmd.MealDishId == mealDish.Id && cmd.DishProductId == dp.Id)
                                ?.CustomizedPortionMultiplier
                        }))
                    .ToList()
            }).ToList();

        List<float> caloriesLastSevenDays = new(7);
        List<float> carbsLastSevenDays = new(7);
        List<float> proteinsLastSevenDays = new(7);
        List<float> fatsLastSevenDays = new(7);

        for (int i = 0; i < 7; i++)
        {
            var date = dataTimeNow.AddDays(-i);
            var existingDishProductOnDate = datedDishProducts
                .Where(ddp => ddp.Date.Date == date)
                .Select(e => e.DishProducts)
                .FirstOrDefault();

            if (existingDishProductOnDate is not null)
            {
                caloriesLastSevenDays.Add(existingDishProductOnDate.Sum(dp => CalculateMultiplierValue(dp.Product?.Calories, dp)));
                carbsLastSevenDays.Add(existingDishProductOnDate.Sum(dp => CalculateMultiplierValue(dp.Product?.Carbohydrates, dp)));
                proteinsLastSevenDays.Add(existingDishProductOnDate.Sum(dp => CalculateMultiplierValue(dp.Product?.Proteins, dp)));
                fatsLastSevenDays.Add(existingDishProductOnDate.Sum(dp => CalculateMultiplierValue(dp.Product?.Fats, dp)));
            }
            else
            {
                caloriesLastSevenDays.Add(0f);
                carbsLastSevenDays.Add(0f);
                proteinsLastSevenDays.Add(0f);
                fatsLastSevenDays.Add(0f);
            }
        }

        caloriesLastSevenDays.Reverse();
        carbsLastSevenDays.Reverse();
        proteinsLastSevenDays.Reverse();
        fatsLastSevenDays.Reverse();

        List<MeasurementDto> measurements = await measurementService.GetAll(userId, ct);
        decimal? currentWeight = measurements.LastOrDefault()?.Weight;

        GoalDTO? goal = await goalService.GetGoalData(userId, GoalType.CaloricDemand, ct);
        int? caloricDemand = (int?)goal?.Value;

        return new DashboardData
        {
            CurrentWeight = (float?)currentWeight,
            CaloriesLastSevenDays = [.. caloriesLastSevenDays],
            CarbsLastSevenDays = [.. carbsLastSevenDays],
            ProteinsLastSevenDays = [.. proteinsLastSevenDays],
            FatsLastSevenDays = [.. fatsLastSevenDays],
            CaloricDemand = caloricDemand
        };
    }

    private static float CalculateMultiplierValue(float? stat, DishProductsDTO dishProductDTO)
    {
        var result = (stat * (float)(dishProductDTO.CustomizedPortionMultiplier ?? dishProductDTO.PortionMultiplier)) ?? 1;
        return result;
    }
}
