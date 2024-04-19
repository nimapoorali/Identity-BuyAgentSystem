using Identity.Domain.Models.Aggregates.Permissions;
using Identity.Domain.Models.Aggregates.Roles;
using Identity.Domain.Models.Aggregates.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Abstraction
{
    public interface IIdentityDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
        public DbSet<Role>? Roles { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Permission>? Permissions { get; set; }

    }
}
