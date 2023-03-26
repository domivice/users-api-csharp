using System.Collections.Generic;
using System.Linq;
using Domivice.Users.Application.Users.Commands.UpdateUser;
using Domivice.Users.Application.Users.ReadModels;
using Domivice.Users.Web.Models;

namespace Domivice.Users.Web.Extensions;

/// <summary>
/// 
/// </summary>
public static class PatchUserExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userUpdate"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static UpdateUserCommand ToCommand(this UserUpdate userUpdate, string userId)
    {
        return new UpdateUserCommand
        {
            UserId = userId,
            FirstName = userUpdate.FirstName,
            LastName = userUpdate.LastName,
            PhoneNumber = userUpdate.PhoneNumber,
            PhoneCountryCode = userUpdate.PhoneCountryCode,
            DisplayLanguage = userUpdate.DisplayLanguage,
            UserBio = userUpdate.UserBio,
            Website = userUpdate.Website,
            EntryInstructions = userUpdate.EntryInstructions,
            Languages = userUpdate.Languages,
            HomeAddress = userUpdate.HomeAddress?.ToCommand(),
            SocialMediaUrls = userUpdate.SocialMediaUrls?.Select(url => url.ToCommand()).ToList() ??
                              new List<UpdateSocialMediaUrlCommand>()
        };
    }

    private static UpdateAddressCommand ToCommand(this Address userRm)
    {
        return new UpdateAddressCommand(
            userRm.Street,
            userRm.City,
            userRm.State,
            userRm.Country,
            userRm.PostalCode
        );
    }

    private static UpdateSocialMediaUrlCommand ToCommand(this SocialMediaUrl url)
    {
        return new UpdateSocialMediaUrlCommand(url.Site, url.Url);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userRm"></param>
    /// <returns></returns>
    public static User ToUser(this UserRm userRm)
    {
        return new User
        {
            Id = userRm.Id.ToString(),
            FirstName = userRm.FirstName,
            LastName = userRm.LastName,
            Email = userRm.Email,
            PhoneNumber = userRm.PhoneNumber,
            DisplayLanguage = userRm.DisplayLanguage,
            UserBio = userRm.UserBio,
            Website = userRm.Website,
            EntryInstructions = userRm.EntryInstructions,
            Languages = userRm.Languages,
            HomeAddress = userRm.HomeAddress?.ToAddress(),
            SocialMediaUrls = userRm.SocialMediaUrls.ConvertAll(url => url.ToSocialMediaUrl())
        };
    }

    private static Address ToAddress(this AddressRm addressRm)
    {
        return new Address
        {
            Street = addressRm.Street,
            City = addressRm.City,
            State = addressRm.State,
            Country = addressRm.Country,
            PostalCode = addressRm.PostalCode
        };
    }

    private static SocialMediaUrl ToSocialMediaUrl(this SocialMediaUrlRm socialMediaUrlRm)
    {
        return new SocialMediaUrl
        {
            Url = socialMediaUrlRm.Uri,
            Site = socialMediaUrlRm.Site
        };
    }
}