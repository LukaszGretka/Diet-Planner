namespace DietPlanner.Api.Responses
{
    public class DashboardDataResponse
    {
        public float[] CaloriesLastSevenDays { get; set; }

        public float[] CarbsLastSevenDays { get; set; }

        public float[] ProteinsLastSevenDays { get; set; }

        public float[] FatsLastSevenDays { get; set; }

        public float? CurrentWeight { get; set; }

        public int CaloricDemand { get; set; }
    }
}
