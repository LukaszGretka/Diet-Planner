using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Api.Models
{
    public class UserMeasurement : Measurement
    {
        public string UserId { get; set; }
    }

    public class Measurement
    {
        [Key]
        public int Id { get; set; }

        public string Date { get; set; }

        public decimal Weight { get; set; }

        public decimal Chest { get; set; }

        public decimal Belly { get; set; }

        public decimal Waist { get; set; }

        public decimal BicepsRight { get; set; }

        public decimal BicepsLeft { get; set; }

        public decimal ForearmRight { get; set; }

        public decimal ForearmLeft { get; set; }

        public decimal ThighRight { get; set; }

        public decimal ThighLeft { get; set; }

        public decimal CalfRight { get; set; }

        public decimal CalfLeft { get; set; }
    }
}
