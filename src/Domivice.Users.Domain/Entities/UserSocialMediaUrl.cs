using Domivice.Domain.ValueObjects;
using FluentResults;

namespace Domivice.Users.Domain.Entities;

public class UserSocialMediaUrl : BaseEntity<Guid>
{
    private UserSocialMediaUrl(Guid userId, SocialMediaUrl socialMediaUrl)
    {
        UserId = userId;
        SocialMediaUrl = socialMediaUrl;
    }

    private UserSocialMediaUrl(SocialMediaUrl socialMediaUrl)
    {
        SocialMediaUrl = socialMediaUrl;
    }

    public static Result<UserSocialMediaUrl> Create(SocialMediaUrl socialMediaUrl)
    {
        return new UserSocialMediaUrl(socialMediaUrl);
    }

    public Guid UserId { get; }
    public virtual User User { get; }
    public SocialMediaUrl SocialMediaUrl { get; }
}