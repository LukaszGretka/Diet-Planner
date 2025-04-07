using DietPlanner.Api.Database.Models;
using DietPlanner.Api.DTO;
using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Api.DTO.Products;
using DietPlanner.Api.Models.MealsCalendar.DbModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DietPlanner.Api.Database.Repository
{
    public class MealCalendarRepository: IMealCalendarRepository
    {
        private readonly DietPlannerDbContext _databaseContext;

        public MealCalendarRepository(DietPlannerDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<DishDTO> GetMealDishes(Meal meal)
        {          
            var mealDishes = _databaseContext.MealDishes
                .Where(md => md.MealId == meal.Id)
                .Include(md => md.Dish)
                .ToList();

            var dishProducts = _databaseContext.DishProducts
                .Where(dp => mealDishes.Select(md => md.DishId).Contains(dp.DishId))
                .Include(dp => dp.Product)
                .ToList();

            var customizedMealDishes = _databaseContext.CustomizedMealDishes
                .Where(cdp => mealDishes.Select(md => md.Id).Contains(cdp.MealDishId))
                .ToList();

            return mealDishes.Select(md => new DishDTO
            {
                Id = md.Dish.Id,
                MealItemId = md.Id,
                Description = md.Dish.Description,
                ExposeToOtherUsers = md.Dish.ExposeToOtherUsers,
                ImagePath = md.Dish.ImagePath,
                IsOwner = md.Dish.UserId == meal.UserId,
                ItemType = ItemType.Dish,
                Name = md.Dish.Name,
                Products = dishProducts
                    .Where(dp => dp.DishId == md.DishId)
                    .Select(dp => new DishProductsDTO
                    {
                        Product = new Product
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
                    }).ToList(),
            }).ToList();
        }

        public List<ProductDTO> GetMealProducts(Meal meal)
        {
            return _databaseContext.MealProducts.Where(mp => mp.MealId == meal.Id)
                .Select(mp => new ProductDTO
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
                        PortionMultiplier = _databaseContext.CustomizedMealProducts
                            .Where(cmp => cmp.MealProductId == mp.Id)
                            .Select(cmp => cmp.CustomizedPortionMultiplier)
                            .FirstOrDefault()
                    }).ToList();
        }
    }
}
