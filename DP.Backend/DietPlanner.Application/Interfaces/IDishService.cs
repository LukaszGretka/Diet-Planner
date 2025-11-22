using DietPlanner.Application.DTO.Dishes;
using DietPlanner.Domain.Entities.Dishes;

namespace DietPlanner.Application.Interfaces
{
    public interface IDishService
    {
        Task<bool> CheckIfExists(int id);

        Task<Dish?> GetById(int id);

        Task<Dish?> GetByName(string name);

        Task<List<Dish>> GetAllUserDishes(string userId);

        Task<List<Dish>> GetAllAvailableDishes(string userId);

        Task<IEnumerable<DishProducts>> GetDishProducts(int dishId);

        Task<DishDTO> Create(DishDTO dish, string userId);

        void Update(DishDTO dish, string userId);

        void DeleteById(int id, string userId);
    }
}
