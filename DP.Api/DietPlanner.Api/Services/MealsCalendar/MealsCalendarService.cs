using DietPlanner.Api.Database;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using DietPlanner.Api.Models.Dto.MealsCalendar;
using DietPlanner.Api.Models;
using DietPlanner.Api.Models.MealsCalendar;

namespace DietPlanner.Api.Services.MealsCalendar
{
    public class MealsCalendarService : IMealsCalendarService
    {
        private readonly ILogger<MealsCalendarService> _logger;
        private readonly DatabaseContext _databaseContext;

        public MealsCalendarService(ILogger<MealsCalendarService> logger, DatabaseContext databaseContext)
        {
            this._logger = logger;
            this._databaseContext = databaseContext;
        }

        public async Task<DailyMealsDTO> GetDailyMeals(DateTime date)
        {
            //TODO: Mock for now. Must be replaced with database data.
            return await Task.FromResult(new DailyMealsDTO
            {
                Breakfast = new SpecifiedMeal()
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
                Dinner = new SpecifiedMeal
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
                Lunch = new SpecifiedMeal()
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
    }
}
