using Domivice.Domain.ValueObjects;
using FluentResults;

namespace Domivice.Users.Domain.Entities;

public sealed class UserLanguage : BaseEntity<Guid>
{
    private UserLanguage(Guid userId, LanguageCode languageCode)
    {
        UserId = userId;
        LanguageCode = languageCode;
    }

    private UserLanguage(LanguageCode languageCode)
    {
        LanguageCode = languageCode;
    }

    public static Result<UserLanguage> Create(LanguageCode languageCode)
    {
        return new UserLanguage(languageCode);
    }

    public Guid UserId { get; }
    public User User { get; }
    public LanguageCode LanguageCode { get; }
}