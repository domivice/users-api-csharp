using Domivice.Domain.ValueObjects;

namespace Domivice.Users.Domain.Entities;

public sealed class User : BaseEntity<Guid>
{
    public FirstName FirstName { get; }
    public LastName LastName { get; }
    public Email Email { get; }
    public PhoneNumber PhoneNumber { get; }
    public CultureCode DisplayLanguage { get; }
    public Text UserBio { get; }
    public Address HomeAddress { get; }
    public Uri Website { get; }
    public Text EntryInstructions { get; }

    public IEnumerable<UserLanguage> UserLanguages { get; } = new List<UserLanguage>();
    public IEnumerable<UserSocialMediaUrl> UserSocialMediaUrls { get; } = new List<UserSocialMediaUrl>();
}