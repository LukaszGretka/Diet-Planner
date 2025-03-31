using DietPlanner.Api.Database.Models;
using DietPlanner.Api.Models.MealsCalendar.DbModel;
using Microsoft.EntityFrameworkCore;
using System;

namespace DietPlanner.Api.Benchmarks
{
    public class PlannerDbContext : DbContext
    {
        public DbSet<Meal> Meals { get; set; }

        public DbSet<MealDish> MealDishes { get; set; }

        public DbSet<Dish> Dishes { get; set; }

        public DbSet<DishProducts> DishProducts { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<CustomizedDishProducts> CustomizedDishProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            throw new NotImplementedException();
            //var dbPath = ""; // Path to the database
            //optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
