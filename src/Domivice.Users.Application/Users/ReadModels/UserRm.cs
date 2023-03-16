using Domivice.Users.Domain.Entities;

namespace Domivice.Users.Application.Users.ReadModels;

public class UserRm
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string DisplayLanguage { get; set; } = string.Empty;
    public string? UserBio { get; set; }
    public AddressRm? HomeAddress { get; set; }
    public string? Website { get; set; }
    public string? EntryInstructions { get; set; }
    public List<string> Languages { get; set; } = new();
    public List<SocialMediaUrlRm> SocialMediaUrls { get; set; } = new();

    public static explicit operator UserRm(User user)
    {
        return new UserRm
        {
            FirstName = user.FirstName.Value,
            LastName = user.LastName.Value,
            Email = user.Email.Value,
            PhoneNumber = user.PhoneNumber.Number,
            DisplayLanguage = user.DisplayLanguage.Value,
            UserBio = user.UserBio?.Value,
            Website = user.Website?.ToString(),
            EntryInstructions = user.EntryInstructions?.Value,
            Languages = user.UserLanguages.ToList().ConvertAll(l => l.LanguageCode.Value),
            SocialMediaUrls = user.UserSocialMediaUrls.ToList().ConvertAll(url => (SocialMediaUrlRm) url),
            HomeAddress = user.HomeAddress!.Equals(null!) ? null : (AddressRm) user.HomeAddress
        };
    }
}