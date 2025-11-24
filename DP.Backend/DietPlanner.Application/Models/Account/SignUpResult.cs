using DietPlanner.Domain.Entities.Account;
using DietPlanner.Domain.Entities.Results;

namespace DietPlanner.Application.Models.Account
{
    public class SignUpResult : BaseResult
    {
        public required CreatedApplicationUser? CreatedUser { get; set; }
    }
}
