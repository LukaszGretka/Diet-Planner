using DietPlanner.Api.Database;
using DietPlanner.Api.Database.Models;
using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.DishService
{
    public class DishService : IDishService
    {
        private readonly ILogger<DishService> _logger;
        private readonly DietPlannerDbContext _databaseContext;

        public DishService(ILogger<DishService> logger, DietPlannerDbContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public async Task<Dish> GetById(int id)
        {
             return await _databaseContext.Dishes.FirstOrDefaultAsync(dish => dish.Id == id);
        }

        public async Task<Dish> GetByName(string name)
        {
            return await _databaseContext.Dishes.FirstOrDefaultAsync(dish => dish.Name.Equals(name));
        }

        public async Task<DatabaseActionResult<Dish>> Create(CreateDishRequest request)
        {
            var dishProducts = new List<DishProducts>();

            request.Products.ToList().ForEach(dishProduct =>
            {
                dishProducts.Add(new DishProducts
                {
                    Product = dishProduct.Product,
                    PortionMultiplier = dishProduct.PortionMultiplier
                });
            });

            try
            {
                var dishToAdd = new Dish
                {
                    Name = request.Name,
                    ImagePath = request.Image,
                };

                await _databaseContext.Dishes.AddAsync(dishToAdd);
                await _databaseContext.DishProducts.AddRangeAsync(dishProducts);

                await _databaseContext.SaveChangesAsync();

                return new DatabaseActionResult<Dish>(true);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<Dish>(false, exception: ex);
            }
        }

        public async Task<DatabaseActionResult<Dish>> DeleteById(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<DatabaseActionResult<Dish>> Update(int id, Dish dish)
        {
            throw new System.NotImplementedException();
        }
    }
}
