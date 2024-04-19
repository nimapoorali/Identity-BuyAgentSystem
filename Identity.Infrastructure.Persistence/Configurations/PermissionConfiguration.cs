using Identity.Domain.Models.Aggregates.Permissions;
using Identity.Domain.Models.Aggregates.Permissions.ValueObjects;
using Identity.Domain.Models.Aggregates.Roles;
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
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable(nameof(Permission).Pluralize());
            builder.HasKey(x => x.Id);

            builder.Property(p => p.ActivityState)
                .IsRequired(true)
                .HasConversion(p => p.Value, p => Enumeration.FromValue<ActivityState>(p));

            builder.Property(p => p.Name)
                .IsRequired(true)
                .HasMaxLength(PermissionName.MaxLength)
                .HasConversion(p => p.Name, p => PermissionName.Create(p));

            builder.OwnsMany(
              p => p.Roles,
              a =>
              {
                  a.WithOwner().HasForeignKey(nameof(PermissionRole.PermissionId));
                  a.ToTable(nameof(PermissionRole).Pluralize());
                  a.HasKey(nameof(PermissionRole.RoleId), nameof(PermissionRole.PermissionId));
                  a.Property(p => p.AssignDate)
                  .IsRequired(true)
                  .HasConversion(p => p.Value, p => DateTimeP.Create(p));
                  a.Property(p => p.ActivityState)
                     .IsRequired(true)
                     .HasConversion(p => p.Value, p => Enumeration.FromValue<ActivityState>(p));
                  a.HasOne<Role>()
                   .WithMany()
                   .HasForeignKey(x => x.RoleId)
                   .IsRequired();
              });
        }
    }
}
