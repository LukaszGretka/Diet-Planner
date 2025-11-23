namespace DietPlanner.Domain.Entities.User
{
    public class CreatedApplicationUser: ApplicationUser
    {
        public required bool RequireEmailConfirmation { get; init; }
    }
}
