using Domivice.Domain.ValueObjects;
using FluentResults;

namespace Domivice.Users.Domain.ValueObjects;

public class UserLanguageList : ValueObject
{
    private const int MaxUserLanguages = 3;
    public List<LanguageCode> Value { get; }

    private UserLanguageList(List<LanguageCode> value)
    {
        Value = value;
    }

    public static Result<UserLanguageList> Create(List<LanguageCode> userLanguages)
    {
        if (userLanguages.Count > MaxUserLanguages)
        {
            return Result.Fail<UserLanguageList>($"Users allowed to add up to {MaxUserLanguages} languages.");
        }

        var duplicateLanguages = userLanguages
            .GroupBy(x => x)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key)
            .ToList();

        return duplicateLanguages.Any()
            ? Result.Fail<UserLanguageList>(
                $"Every language should be added only once. Please check: {string.Join(", ", duplicateLanguages)}.")
            : new UserLanguageList(userLanguages);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}