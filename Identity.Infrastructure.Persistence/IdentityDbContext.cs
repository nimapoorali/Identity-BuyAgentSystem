using Identity.Application.Abstraction;
using Identity.Domain.Models.Aggregates.Permissions;
using Identity.Domain.Models.Aggregates.Roles;
using Identity.Domain.Models.Aggregates.Users;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence
{
    ///////////////migration command
    //add-migration initializeDatabase -context IdentityDbContext -outputdir Migrations -startupproject Microservices\IdentityService\Identity.Api
    //update-database -context IdentityDbContext -startupproject Microservices\IdentityService\Identity.Api
    //Remove-Migration -startupproject Microservices\IdentityService\Identity.Api
    ////////////////////////////////

    public class IdentityDbContext : DbContext, IIdentityDbContext
    {
        public DbSet<Role>? Roles { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Permission>? Permissions { get; set; }

        public IdentityDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //new Configurations.RoleConfiguration().Configure(modelBuilder.Entity<Role>());
            //modelBuilder.ApplyConfiguration(new Configurations.RoleConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);


            base.OnModelCreating(modelBuilder);
        }
    }
}
