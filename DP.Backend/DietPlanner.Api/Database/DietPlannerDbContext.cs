using DietPlanner.Api.Database.Models;
using DietPlanner.Api.Models.MealsCalendar.DbModel;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DietPlanner.Api.Database
{
    public class DietPlannerDbContext : DbContext
    {
        public DietPlannerDbContext(DbContextOptions<DietPlannerDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Meal>()
            .HasIndex(u => u.Id)
            .IsUnique();

            builder.Entity<Dish>()
                   .HasIndex(u => u.Id)
                   .IsUnique();


            builder.Entity<Product>()
                   .HasIndex(u => u.Id)
                   .IsUnique();

            builder.Entity<MealDish>()
                   .HasIndex(u => u.Id)
                   .IsUnique();

            builder.Entity<Dish>()
                   .HasIndex(u => u.Id)
                   .IsUnique();

            builder.Entity<DishProducts>()
                   .HasIndex(u => u.Id)
                   .IsUnique();

            builder.Entity<DishProducts>()
                   .Property(mp => mp.PortionMultiplier)
                   .HasDefaultValue(1.0);

            builder.Entity<CustomizedDishProducts>()
            .HasOne(c => c.MealDish)
            .WithMany()
            .HasForeignKey(c => c.MealDishId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CustomizedDishProducts>()
               .Property(mp => mp.CustomizedPortionMultiplier)
            .HasDefaultValue(1.0);

            builder.Entity<Measurement>()
            .HasIndex(u => u.Id)
            .IsUnique();

        }

        public DbSet<Meal> Meals { get; set; }

        public DbSet<Dish> Dishes { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<MealDish> MealDishes { get; set; }

        public DbSet<DishProducts> DishProducts { get; set; }

        public DbSet<Measurement> Measurements { get; set; }

        public DbSet<CustomizedDishProducts> CustomizedDishProducts { get; set; }
    }
}
