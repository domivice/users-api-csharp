using Domivice.Domain.ValueObjects;
using Domivice.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domivice.Users.Infrastructure.Persistence.Configurations;

public class UserLanguageConfiguration : IEntityTypeConfiguration<UserLanguage>
{
    public void Configure(EntityTypeBuilder<UserLanguage> builder)
    {
        builder.ToTable("UserLanguages");
        builder.HasKey(ul => ul.Id);
        builder.Property(ul => ul.LanguageCode).HasConversion(
            v => v.Value,
            v => LanguageCode.Create(v).Value);
    }
}