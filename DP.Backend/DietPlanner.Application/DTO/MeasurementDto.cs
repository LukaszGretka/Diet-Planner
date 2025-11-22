using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Application.DTO
{
    public class MeasurementDto
    {
        public int Id { get; set; }

        public required string Date { get; set; }

        [Range(0, 500)]
        public decimal Weight { get; set; }

        [Range(0, 500)]
        public decimal Chest { get; set; }

        [Range(0, 500)]
        public decimal Belly { get; set; }

        [Range(0, 500)]
        public decimal Waist { get; set; }

        [Range(0, 100)]
        public decimal BicepsRight { get; set; }

        [Range(0, 100)]
        public decimal BicepsLeft { get; set; }

        [Range(0, 100)]
        public decimal ForearmRight { get; set; }

        [Range(0, 100)]
        public decimal ForearmLeft { get; set; }

        [Range(0, 100)]
        public decimal ThighRight { get; set; }

        [Range(0, 100)]
        public decimal ThighLeft { get; set; }

        [Range(0, 100)]
        public decimal CalfRight { get; set; }

        [Range(0, 100)]
        public decimal CalfLeft { get; set; }
    }
}
