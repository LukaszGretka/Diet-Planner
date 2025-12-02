using DietPlanner.Application.Interfaces.Common;
using DietPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DietPlanner.Application.Interfaces.Repository
{
    public interface IUserProfileRepository: IGenericUserRepository<UserProfile>
    {
    }
}
