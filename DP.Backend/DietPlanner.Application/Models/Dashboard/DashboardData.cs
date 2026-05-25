namespace DietPlanner.Application.Models.Dashboard;

public class DashboardData
{
    public required float[] CaloriesLastSevenDays { get; init; }
    public required float[] CarbsLastSevenDays { get; init; }
    public required float[] ProteinsLastSevenDays { get; init; }
    public required float[] FatsLastSevenDays { get; init; }
    public float? CurrentWeight { get; init; }
    public int? CaloricDemand { get; init; }
}
