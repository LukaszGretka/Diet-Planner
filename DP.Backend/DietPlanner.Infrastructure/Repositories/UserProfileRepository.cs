using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Domain.Entities;
using DietPlanner.Infrastructure.Database;

namespace DietPlanner.Infrastructure.Repositories;

public class UserProfileRepository(DietPlannerDbContext dbContext) : GenericUserRepository<UserProfile>(dbContext), IUserProfileRepository
{
}
