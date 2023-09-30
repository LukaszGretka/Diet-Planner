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
            //.HasMany(meal => meal.MealDishes)
            //.WithOne(mealDish => mealDish.Meal)
            //.HasForeignKey(mealDish => mealDish.MealId)
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
    }
}
