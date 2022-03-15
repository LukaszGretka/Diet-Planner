using EatMyFat.Api.Database;
using EatMyFat.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EatMyFat.Api.Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly ILogger<MeasurementService> _logger;
        private readonly DatabaseContext _databaseContext;

        public MeasurementService(ILogger<MeasurementService> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public async Task<DatabaseActionResult<Measurement>> Create(Measurement measurement)
        {
            try
            {
                await _databaseContext.AddAsync(measurement);
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<Measurement>(false, exception: ex);
            }

            return new DatabaseActionResult<Measurement>(true, obj: measurement);
        }

        public async Task<DatabaseActionResult<Measurement>> DeleteById(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<Measurement>> GetAll()
        {
            return await _databaseContext.Measurements.AsNoTracking().ToListAsync();
        }

        public async Task<DatabaseActionResult<Measurement>> Update(int id, Measurement measurement)
        {
            throw new System.NotImplementedException();
        }
    }
}
