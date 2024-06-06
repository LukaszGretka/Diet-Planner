namespace DietPlanner.Api.Models.Dashboard
{
    public class DashboardData
    {
        public float[] CaloriesLastSevenDays { get; set; }

        public float[] CarbsLastSevenDays { get; set; }

        public float[] ProteinsLastSevenDays { get; set; }

        public float[] FatsLastSevenDays { get; set; }

        public float? CurrentWeight { get; set; }
    }
}
