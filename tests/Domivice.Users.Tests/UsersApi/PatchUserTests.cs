using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Domivice.Domain.ValueObjects;
using Domivice.Users.Application.Common.Interfaces;
using Domivice.Users.Domain.ValueObjects;
using Domivice.Users.Web.Models;
using Domivice.Users.Tests.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Address = Domivice.Users.Web.Models.Address;
using SocialMediaUrl = Domivice.Users.Web.Models.SocialMediaUrl;
using UserEntity = Domivice.Users.Domain.Entities.User;
using UserModel = Domivice.Users.Web.Models.User;


namespace Domivice.Users.Tests.UsersApi;

[Collection("TestCollection")]
public class PatchUserTests
{
    private readonly HttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;

    public PatchUserTests(TestServer server)
    {
        _httpClient = server.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:5005/");

        var scope = server.GetServiceScope();
        _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    }

    private static string GetEndpoint(string userId)
    {
        return $"/v1/users/{userId}";
    }

    [Fact]
    public async Task Should_Return_Not_Found_When_User_Not_Found()
    {
        var userUpdate = BuildUserUpdateModel();
        var response = await _httpClient.PatchAsJsonAsync(GetEndpoint(Guid.NewGuid().ToString()), userUpdate);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_Update_User_With_Success()
    {
        var userEntity = BuildUserEntity();
        _unitOfWork.UserRepository.Insert(userEntity);
        await _unitOfWork.SaveChangesAsync();
        userEntity.Id.Should().NotBe(default(Guid));

        var userUpdate = BuildUserUpdateModel();
        var response = await _httpClient.PatchAsJsonAsync(GetEndpoint(userEntity.Id.ToString()), userUpdate);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = await response.Content.ReadFromJsonAsync<UserModel>();
        user?.Id.Should().Be(userEntity.Id.ToString());
        user?.HomeAddress.Should().NotBeNull();
        user?.SocialMediaUrls.Should().NotBeNull();
    }

    public static TheoryData<UserUpdate, string, string> InvalidUserUpdateData => new()
    {
        {BuildUserUpdateModel(), "!nvalid u$er !d", "userId"},
        {BuildUserUpdateModel("!nvalid first n@me"), Guid.NewGuid().ToString(), "firstName"},
        {BuildUserUpdateModel(lastName: "!nvalid last n@me"), Guid.NewGuid().ToString(), "lastName"},
        {BuildUserUpdateModel(phoneNumber: "!nvalid ph0ne number"), Guid.NewGuid().ToString(), "phoneNumber"},
        {BuildUserUpdateModel(phoneCountryCode: "!nvalid c0untry c0de"), Guid.NewGuid().ToString(), "phoneCountryCode"},
        {BuildUserUpdateModel(displayLanguage: "!nvalid language c0de"), Guid.NewGuid().ToString(), "displayLanguage"},
        {BuildUserUpdateModel(webSite: "!nvalid url"), Guid.NewGuid().ToString(), "website"},
        {
            BuildUserUpdateModel(languages: new List<string>
                {LanguageCode.English, LanguageCode.Arabic, LanguageCode.Hebrew, LanguageCode.Afrikaans}),
            Guid.NewGuid().ToString(), "languages"
        },
        {
            BuildUserUpdateModel(languages: new List<string> {LanguageCode.English, LanguageCode.English}),
            Guid.NewGuid().ToString(), "languages"
        },
        {
            BuildUserUpdateModel(languages: new List<string> {"!nvalid language code"}), Guid.NewGuid().ToString(),
            "languages[0]"
        },
        {
            BuildUserUpdateModel(socialMediaUrls: new List<SocialMediaUrl>
                {new() {Site = "!nvalid site", Url = "!nvalid url"}}),
            Guid.NewGuid().ToString(),
            "socialMediaUrls[0]"
        },
        {
            BuildUserUpdateModel(socialMediaUrls: new List<SocialMediaUrl>
                {new() {Site = SocialMediaSite.Facebook, Url = "https://twitter.com/nelsonkana"}}),
            Guid.NewGuid().ToString(),
            "socialMediaUrls[0]"
        },
        {
            BuildUserUpdateModel(socialMediaUrls: new List<SocialMediaUrl>
                {new() {Site = SocialMediaSite.Twitter, Url = "https://www.facebook.com/NocesDeKana"}}),
            Guid.NewGuid().ToString(),
            "socialMediaUrls[0]"
        },
        {
            BuildUserUpdateModel(socialMediaUrls: new List<SocialMediaUrl>
                {new() {Site = SocialMediaSite.YouTube, Url = "https://www.facebook.com/NocesDeKana"}}),
            Guid.NewGuid().ToString(),
            "socialMediaUrls[0]"
        },
        {
            BuildUserUpdateModel(socialMediaUrls: new List<SocialMediaUrl>
                {new() {Site = SocialMediaSite.Instagram, Url = "https://www.facebook.com/NocesDeKana"}}),
            Guid.NewGuid().ToString(),
            "socialMediaUrls[0]"
        },
        {
            BuildUserUpdateModel(socialMediaUrls: new List<SocialMediaUrl>
            {
                new() {Site = SocialMediaSite.Facebook, Url = "https://www.facebook.com/NocesDeKana"},
                new() {Site = SocialMediaSite.Facebook, Url = "https://www.facebook.com/NocesDeKana"}
            }),
            Guid.NewGuid().ToString(),
            "socialMediaUrls"
        }
    };

    [Theory]
    [MemberData(nameof(InvalidUserUpdateData))]
    public async Task Should_Return_Validation_Errors(UserUpdate userUpdate, string userId,
        string errorField)
    {
        var response = await _httpClient.PatchAsJsonAsync(GetEndpoint(userId), userUpdate);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemResult = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        problemResult?.Errors.Should().ContainKey(errorField);
    }

    private static UserEntity BuildUserEntity()
    {
        var user = new UserEntity(
            Guid.NewGuid(),
            FirstName.Create(NamesGenerator.FirstName()).Value,
            LastName.Create(NamesGenerator.LastName()).Value,
            Email.Create($"{Guid.NewGuid()}@{nameof(PatchUserTests)}.com".ToLower()).Value,
            PhoneNumber.Create("4389000000", "CA").Value,
            CultureCode.EnCa
        );

        user.SetUserLanguages(UserLanguageList.Create(new List<LanguageCode>
            {
                LanguageCode.Hebrew, LanguageCode.Arabic
            }).Value
        );

        return user;
    }

    private static UserUpdate BuildUserUpdateModel(string? firstName = null, string? lastName = null,
        string? phoneNumber = null, string? phoneCountryCode = null, string? displayLanguage = null,
        string? userBio = null, string? webSite = null, string? entryInstructions = null, Address? homeAddress = null,
        List<string>? languages = null, List<SocialMediaUrl>? socialMediaUrls = null)
    {
        return new UserUpdate
        {
            FirstName = firstName ?? NamesGenerator.FirstName(),
            LastName = lastName ?? NamesGenerator.LastName(),
            PhoneNumber = phoneNumber ?? "4389000001",
            PhoneCountryCode = phoneCountryCode ?? CountryCode.Ca,
            DisplayLanguage = displayLanguage ?? CultureCode.EnCa,
            UserBio = userBio ?? "check wikipedia",
            Website = webSite ?? "https://domivice.com",
            EntryInstructions = entryInstructions ?? "stay out of here",
            HomeAddress = homeAddress ?? new Address
            {
                Street = "7401 rue de Marseille",
                City = "Montreal",
                State = "Quebec",
                Country = "Canada",
                PostalCode = "H1N 0A9"
            },
            SocialMediaUrls = socialMediaUrls ?? new List<SocialMediaUrl>
            {
                new()
                {
                    Site = SocialMediaSite.Facebook,
                    Url = "https://www.facebook.com/NocesDeKana"
                },
                new()
                {
                    Site = SocialMediaSite.Twitter,
                    Url = "https://twitter.com/nelsonkana"
                },
                new()
                {
                    Site = SocialMediaSite.YouTube,
                    Url = "https://www.youtube.com/channel/UCx7NocBn5zoYOgatTFNJHjA"
                },
            },
            Languages = languages ?? new List<string>
            {
                LanguageCode.English
            }
        };
    }
}