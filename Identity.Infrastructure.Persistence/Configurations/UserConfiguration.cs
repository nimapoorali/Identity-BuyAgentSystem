using Identity.Domain.Models.Aggregates.Roles;
using Identity.Domain.Models.Aggregates.Users;
using Identity.Domain.Models.Aggregates.Users.ValueObjects;
using Identity.Domain.Models.SeedWork;
using Identity.Domain.Models.SharedKernel;
using Identity.Resources;
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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User).Pluralize());
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Username).IsUnique();

            builder.Property(p => p.Username)
                .IsRequired(true)
                .HasMaxLength(Username.MaxLength)
                .HasConversion(p => p.Value, p => Username.Create(p));

            builder.Property(p => p.Password)
                .IsRequired(true)
            //.HasMaxLength(Password.MaxLength)
            .HasConversion(p => p.Value, p => Password.Create(p, PasswordStrength.Blank));
            //.HasConversion(p => p.Value, p => Password.Create("***", PasswordStrength.Blank));

            builder.Property(p => p.NickName)
               .IsRequired(false)
               .HasMaxLength(Name.MaxLength)
               .HasConversion(p => p.Value, p => Name.Create(p));
            builder.Property(p => p.FirstName)
               .IsRequired(false)
               .HasMaxLength(Name.MaxLength)
               .HasConversion(p => p.Value, p => Name.Create(p));
            builder.Property(p => p.LastName)
               .IsRequired(false)
               .HasMaxLength(Name.MaxLength)
               .HasConversion(p => p.Value, p => Name.Create(p));

            builder.OwnsOne(
               user => user.Mobile,
               userMobile =>
               {
                   userMobile.Property(mobile => mobile.Value)
                   .IsRequired(false)
                   .HasColumnName(IdentityDataDictionary.Mobile);

                   userMobile.Property(mobile => mobile.IsVerified)
                   .IsRequired(true)
                   .HasColumnName(IdentityDataDictionary.MobileIsVerified);

                   userMobile.Property(mobile => mobile.VerificationKey)
                   .IsRequired(false)
                   .HasColumnName(IdentityDataDictionary.MobileVerificationKey);

                   userMobile.Property(mobile => mobile.KeyExpirationDate)
                   .IsRequired(false)
                   .HasColumnName(IdentityDataDictionary.MobileKeyExpirationDate)
                   .HasConversion(model => model.Value, db => DateTimeP.Create(db));
               });

            builder.OwnsOne(
                user => user.Email,
                userEmail =>
                {
                    userEmail.Property(email => email.Value)
                    .IsRequired(false)
                    .HasColumnName(IdentityDataDictionary.Email);
                    userEmail.Property(email => email.IsVerified)
                    .IsRequired(true)
                    .HasColumnName(IdentityDataDictionary.EmailIsVerified);
                    userEmail.Property(email => email.VerificationKey)
                    .IsRequired(false)
                    .HasColumnName(IdentityDataDictionary.EmailVerificationKey);
                    userEmail.Property(email => email.KeyExpirationDate)
                    .IsRequired(false)
                    .HasColumnName(IdentityDataDictionary.EmailKeyExpirationDate)
                    .HasConversion(model => model.Value, db => DateTimeP.Create(db));
                });

            builder.Property(p => p.ActivityState)
               .IsRequired(true)
               .HasConversion(p => p.Value, p => Enumeration.FromValue<ActivityState>(p));

            builder.OwnsMany(
                p => p.Roles,
                a =>
                {
                    a.WithOwner().HasForeignKey(nameof(UserRole.UserId));
                    a.ToTable(nameof(UserRole).Pluralize());
                    a.HasKey(nameof(UserRole.UserId), nameof(UserRole.RoleId));
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

            //builder.HasMany(x => x.Roles).WithOne().HasForeignKey(x => x.RoleId).IsRequired();
        }
    }
}
