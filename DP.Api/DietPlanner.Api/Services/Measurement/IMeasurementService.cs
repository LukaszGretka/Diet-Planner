using DietPlanner.Api.Models;
using DietPlanner.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services
{
    public interface IMeasurementService
    {
        Task<List<UserMeasurement>> GetAll();

        Task<UserMeasurement> GetById(int id);

        Task<DatabaseActionResult<UserMeasurement>> Create(UserMeasurement product);

        Task<DatabaseActionResult<UserMeasurement>> Update(int id, UserMeasurement product);

        Task<DatabaseActionResult<UserMeasurement>> DeleteById(int id);
    }
}
