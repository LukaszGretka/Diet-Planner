using DietPlanner.Api.Models.BodyProfile.DTO;
using DietPlanner.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services
{
    public interface IMeasurementService
    {
        Task<List<MeasurementDto>> GetAll(string userId);

        Task<MeasurementDto> GetById(int id, string userId);

        Task<DatabaseActionResult<MeasurementDto>> Create(MeasurementDto measurement, string userId);

        Task<DatabaseActionResult<MeasurementDto>> Update(int measurementId, MeasurementDto measurement, string userId);

        Task<DatabaseActionResult<MeasurementDto>> DeleteById(int measurementId, string userId);
    }
}
