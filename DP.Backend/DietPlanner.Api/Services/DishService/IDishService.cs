using DietPlanner.Api.Database.Models;
using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.DishService
{
    public interface IDishService
    {
        Task<bool> CheckIfExists(int id);

        Task<Dish> GetById(int id);

        Task<List<Dish>> GetAllUserDishes(string userId);

        Task<IEnumerable<DishProducts>> GetDishProducts(int dishId);

        Task<DatabaseActionResult<DishDTO>> Create(PutDishRequest dish, string userId);

        Task<DatabaseActionResult> Update(PutDishRequest dish, string userId);

        Task<DatabaseActionResult> DeleteById(int id, string userId);
    }
}
