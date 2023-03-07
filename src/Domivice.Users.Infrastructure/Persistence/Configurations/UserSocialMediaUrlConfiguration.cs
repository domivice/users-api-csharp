using Domivice.Domain.ValueObjects;
using Domivice.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domivice.Users.Infrastructure.Persistence.Configurations;

public class UserSocialMediaUrlConfiguration : IEntityTypeConfiguration<UserSocialMediaUrl>
{
    public void Configure(EntityTypeBuilder<UserSocialMediaUrl> builder)
    {
        builder.ToTable("UserSocialMediaUrls");
        builder.HasKey(ul => ul.Id);
        builder.OwnsOne(ul => ul.SocialMediaUrl)
            .Property(p => p.Uri)
            .HasColumnName("Url")
            .HasConversion(
                v => v.ToString(),
                v => new Uri(v));
        builder.OwnsOne(ul => ul.SocialMediaUrl)
            .Property(p => p.Site)
            .HasColumnName("Site")
            .HasConversion(
                v => v.Value,
                v => SocialMediaSite.Create(v).Value);
    }
}