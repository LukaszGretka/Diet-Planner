using DietPlanner.Application.Interfaces.Repository;
using DietPlanner.Domain.Entities;
using DietPlanner.Infrastructure.Database;

namespace DietPlanner.Infrastructure.Repositories
{
    public class UserProfileRepository : GenericUserRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(DietPlannerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
