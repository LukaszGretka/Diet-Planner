namespace DietPlanner.Domain.Entities.User
{
    public class ApplicationUser
    {
        public required string Id { get; init; }

        public required string? UserName { get; init; }

        public required string? Email { get; init; }
    }
}
