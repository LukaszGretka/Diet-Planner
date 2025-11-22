namespace DietPlanner.Application.DTO.Dashboard
{
    public class DashboardData
    {
        public required float[] CaloriesLastSevenDays { get; set; }

        public required float[] CarbsLastSevenDays { get; set; }

        public required float[] ProteinsLastSevenDays { get; set; }

        public required float[] FatsLastSevenDays { get; set; }

        public required float? CurrentWeight { get; set; }

        public int CaloricDemand { get; set; }
    }
}
