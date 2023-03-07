using Domivice.Domain.ValueObjects;

namespace Domivice.Users.Domain.Entities;

public sealed class UserLanguage : BaseEntity<Guid>
{
    public Guid UserId { get; }
    public User User { get; }
    public LanguageCode LanguageCode { get; }
}