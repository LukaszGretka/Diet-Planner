using EatMyFat.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EatMyFat.Api.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>()
                   .HasIndex(u => u.Id)
                   .IsUnique();
        }

        public DbSet<Product> Products { get; set; }

    }
}
