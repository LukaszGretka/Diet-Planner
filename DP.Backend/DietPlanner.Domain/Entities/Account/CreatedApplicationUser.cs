namespace DietPlanner.Domain.Entities.Account
{
    public class CreatedApplicationUser: ApplicationUser
    {
        public required bool RequireEmailConfirmation { get; init; }
    }
}
