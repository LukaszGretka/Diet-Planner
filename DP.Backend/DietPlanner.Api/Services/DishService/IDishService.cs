using DietPlanner.Api.Database.Models;
using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Api.Models.MealsCalendar.DbModel;
using DietPlanner.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.DishService
{
    public interface IDishService
    {
        Task<Dish> GetById(int id);

        Task<Dish> GetByName(string name);

        Task<DatabaseActionResult<Dish>> Create(CreateDishRequest dish);

        Task<DatabaseActionResult<Dish>> Update(int id, Dish dish);

        Task<DatabaseActionResult<Dish>> DeleteById(int id);
    }
}
