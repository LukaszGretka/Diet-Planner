using EatMyFat.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EatMyFat.Api.Services
{
    public interface IMeasurementService
    {
        Task<List<Measurement>> GetAll();

        Task<DatabaseActionResult<Measurement>> Create(Measurement product);

        Task<DatabaseActionResult<Measurement>> Update(int id, Measurement product);

        Task<DatabaseActionResult<Measurement>> DeleteById(int id);
    }
}
