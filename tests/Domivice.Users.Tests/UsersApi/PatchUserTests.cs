using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Domivice.Domain.ValueObjects;
using Domivice.Users.Application.Common.Interfaces;
using Domivice.Users.Domain.ValueObjects;
using Domivice.Users.Web.Models;
using Domivice.Users.Tests.Extensions;
using Domivice.Users.Web.Constants;
using FluentAssertions;
using IdentityModel;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly TestServer _testServer;
    private readonly UserEntity _defaultTestUser;

    public PatchUserTests(TestServer server)
    {
        _testServer = server;
        var scope = server.GetServiceScope();
        _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        _defaultTestUser = BuildUserEntity();
        _unitOfWork.UserRepository.Insert(_defaultTestUser);
        _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
    }

    private static string GetEndpoint(string userId)
    {
        return $"/v1/users/{userId}";
    }

    [Theory]
    [InlineData("cd39ac3a-cb1c-41e2-94a8-8b2868099b57")]
    [InlineData("!nvalid u$er !d")]
    public async Task Should_Return_Not_Found_When_User_Not_Found(string userId)
    {
        var userUpdate = BuildUserUpdateModel();
        var response = await _testServer.CreateAuthenticatedClient()
            .PatchAsJsonAsync(GetEndpoint(userId), userUpdate);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_Return_UnAuthorized_When_User_Not_Authenticated()
    {
        var userUpdate = BuildUserUpdateModel();
        var response = await _testServer.CreateUnAuthenticatedClient()
            .PatchAsJsonAsync(GetEndpoint(Guid.NewGuid().ToString()), userUpdate);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_Return_Forbidden_When_User_Does_Not_Have_The_Required_Permissions()
    {
        var userEntity = BuildUserEntity();
        _unitOfWork.UserRepository.Insert(userEntity);
        await _unitOfWork.SaveChangesAsync();
        userEntity.Id.Should().NotBe(default(Guid));

        var userUpdate = BuildUserUpdateModel();
        var response = await _testServer.CreateAuthenticatedClient(new[]
        {
            new Claim(JwtClaimTypes.Subject, Guid.NewGuid().ToString())
        }).PatchAsJsonAsync(GetEndpoint(userEntity.Id.ToString()), userUpdate);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Should_Return_Success_When_User_Is_Admin()
    {
        var userEntity = BuildUserEntity();
        _unitOfWork.UserRepository.Insert(userEntity);
        await _unitOfWork.SaveChangesAsync();
        userEntity.Id.Should().NotBe(default(Guid));
        
        var userUpdate = BuildUserUpdateModel();
        var response = await _testServer.CreateAuthenticatedClient(new[]
        {
            new Claim(JwtClaimTypes.Subject, Guid.NewGuid().ToString()),
            new Claim(JwtClaimTypes.Role, UserRoles.AppAdmin)
        }).PatchAsJsonAsync(GetEndpoint(userEntity.Id.ToString()), userUpdate);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_Update_User_With_Success()
    {
        var userEntity = BuildUserEntity();
        _unitOfWork.UserRepository.Insert(userEntity);
        await _unitOfWork.SaveChangesAsync();
        userEntity.Id.Should().NotBe(default(Guid));

        var userUpdate = BuildUserUpdateModel();
        var response = await _testServer.CreateAuthenticatedClient(new[]
        {
            new Claim(JwtClaimTypes.Subject, userEntity.Id.ToString())
        }).PatchAsJsonAsync(GetEndpoint(userEntity.Id.ToString()), userUpdate);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = await response.Content.ReadFromJsonAsync<UserModel>();
        user?.Id.Should().Be(userEntity.Id.ToString());
        user?.HomeAddress.Should().NotBeNull();
        user?.SocialMediaUrls.Should().NotBeNull();
    }

    public static TheoryData<UserUpdate, string> InvalidUserUpdateData => new()
    {
        {BuildUserUpdateModel("!nvalid first n@me"), "firstName"},
        {BuildUserUpdateModel(lastName: "!nvalid last n@me"), "lastName"},
        {BuildUserUpdateModel(phoneNumber: "!nvalid ph0ne number"), "phoneNumber"},
        {BuildUserUpdateModel(phoneCountryCode: "!nvalid c0untry c0de"), "phoneCountryCode"},
        {BuildUserUpdateModel(displayLanguage: "!nvalid language c0de"), "displayLanguage"},
        {BuildUserUpdateModel(webSite: "!nvalid url"), "website"},
        {
            BuildUserUpdateModel(languages: new List<string>
                {LanguageCode.English, LanguageCode.Arabic, LanguageCode.Hebrew, LanguageCode.Afrikaans}),
            "languages"
        },
        {
            BuildUserUpdateModel(languages: new List<string> {LanguageCode.English, LanguageCode.English}),
            "languages"
        },
        {
            BuildUserUpdateModel(languages: new List<string> {"!nvalid language code"}),
            "languages[0]"
        },
        {
            BuildUserUpdateModel(socialMediaUrls: new List<SocialMediaUrl>
                {new() {Site = "!nvalid site", Url = "!nvalid url"}}),
            "socialMediaUrls[0]"
        },
        {
            BuildUserUpdateModel(socialMediaUrls: new List<SocialMediaUrl>
                {new() {Site = SocialMediaSite.Facebook, Url = "https://twitter.com/nelsonkana"}}),
            "socialMediaUrls[0]"
        },
        {
            BuildUserUpdateModel(socialMediaUrls: new List<SocialMediaUrl>
                {new() {Site = SocialMediaSite.Twitter, Url = "https://www.facebook.com/NocesDeKana"}}),
            "socialMediaUrls[0]"
        },
        {
            BuildUserUpdateModel(socialMediaUrls: new List<SocialMediaUrl>
                {new() {Site = SocialMediaSite.YouTube, Url = "https://www.facebook.com/NocesDeKana"}}),
            "socialMediaUrls[0]"
        },
        {
            BuildUserUpdateModel(socialMediaUrls: new List<SocialMediaUrl>
                {new() {Site = SocialMediaSite.Instagram, Url = "https://www.facebook.com/NocesDeKana"}}),
            "socialMediaUrls[0]"
        },
        {
            BuildUserUpdateModel(socialMediaUrls: new List<SocialMediaUrl>
            {
                new() {Site = SocialMediaSite.Facebook, Url = "https://www.facebook.com/NocesDeKana"},
                new() {Site = SocialMediaSite.Facebook, Url = "https://www.facebook.com/NocesDeKana"}
            }),
            "socialMediaUrls"
        }
    };

    [Theory]
    [MemberData(nameof(InvalidUserUpdateData))]
    public async Task Should_Return_Validation_Errors(UserUpdate userUpdate, string errorField)
    {
        var response = await _testServer.CreateAuthenticatedClient(new[]
        {
            new Claim(JwtClaimTypes.Subject, _defaultTestUser.Id.ToString())
        }).PatchAsJsonAsync(GetEndpoint(_defaultTestUser.Id.ToString()), userUpdate);
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