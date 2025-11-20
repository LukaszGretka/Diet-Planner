using BenchmarkDotNet.Attributes;
using DietPlanner.Api.Database;
using DietPlanner.Api.Database.Models;
using DietPlanner.Api.Database.Repository;
using DietPlanner.Api.Enums;
using DietPlanner.Api.Models.MealsCalendar.DTO;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DietPlanner.Api.Benchmarks
{
    [MemoryDiagnoser]
    public class EfBenchmark
    {
        private DietPlannerDbContext? _dbContext;
        private readonly string userId = "77ad6a80-d92a-4a10-bf52-c834ddfef297";

        [GlobalSetup]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DietPlannerDbContext>()
                .UseInMemoryDatabase("DietPlannerDb")
                .Options;

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _dbContext = new DietPlannerDbContext(options, configuration);
        }

        [Benchmark]
        public async Task<List<Meal>> GetMeals()
        {
            return await _dbContext!.Meals.Where(x => x.UserId == userId
                                                && x.Date == new DateTime(2025, 02, 25)).ToListAsync();
        }

        [Benchmark]
        public async Task<List<Meal>> GetMealsWithoutDate()
        {
            return await _dbContext!.Meals.Where(x => x.UserId == userId).ToListAsync();
        }

        [Benchmark]
        public async Task<List<MealDto>> MealServiceBenchmark()
        {
            var meals = await _dbContext!.Meals.Where(x => x.UserId == userId
                               && x.Date == new DateTime(2025, 02, 25)).ToListAsync();

            var repository = new MealCalendarRepository(_dbContext);


            List<MealDto> mealDtos = [.. meals
                .GroupBy(m => new { m.MealType })
                .Select(g => new MealDto
                {
                    MealType = (MealType)g.Key.MealType,
                    Products = [.. g.SelectMany(meal => repository.GetMealProducts(meal, CancellationToken.None).Result)],
                    Dishes = [.. g.SelectMany(meal => repository.GetMealDishes(meal, CancellationToken.None).Result)]
                })];

            return mealDtos;
        }
    }
}