using BenchmarkDotNet.Attributes;
using DietPlanner.Api.Database.Models;
using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Api.Models.MealsCalendar.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DietPlanner.Api.Benchmarks
{
    [MemoryDiagnoser]
    public class EfBenchmark
    {
        private PlannerDbContext _dbContext;

        [GlobalSetup]
        public async Task Setup()
        {
            var dbConnectionFactory = new SqliteConnectionFactory("Data Source=../DietPlannerDb.db");
            _dbContext = new();

        }

        [Benchmark]
        public async Task<List<Meal>> GetMeals()
        {
            return await _dbContext.Meals.Where(x => x.UserId == "77ad6a80-d92a-4a10-bf52-c834ddfef297" 
                                                && x.Date == new DateTime(2025, 02, 25)).ToListAsync();
        }

        [Benchmark]
        public async Task<List<Meal>> GetMealsWithoutDate()
        {
            return await _dbContext.Meals.Where(x => x.UserId == "77ad6a80-d92a-4a10-bf52-c834ddfef297").ToListAsync();
        }

        [Benchmark]
        public async Task<List<MealDto>> MealServiceBenchmark()
        {
            var meals = await _dbContext.Meals.Where(x => x.UserId == "77ad6a80-d92a-4a10-bf52-c834ddfef297"
                                                && x.Date == new DateTime(2025, 02, 25)).ToListAsync();

            var mealDishes = await _dbContext.MealDishes
                .Where(md => meals.Select(m => m.Id).Contains(md.MealId))
                .ToListAsync();

            var dishes = await _dbContext.Dishes
                .Where(d => mealDishes.Select(md => md.DishId).Contains(d.Id))
                .ToListAsync();

            var dishProducts = await _dbContext.DishProducts
                .Where(dp => dishes.Select(d => d.Id).Contains(dp.DishId))
                .ToListAsync();

            var products = await _dbContext.Products
                .Where(p => dishProducts.Select(dp => dp.ProductId).Contains(p.Id))
                .ToListAsync();

            var customizedDishProducts = await _dbContext.CustomizedDishProducts
                 .Where(cdp => mealDishes.Select(md => md.Id).Contains(cdp.MealDishId))
                 .ToListAsync();

            var mealDtos = meals
                     .GroupBy(m => new { m.MealType })
                     .Select(g => new MealDto
                     {
                         MealTypeId = (MealTypeEnum)g.Key.MealType,
                         Dishes = g.SelectMany(m => mealDishes.Where(md => md.MealId == m.Id)
                             .Select(md => new DishDTO
                             {
                                 Id = md.DishId,
                                 MealDishId = md.Id,
                                 Name = dishes.First(d => d.Id == md.DishId).Name,
                                 Description = dishes.First(d => d.Id == md.DishId).Description,
                                 ImagePath = dishes.First(d => d.Id == md.DishId).ImagePath,
                                 ExposeToOtherUsers = dishes.First(d => d.Id == md.DishId).ExposeToOtherUsers,
                                 Products = dishProducts.Where(dp => dp.DishId == md.DishId)
                                     .Select(dp => new DishProductsDTO
                                     {
                                         Product = products.First(p => p.Id == dp.ProductId),
                                         PortionMultiplier = dp.PortionMultiplier,
                                         CustomizedPortionMultiplier = customizedDishProducts
                                             .Where(cdp => cdp.MealDishId == md.Id && cdp.DishProductId == dp.Id)
                                             .Select(cdp => (decimal?)cdp.CustomizedPortionMultiplier)
                                             .FirstOrDefault()
                                     }).ToList()
                             })).ToList()
                     }).ToList();

            return mealDtos;
        }
    }
}