using Newtonsoft.Json;
using System;

namespace EatMyFat.Api.Models
{
    public class Measurement
    {
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
