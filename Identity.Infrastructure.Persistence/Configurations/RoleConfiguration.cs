using Identity.Domain.Models.Aggregates.Roles;
using Identity.Domain.Models.Aggregates.Roles.ValueObjects;
using NP.Shared.Domain.Models.SeedWork;
using NP.Shared.Domain.Models.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NP.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(nameof(Role).Pluralize());
            builder.HasKey(x => x.Id);

            builder.Property(p => p.ActivityState)
                .IsRequired(true)
                .HasConversion(p => p.Value, p => Enumeration.FromValue<ActivityState>(p));

            builder.Property(p => p.Title)
                .IsRequired(true)
                .HasMaxLength(RoleTitle.MaxLength)
                .HasConversion(p => p.Title, p => RoleTitle.Create(p));

            builder.Property(p => p.GroupTitle)
                .IsRequired(true)
                .HasMaxLength(RoleGroupTitle.MaxLength)
                .HasConversion(p => p.Title, p => RoleGroupTitle.Create(p));

           

        }
    }
}
