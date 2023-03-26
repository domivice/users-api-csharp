using Domivice.Domain.ValueObjects;
using Domivice.Users.Domain.ValueObjects;

namespace Domivice.Users.Domain.Entities;

public sealed class User : BaseEntity<Guid>
{
    private readonly List<UserLanguage> _userLanguages = new();
    private readonly List<UserSocialMediaUrl> _userSocialMediaUrls = new();

    private User()
    {
    }

    public User(Guid userId,FirstName firstName, LastName lastName, Email email, PhoneNumber phoneNumber,
        CultureCode displayLanguage)
    {
        Id = userId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        DisplayLanguage = displayLanguage;
    }

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public CultureCode DisplayLanguage { get; private set; }
    public Text? UserBio { get; private set; }
    public Address? HomeAddress { get; private set; }
    public Uri? Website { get; private set; }
    public Text? EntryInstructions { get; private set; }
    public IEnumerable<UserLanguage> UserLanguages => _userLanguages.AsReadOnly();
    public IEnumerable<UserSocialMediaUrl> UserSocialMediaUrls => _userSocialMediaUrls.AsReadOnly();

    public void UpdateFirstName(FirstName firstName)
    {
        FirstName = firstName;
    }

    public void UpdateLastName(LastName lastName)
    {
        LastName = lastName;
    }

    public void UpdateEmail(Email email)
    {
        Email = email;
    }

    public void UpdatePhoneNumber(PhoneNumber phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }

    public void UpdateDisplayLanguage(CultureCode displayLanguage)
    {
        DisplayLanguage = displayLanguage;
    }

    public void UpdateUserBiography(Text userBio)
    {
        UserBio = userBio;
    }

    public void UpdateHomeAddress(Address address)
    {
        HomeAddress = address;
    }

    public void UpdateUserWebsite(Uri website)
    {
        Website = website;
    }

    public void UpdateEntryInstructions(Text instructions)
    {
        EntryInstructions = instructions;
    }
    public void SetUserLanguages(UserLanguageList userLanguageList)
    {
        // Remove user languages which are not part of the new list
        _userLanguages.RemoveAll(userLanguage => !userLanguageList.Value.Contains(userLanguage.LanguageCode));

        // Add user languages which have not already been added
        foreach (var newLanguageCode in userLanguageList.Value.Where(newLanguageCode =>
                     !_userLanguages.Any(userLanguage => userLanguage.LanguageCode.Equals(newLanguageCode))))
            _userLanguages.Add(UserLanguage.Create(newLanguageCode).Value);
    }

    public void SetUserSocialMediaUrls(List<SocialMediaUrl> newSocialMediaUrls)
    {
        _userSocialMediaUrls.RemoveAll(userSocialMediaUrl =>
            !newSocialMediaUrls.Contains(userSocialMediaUrl.SocialMediaUrl));

        foreach (var newSocialMediaUrl in newSocialMediaUrls.Where(newSocialMediaUrl =>
                     !_userSocialMediaUrls.Any(userSocialMediaUrl =>
                         userSocialMediaUrl.SocialMediaUrl.Equals(newSocialMediaUrl))))
            _userSocialMediaUrls.Add(UserSocialMediaUrl.Create(newSocialMediaUrl).Value);
    }
    
    public void SetUserSocialMediaUrls(UserSocialMediaUrlList userSocialMediaUrlList)
    {
        _userSocialMediaUrls.RemoveAll(userSocialMediaUrl =>
            !userSocialMediaUrlList.Value.Contains(userSocialMediaUrl.SocialMediaUrl));

        foreach (var newSocialMediaUrl in userSocialMediaUrlList.Value.Where(newSocialMediaUrl =>
                     !_userSocialMediaUrls.Any(userSocialMediaUrl =>
                         userSocialMediaUrl.SocialMediaUrl.Equals(newSocialMediaUrl))))
            _userSocialMediaUrls.Add(UserSocialMediaUrl.Create(newSocialMediaUrl).Value);
    }
}