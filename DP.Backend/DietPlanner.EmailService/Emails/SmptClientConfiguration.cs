namespace DietPlanner.EmailService.Emails
{
    internal class SmptClientConfiguration
    {
        public string SmptHost { get; set; } = null!;

        public int SmptPort { get; set; } = 0!;

        public bool EnableSsl { get; set; } = false!;

        public string SenderEmail { get; set; } = null!;

        public string SenderPassword { get; set; } = null!;
    }
}
