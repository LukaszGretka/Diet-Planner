using DietPlanner.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services
{
    public interface IMeasurementService
    {
        Task<List<Measurement>> GetAll();

        Task<Measurement> GetById(int id);

        Task<DatabaseActionResult<Measurement>> Create(Measurement product);

        Task<DatabaseActionResult<Measurement>> Update(int id, Measurement product);

        Task<DatabaseActionResult<Measurement>> DeleteById(int id);
    }
}
