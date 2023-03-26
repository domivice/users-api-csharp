using Domivice.Domain.ValueObjects;
using FluentResults;

namespace Domivice.Users.Domain.ValueObjects;

public class UserSocialMediaUrlList : ValueObject
{
    public List<SocialMediaUrl> Value { get; }

    private UserSocialMediaUrlList(List<SocialMediaUrl> value)
    {
        Value = value;
    }

    public static Result<UserSocialMediaUrlList> Create(List<SocialMediaUrl> userSocialMediaUrls)
    {
        var duplicates = userSocialMediaUrls
            .GroupBy(x => x.Site)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key)
            .ToList();

        if (duplicates.Any())
        {
            return Result.Fail<UserSocialMediaUrlList>(
                $"Every site url should be added only once. Please check: {string.Join(", ", duplicates)}");
        }

        return new UserSocialMediaUrlList(userSocialMediaUrls);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}