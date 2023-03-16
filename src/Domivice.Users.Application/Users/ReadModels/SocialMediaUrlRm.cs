using Domivice.Users.Domain.Entities;

namespace Domivice.Users.Application.Users.ReadModels;

public class SocialMediaUrlRm
{
    public string Site { get; init; } = string.Empty;
    public string Uri { get; init; } = string.Empty;

    public static explicit operator SocialMediaUrlRm(UserSocialMediaUrl userSocialMediaUrl)
    {
        return new SocialMediaUrlRm
        {
            Site = userSocialMediaUrl.SocialMediaUrl.Site,
            Uri = userSocialMediaUrl.SocialMediaUrl.Uri.ToString()
        };
    }
}