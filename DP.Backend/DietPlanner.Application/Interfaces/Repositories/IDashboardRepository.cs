using DietPlanner.Application.DTO;

namespace DietPlanner.Application.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<List<DatedDishProductsDto>> GetDatedDishProducts(string userId, DateTime dataTimeNow);
    }
}
