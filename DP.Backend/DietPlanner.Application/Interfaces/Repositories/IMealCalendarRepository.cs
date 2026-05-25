using DietPlanner.Application.Models.MealsCalendar;
using DietPlanner.Domain.Entities;

namespace DietPlanner.Application.Interfaces.Repositories;

public interface IMealCalendarRepository
{
    public Task<List<MealDishDto>> GetMealDishes(Meal meal, CancellationToken ct);

    public Task<List<MealProductDto>> GetMealProducts(Meal meal, CancellationToken ct);
}
