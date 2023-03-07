using Domivice.Domain.ValueObjects;
using Domivice.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domivice.Users.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.FirstName).HasConversion(
            v => v.Value,
            v => FirstName.Create(v).Value);
        builder.Property(u => u.LastName).HasConversion(
            v => v.Value,
            v => LastName.Create(v).Value);
        builder.Property(u => u.Email).HasConversion(
            v => v.Value,
            v => Email.Create(v).Value);
        builder.OwnsOne(u => u.PhoneNumber)
            .Property(p => p.Number).HasColumnName("PhoneNumber");
        builder.OwnsOne(uv => uv.PhoneNumber)
            .Property(p => p.CountryCode).HasColumnName("PhoneCountryCode");
        builder.Property(u => u.DisplayLanguage).HasConversion(
            v => v.Value,
            v => CultureCode.Create(v).Value);
        builder.Property(u => u.UserBio).HasConversion(
            v => v.Value,
            v => Text.Create(v).Value);
        builder.OwnsOne(u => u.HomeAddress, ha => ha.ToTable("HomeAddresses"));
        builder.Property(u => u.Website).HasConversion(
            v => v.ToString(),
            v => new Uri(v));
        builder.Property(u => u.EntryInstructions).HasConversion(
            v => v.Value,
            v => Text.Create(v).Value);
        builder.HasMany(u => u.UserLanguages)
            .WithOne(ul => ul.User)
            .HasForeignKey(ul => ul.UserId)
            .IsRequired();
        builder.HasMany(u => u.UserSocialMediaUrls)
            .WithOne(smUrl => smUrl.User)
            .HasForeignKey(smUrl => smUrl.UserId)
            .IsRequired();
    }
}