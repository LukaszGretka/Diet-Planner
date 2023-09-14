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

        public async Task<List<Dish>> GetAllUserDishes(string userId)
        {
            return await _databaseContext.Dishes.Where(dish => dish.UserId.Equals(userId)).ToListAsync();
        }

        public async Task<Dish> GetById(int id)
        {
             return await _databaseContext.Dishes.FirstOrDefaultAsync(dish => dish.Id == id);
        }

        public async Task<DatabaseActionResult<Dish>> Create(CreateDishRequest request, string userId)
        {
            if(!request.Products.Any())
            {
                return new DatabaseActionResult<Dish>(false, message: "No product were added to the dish");
            }

            var dishProducts = new List<DishProducts>();

            try
            {
                var dishResult = await _databaseContext.Dishes.AddAsync(new Dish
                {
                    Name = request.Name,
                    ImagePath = request.Image,
                    Description = request.Description,
                    UserId = userId,
                    ExposeToOtherUsers = request.ExposeToOtherUsers
                });

                request.Products.ToList().ForEach(dishProduct =>
                {
                    dishProducts.Add(new DishProducts
                    {
                        Product = dishProduct.Product,
                        PortionMultiplier = dishProduct.PortionMultiplier,
                        Dish = dishResult.Entity,
                    });
                });

                _databaseContext.DishProducts.AttachRange(dishProducts);

                await _databaseContext.SaveChangesAsync();

                return new DatabaseActionResult<Dish>(true, obj: dishResult.Entity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<Dish>(false, exception: ex);
            }
        }

        public async Task<List<DishProducts>> GetDishProducts(int dishId)
        {
            return await _databaseContext.DishProducts
                .Where(dish => dish.DishId == dishId)
                .Select(x => new DishProducts() { 
                    Product = x.Product,
                    PortionMultiplier= x.PortionMultiplier,
            }).ToListAsync();
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
