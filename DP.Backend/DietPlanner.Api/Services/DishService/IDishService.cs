using DietPlanner.Api.Database.Models;
using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.DishService
{
    public interface IDishService
    {
        Task<Dish> GetById(int id);

        Task<List<Dish>> GetAllUserDishes(string userId);

        Task<List<DishProducts>> GetDishProducts(int dishId);

        Task<DatabaseActionResult<Dish>> Create(CreateDishRequest dish, string userId);

        Task<DatabaseActionResult<Dish>> Update(int id, Dish dish);

        Task<DatabaseActionResult<Dish>> DeleteById(int id);
    }
}
