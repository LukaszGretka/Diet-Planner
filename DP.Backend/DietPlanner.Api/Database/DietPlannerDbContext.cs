using DietPlanner.Api.Database.Models;
using DietPlanner.Api.Models.MealsCalendar.DbModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DietPlanner.Api.Database
{
    public class DietPlannerDbContext : IdentityDbContext<IdentityUser>
    {
        private readonly IConfiguration _configuration;

        public DietPlannerDbContext(DbContextOptions<DietPlannerDbContext> options, IConfiguration configuration) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlite(_configuration.GetConnectionString("DietPlannerDb"));
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
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CustomizedDishProducts>()
               .Property(mp => mp.CustomizedPortionMultiplier)
            .HasDefaultValue(1.0);

            builder.Entity<Measurement>()
            .HasIndex(u => u.Id)
            .IsUnique();
        }

        public DbSet<UserProfile> UserProfile { get; set; }

        public DbSet<Meal> Meals { get; set; }

        public DbSet<Dish> Dishes { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<MealDish> MealDishes { get; set; }

        public DbSet<MealProduct> MealProducts { get; set; }

        public DbSet<DishProducts> DishProducts { get; set; }

        public DbSet<Measurement> Measurements { get; set; }

        public DbSet<CustomizedDishProducts> CustomizedDishProducts { get; set; }
    }
}
