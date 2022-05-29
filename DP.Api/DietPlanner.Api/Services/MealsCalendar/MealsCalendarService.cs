using DietPlanner.Api.Database;
using DietPlanner.Api.Models.MealsCalendar;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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

        public async Task<DailyMeals> GetDailyMeals(DateTime date)
        {
            //TODO: Mock for now. Must be replaced with database data.
            return await Task.FromResult(new DailyMeals
            {
                Breakfast = new SpecifiedMeal()
                {
                    Products = new Models.Product[]
                    {
                        new Models.Product()
                        {
                            Name = "Chleb"
                        }
                    }
                }
            });
        }
    }
}
