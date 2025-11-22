using DietPlanner.Application.DTO.Dishes;
using DietPlanner.Domain.Entities.Dishes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DietPlanner.Application.Extensions
{
    internal static class DtoExtensions
    {
        public static DishDTO ToDataTransferObject(this Dish dish, IEnumerable<DishProductsDTO> dishProducts)
        {
            if (dish == null)
            {
                return new DishDTO() { Products = [] };
            }

            return new DishDTO
            {
                Id = dish.Id,
                Name = dish.Name,
                ImagePath = dish.ImagePath,
                Description = dish.Description,
                ExposeToOtherUsers = dish.ExposeToOtherUsers,
                Products = dishProducts
            };
        }

    }
}
