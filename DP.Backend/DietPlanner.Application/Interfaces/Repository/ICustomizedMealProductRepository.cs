namespace DietPlanner.Application.Interfaces.Repository
{
    public interface ICustomizedMealProductRepository
    {
        Task<decimal?> GetPortionMultiplierAsync(int mealProductId, CancellationToken ct);
    }
}
