﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietPlanner.Api.Models
{
    public class DailyMeals
    {
        public int Id { get; set; }

        public string Date { get; set; }        

        [ForeignKey("MealType")]
        public int MealTypeId { get; set; }
        public virtual MealType MealType { get; set; }

        public string ProductsId { get; set; }

        //[ForeignKey("Product")]
        //public int ProductId { get; set; }
        //public virtual Product Product { get; set; }

        //public int UserId { get; set; } 
    }
}