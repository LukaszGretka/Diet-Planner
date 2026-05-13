using DietPlanner.Application.Models.Dishes;
using DietPlanner.Application.Models.Dashboard;
using DietPlanner.Application.Interfaces.Services;
using DietPlanner.Domain.Enums;
using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Application.Interfaces;

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
        var measurements = await measurementService.GetAll(userId, ct);
        decimal? currentWeight = measurements.LastOrDefault()?.Weight;

        var goal = await goalService.GetGoalData(userId, GoalType.CaloricDemand, ct);
        int? caloricDemand = (int?)goal?.Value;

        var dataTimeNow = DateTime.Now.Date;
        var fromDate = dataTimeNow.AddDays(-6); // 7 days including today

        var meals = await mealRepository.GetMealsByUserAndDateRangeAsync(userId, fromDate, dataTimeNow, ct);
        var mealIds = meals.Select(m => m.Id).ToList();

        var mealDishes = await mealDishRepository.GetMealDishesByMealIdsAsync(mealIds, ct);
        var mealDishIds = mealDishes.Select(md => md.Id).ToList();

        var dishIds = mealDishes.Select(md => md.DishId).Distinct().ToList();
        var allDishProducts = new List<DishProductsDTO>();

        foreach (var mealDish in mealDishes)
        {
            var dishProducts = dishProductRepository.GetQuery().Where(dp => dp.DishId == mealDish.DishId).ToList();
            foreach (var dp in dishProducts)
            {
                var product = await productRepository.GetByIdAsync(dp.ProductId, ct);
                // Get customized meal dish if exists
                var customized = await customizedMealDishRepository.GetByMealDishIdAndDishProductIdAsync(mealDish.Id, dp.Id, ct);
                allDishProducts.Add(new DishProductsDTO
                {
                    Product = product,
                    PortionMultiplier = dp.PortionMultiplier,
                    CustomizedPortionMultiplier = customized?.CustomizedPortionMultiplier
                });
            }
        }

        // Group by date
        var datedDishProducts = meals.GroupBy(m => m.Date.Date)
            .Select(g => new DatedDishProductsDto
            {
                Date = g.Key,
                DishProducts = allDishProducts // In a real scenario, filter by meal/date
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
        var result = (stat * (float)(dishProductDTO.CustomizedPortionMultiplier ?? dishProductDTO.PortionMultiplier)) ?? 0f;
        return result;
    }
}
