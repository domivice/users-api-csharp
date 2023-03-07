using Domivice.Domain.ValueObjects;

namespace Domivice.Users.Domain.Entities;

public sealed class UserSocialMediaUrl : BaseEntity<Guid>
{
    public Guid UserId { get; }
    public User User { get; }
    public SocialMediaUrl SocialMediaUrl { get; }
}