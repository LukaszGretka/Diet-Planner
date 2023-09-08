using DietPlanner.Api.Database.Models;
using DietPlanner.Api.Models.MealsCalendar.DbModel;
using Microsoft.EntityFrameworkCore;

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
            .HasMany(e => e.Dishes)
            .WithOne(e => e.Meal)
            .HasForeignKey(e => e.MealId)
            .IsRequired();

            builder.Entity<Dish>()
                .HasMany(e => e.DishProducts)
                .WithOne(e => e.Dish)
                .HasForeignKey(e => e.DishId)
                .IsRequired();


            builder.Entity<Product>()
                   .HasIndex(u => u.Id)
                   .IsUnique();

            builder.Entity<MealDishes>()
                   .HasIndex(u => u.Id)
                   .IsUnique();

            builder.Entity<Dish>()
                   .HasIndex(u => u.Id)
                   .IsUnique();

            builder.Entity<DishProducts>()
                   .Property(mp => mp.PortionMultiplier)
                   .HasDefaultValue(1.0);

            builder.Entity<Measurement>()
            .HasIndex(u => u.Id)
            .IsUnique();

        }

        public DbSet<Meal> Meals { get; set; }

        public DbSet<Dish> Dishes { get; set; }

        public DbSet<Product> Products { get; set; }

        //public DbSet<MealDishes> MealDishes { get; set; }

        //public DbSet<DishProducts> DishProducts { get; set; }

        public DbSet<Measurement> Measurements { get; set; }
    }
}
