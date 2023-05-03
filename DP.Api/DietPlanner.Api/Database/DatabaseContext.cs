using DietPlanner.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DietPlanner.Api.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserMeal>()
            .HasOne(m => m.MealType)
            .WithOne(m => m.Meal)
            .HasForeignKey<MealType>(m => m.Id);

            builder.Entity<MealType>()
                   .HasIndex(m => m.Id)
                   .IsUnique();

            builder.Entity<UserProduct>()
                     .HasIndex(u => u.Id)
                     .IsUnique();

            builder.Entity<MealProduct>()
                     .HasIndex(u => u.Id)
                     .IsUnique();

            builder.Entity<UserMeasurement>()
               .HasIndex(u => u.Id)
               .IsUnique();
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<UserMeal> UserMeals { get; set; }

        public DbSet<MealType> MealTypes { get; set; }

        public DbSet<MealProduct> MealProducts { get; set; }

        public DbSet<UserMeasurement> UserMeasurements { get; set; }
    }
}
