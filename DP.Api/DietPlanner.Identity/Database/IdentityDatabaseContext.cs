using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DietPlanner.Identity.Database
{
    public class IdentityDatabaseContext : ApiAuthorizationDbContext<IdentityUser>
    {
        private readonly IConfiguration _configuration;

        public IdentityDatabaseContext(DbContextOptions options, 
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            IConfiguration configuration) : base(options, operationalStoreOptions)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlite(_configuration.GetConnectionString("IdentityDatabase"));
            }

            builder.EnableSensitiveDataLogging();
            base.OnConfiguring(builder);
        }

    }
}
