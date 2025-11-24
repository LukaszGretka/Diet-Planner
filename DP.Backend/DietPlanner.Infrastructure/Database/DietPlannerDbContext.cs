using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DietPlanner.Domain.Entities;

namespace DietPlanner.Infrastructure.Database
{
    public class DietPlannerDbContext(DbContextOptions<DietPlannerDbContext> options, IConfiguration configuration) : IdentityDbContext<IdentityUser>(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlite(configuration.GetConnectionString("DietPlannerDb"));
            }

            builder.EnableSensitiveDataLogging();
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserProfile>()
               .HasIndex(u => u.UserId)
               .IsUnique();

            builder.Entity<Meal>()
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

            builder.Entity<DishProduct>()
                   .HasIndex(u => u.Id)
                   .IsUnique();

            builder.Entity<DishProduct>()
                   .Property(mp => mp.PortionMultiplier)
                   .HasDefaultValue(1.0);

            builder.Entity<CustomizedMealDish>()
                .HasOne(c => c.MealDish)
                .WithMany()
                .HasForeignKey(c => c.MealDishId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Goals>()
                .HasIndex(u => u.UserId)
                .IsUnique();

            builder.Entity<CustomizedMealDish>()
               .Property(mp => mp.CustomizedPortionMultiplier)
            .HasDefaultValue(1.0);

            builder.Entity<UserMeasurement>()
            .HasIndex(u => u.Id)
            .IsUnique();
        }

        public DbSet<UserProfile> UserProfile { get; set; }

        public DbSet<Meal> Meals { get; set; }

        public DbSet<Dish> Dishes { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<MealDish> MealDishes { get; set; }

        public DbSet<MealProduct> MealProducts { get; set; }

        public DbSet<DishProduct> DishProducts { get; set; }

        public DbSet<UserMeasurement> Measurements { get; set; }

        public DbSet<CustomizedMealDish> CustomizedMealDishes { get; set; }

        public DbSet<CustomizedMealProducts> CustomizedMealProducts { get; set; }

        public DbSet<Goals> Goals { get; set; }
    }
}
