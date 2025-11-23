using DietPlanner.Domain.Entities.User;

namespace DietPlanner.Application.Models.Account
{
    public class SignUpResult : BaseResult
    {
        public required CreatedApplicationUser? CreatedUser { get; set; }
    }
}
