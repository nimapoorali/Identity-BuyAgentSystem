using Identity.Domain.Models.Aggregates.Roles;
using Identity.Domain.Models.Aggregates.Users;
using Identity.Domain.Models.Aggregates.Users.ValueObjects;
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
    public class UserRoleConfiguration //: IEntityTypeConfiguration<UserRole>
    {
        //public void Configure(EntityTypeBuilder<UserRole> builder)
        //{
        //    builder.ToTable(nameof(UserRole).Pluralize());
        //    builder.HasKey(nameof(UserRole.UserId), nameof(UserRole.RoleId));
        //    builder.Property(p => p.AssignDate)
        //                .IsRequired(true)
        //                .HasConversion(p => p.Value, p => AssignDateTime.Create(p).Value);
        //    builder.Property(p => p.ActivityState)
        //       .IsRequired(true)
        //       .HasConversion(p => p.Value, p => Enumeration.FromValue<ActivityState>(p));
        //    builder
        //        .HasOne<User>()
        //        .WithMany(x => x.Roles)
        //        .HasForeignKey(x => x.UserId)
        //        .IsRequired();
        //    builder
        //        .HasOne<Role>()
        //        .WithMany()
        //        .HasForeignKey(x => x.RoleId)
        //        .IsRequired();
        //    builder
        //        .HasIndex(x => new { x.UserId, x.RoleId });
        //}
    }
}
