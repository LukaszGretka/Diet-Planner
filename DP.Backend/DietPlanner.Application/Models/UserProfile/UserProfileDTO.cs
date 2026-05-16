namespace DietPlanner.Application.Models.UserProfile;

public class UserProfileDTO : UserAvatarDTO
{
    public string? Name { get; set; }

    public int Gender { get; set; }

    public DateTime BirthDate { get; set; }

    public int Height { get; set; }
}
