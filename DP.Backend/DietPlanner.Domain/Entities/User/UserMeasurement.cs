using Microsoft.EntityFrameworkCore;

namespace DietPlanner.Domain.Entities.User
{
    public class Measurement : BaseEntity
    {
        [Precision(5, 2)]
        public string Date { get; set; }

        [Precision(5, 2)]
        public decimal Weight { get; set; }

        [Precision(5, 2)]
        public decimal Chest { get; set; }

        [Precision(5 ,2)]
        public decimal Belly { get; set; }

        [Precision(5, 2)]
        public decimal Waist { get; set; }

        [Precision(5, 2)]
        public decimal BicepsRight { get; set; }

        [Precision(5, 2)]
        public decimal BicepsLeft { get; set; }

        [Precision(5, 2)]
        public decimal ForearmRight { get; set; }

        [Precision(5, 2)]
        public decimal ForearmLeft { get; set; }

        [Precision(5, 2)]
        public decimal ThighRight { get; set; }

        [Precision(5, 2)]
        public decimal ThighLeft { get; set; }

        [Precision(5, 2)]
        public decimal CalfRight { get; set; }

        [Precision(5, 2)]
        public decimal CalfLeft { get; set; }

        [Precision(5, 2)]
        public string UserId { get; set; }
    }
}
