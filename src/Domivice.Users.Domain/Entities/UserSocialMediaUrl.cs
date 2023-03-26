using Domivice.Domain.ValueObjects;
using FluentResults;

namespace Domivice.Users.Domain.Entities;

public class UserSocialMediaUrl : BaseEntity<Guid>
{
    private UserSocialMediaUrl()
    {
        
    }
    private UserSocialMediaUrl(Guid userId, SocialMediaUrl socialMediaUrl)
    {
        UserId = userId;
        SocialMediaUrl = socialMediaUrl;
    }
    public Guid UserId { get; }
    public virtual User User { get; }
    public SocialMediaUrl SocialMediaUrl { get; }

    public static Result<UserSocialMediaUrl> Create(SocialMediaUrl socialMediaUrl)
    {
        return new UserSocialMediaUrl(default, socialMediaUrl);
    }
}