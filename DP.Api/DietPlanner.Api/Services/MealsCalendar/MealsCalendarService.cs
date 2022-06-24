using DietPlanner.Api.Database;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using DietPlanner.Api.Models.Dto.MealsCalendar;
using DietPlanner.Api.Models;
using DietPlanner.Api.Models.MealsCalendar;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DietPlanner.Api.Services.MealsCalendar
{
    public class MealsCalendarService : IMealsCalendarService
    {
        private readonly ILogger<MealsCalendarService> _logger;
        private readonly DatabaseContext _databaseContext;

        public MealsCalendarService(ILogger<MealsCalendarService> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public async Task<DailyMealsDTO> GetDailyMeals(DateTime date)
        {
            string formattedDate = date.ToShortDateString();
            var dailyMeals = await _databaseContext.DailyMeals.Where(dailyMeal => dailyMeal.Date.Equals(formattedDate)).FirstOrDefaultAsync();

            //TODO: Mock for now. Must be replaced with database data.
            return await Task.FromResult(new DailyMealsDTO
            {
                Breakfast = new Meal()
                {
                    Products = new Product[]
                    {
                        new Product()
                        {
                            Name = "Chleb",
                            Calories = 300,
                            Carbohydrates = 24.3f,
                            Proteins = 2.4f,
                            Fats = 3,
                            Description = "Razowy",
                            Id = 83,
                            BarCode = 220111487771
                        }
                    }
                },
                Dinner = new Meal
                {
                    Products = new Product[]
                    {
                        new Product()
                        {
                            Name = "Filet z kurczaka",
                            Calories = 200,
                            Carbohydrates = 6.4f,
                            Proteins = 14,
                            Fats = 8,
                            Description = "Marki Rzeźnik",
                            Id = 4
                        },
                        new Product()
                        {
                            Name = "Ryż biały",
                            BarCode = 2000211124551,
                            Calories = 280,
                            Carbohydrates = 41,
                            Fats = 4.3f,
                            Proteins = 8,
                            Description = "Firmy Kupiec"

                        }
                    }
                },
                Lunch = new Meal()
                {
                    Products = new Product[]
                    {
                        new Product 
                        { 
                            Name = "Snickers", 
                            Id = 66, 
                            Calories = 421, 
                            Carbohydrates = 18.1f,
                            Proteins = 6.4f,
                            Fats = 10.1f, 
                            Description = "Batonik karmelowy z masą czekoladą i orzechami",
                            BarCode = 5000159461122,
                            
                        }
                    }
                }
            });
        }

        public async Task<DatabaseActionResult<DailyMeals>> AddMeal(MealByDay mealByDay)
        {
            var meal = new DailyMeals
            {
                Date = mealByDay.Date.ToShortDateString(),
                MealType = new MealType { MealName = mealByDay.MealType.ToString() },
                ProductsId = string.Join(",", mealByDay.Products.ToList().Select(product => product.Id.ToString()))
            };

            try
            {
                await _databaseContext.DailyMeals.AddAsync(meal);
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<DailyMeals>(false, exception: ex);
            }

            return new DatabaseActionResult<DailyMeals>(true, obj: meal);
        }
    }
}
