namespace DietPlanner.Domain.Entities.Account
{
    public class ApplicationUser
    {
        public required string Id { get; init; }

        public required string? UserName { get; init; }

        public required string? Email { get; init; }
    }
}
