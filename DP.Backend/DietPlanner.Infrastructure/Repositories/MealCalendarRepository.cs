using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Application.Models.MealsCalendar;
using DietPlanner.Domain.Entities;
using DietPlanner.Domain.Enums;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DietPlanner.Infrastructure.Repositories;

public class MealCalendarRepository(DietPlannerDbContext databaseContext) : IMealCalendarRepository
{
    public async Task<List<MealDishDto>> GetMealDishes(Meal meal, CancellationToken ct)
    {
        var mealDishes = await databaseContext.MealDishes
            .Where(md => md.MealId == meal.Id)
            .Include(md => md.Dish)
            .ToListAsync(ct);

        var dishProducts = await databaseContext.DishProducts
            .Where(dp => mealDishes.Select(md => md.DishId).Contains(dp.DishId))
            .Include(dp => dp.Product)
            .ToListAsync(ct);

        var customizedMealDishes = await databaseContext.CustomizedMealDishes
            .Where(cdp => mealDishes.Select(md => md.Id).Contains(cdp.MealDishId))
            .ToListAsync(ct);

        return [.. mealDishes.Select(md => new MealDishDto
        {
            Id = md.Dish.Id,
            MealItemId = md.Id,
            Description = md.Dish.Description,
            ExposeToOtherUsers = md.Dish.ExposeToOtherUsers,
            ImagePath = md.Dish.ImagePath,
            IsOwner = md.Dish.UserId == meal.UserId,
            ItemType = ItemType.Dish,
            Name = md.Dish.Name,
            Products = [.. dishProducts
                .Where(dp => dp.DishId == md.DishId)
                .Select(dp => new MealDishProductDto
                {
                    DishProductId = dp.Id,
                    Product = new MealProductDto
                    {
                        Id = dp.Product.Id,
                        Name = dp.Product.Name,
                        Description = dp.Product.Description,
                        ImagePath = dp.Product.ImagePath,
                        BarCode = dp.Product.BarCode,
                        Calories = (float)dp.Product.Calories,
                        Carbohydrates = (float)dp.Product.Carbohydrates,
                        Proteins = (float)dp.Product.Proteins,
                        Fats = (float)dp.Product.Fats,
                    },
                    PortionMultiplier = dp.PortionMultiplier,
                    CustomizedPortionMultiplier = customizedMealDishes
                        .Where(cdp => cdp.MealDishId == md.Id && cdp.DishProductId == dp.Id)
                        .Select(cmd => cmd.CustomizedPortionMultiplier)
                        .SingleOrDefault()
                })]
        })];
    }

    public async Task<List<MealProductDto>> GetMealProducts(Meal meal, CancellationToken ct)
    {
        List<MealProductDto> result = await databaseContext.MealProducts.Where(mp => mp.MealId == meal.Id)
            .Select(mp => new MealProductDto
            {
                MealItemId = mp.Id,
                Id = mp.Product.Id,
                Name = mp.Product.Name,
                Description = mp.Product.Description,
                ImagePath = mp.Product.ImagePath,
                ItemType = ItemType.Product,
                BarCode = mp.Product.BarCode,
                Calories = (float)mp.Product.Calories,
                Carbohydrates = (float)mp.Product.Carbohydrates,
                Proteins = (float)mp.Product.Proteins,
                Fats = (float)mp.Product.Fats,
                PortionMultiplier = databaseContext.CustomizedMealProducts
                    .Where(cmp => cmp.MealProductId == mp.Id)
                    .Select(cmp => cmp.CustomizedPortionMultiplier)
                    .FirstOrDefault()
            }).ToListAsync(ct);

        result.Where(r => r.PortionMultiplier is null or 0m)
            .ToList()
            .ForEach(r => r.PortionMultiplier = 1.0m);

        return result;
    }
}
