using DietPlanner.Application.DTO;

namespace DietPlanner.Application.Interfaces
{
    public interface IMealService
    {
        Task<DatabaseActionResult<List<MealDto>>> GetMeals(DateTime date, string userId, CancellationToken ct);

        Task<DatabaseActionResult<List<MealDto>>> AddMealItem(MealItemRequest addMealItemRequest, string userId, CancellationToken ct);

        Task<DatabaseActionResult<List<MealDto>>> RemoveMealItem(MealItemRequest removeMealItemRequest, string userId, CancellationToken ct);

        Task<DatabaseActionResult<List<MealDto>>> UpdateMealItemPortion(UpdateMealItemPortionRequest request, string userId, CancellationToken ct);
    }
}
