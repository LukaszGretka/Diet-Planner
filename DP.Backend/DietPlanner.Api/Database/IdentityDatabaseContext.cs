using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DietPlanner.Api.Database
{
    public class IdentityDatabaseContext : IdentityDbContext<IdentityUser>
    {
        private readonly IConfiguration _configuration;

        public IdentityDatabaseContext(DbContextOptions options,
            IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(_configuration.GetConnectionString("IdentityDatabase"));
            }

            builder.EnableSensitiveDataLogging();
            base.OnConfiguring(builder);
        }
    }
}
