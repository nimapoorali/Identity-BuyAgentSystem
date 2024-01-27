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
    public class ActivityStateConfiguration : IEntityTypeConfiguration<ActivityState>
    {
        public void Configure(EntityTypeBuilder<ActivityState> builder)
        {
            builder.ToTable(nameof(ActivityState).Pluralize());
            builder.HasKey(x => x.Value);
            builder.Property(p => p.Value)
                .ValueGeneratedNever()
                .IsRequired(true);

            builder.Property(p => p.Name)
                .HasMaxLength(ActivityState.NameMaxLength);

            builder.HasData(
                ActivityState.Active,
                ActivityState.Deactive,
                ActivityState.Suspend);
        }
    }
}
