using System;
using System.Collections.Generic;
using DietPlanner.Domain.Enums;

namespace DietPlanner.Api.DTO.Dishes
{
    public class DishDTO : BaseItem
    {
        public DishDTO()
        {
            ItemType = ItemType.Dish;
        }

        public bool ExposeToOtherUsers { get; set; }

        // This property is used to determine if the dish was created by currenly signed in user
        public bool IsOwner { get; set; }

        public IEnumerable<DishProductsDTO> Products { get; set; }
    }
}
