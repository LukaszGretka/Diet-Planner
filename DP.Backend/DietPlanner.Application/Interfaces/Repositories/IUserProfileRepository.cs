using DietPlanner.Domain.Repositories;
using DietPlanner.Domain.Entities;

namespace DietPlanner.Application.Interfaces.Repositories;

public interface IUserProfileRepository : IUserRepository<UserProfile>
{
}
