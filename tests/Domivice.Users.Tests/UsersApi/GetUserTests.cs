using System;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Domivice.Domain.ValueObjects;
using Domivice.Users.Application.Common.Interfaces;
using Domivice.Users.Web.Constants;
using FluentAssertions;
using IdentityModel;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using UserEntity = Domivice.Users.Domain.Entities.User;
using UserModel = Domivice.Users.Web.Models.User;

namespace Domivice.Users.Tests.UsersApi;

[Collection("TestCollection")]
public class GetUserTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly TestServer _testServer;

    public GetUserTests(TestServer server)
    {
        _testServer = server;
        var scope = server.GetServiceScope();
        _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    }

    private static string GetEndpoint(string userId)
    {
        return $"/v1/users/{userId}";
    }

    [Fact]
    public async Task Should_Return_UnAuthorized_When_User_Not_Authenticated()
    {
        var response = await _testServer.CreateUnAuthenticatedClient()
            .GetAsync(GetEndpoint(Guid.NewGuid().ToString()));
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Should_Return_Forbidden_When_User_Does_Not_Have_The_Required_Permissions()
    {
        var userEntity = BuildUserEntity();
        _unitOfWork.UserRepository.Insert(userEntity);
        await _unitOfWork.SaveChangesAsync();
        userEntity.Id.Should().NotBe(default(Guid));
        
        var response = await _testServer.CreateAuthenticatedClient()
            .GetAsync(GetEndpoint(userEntity.Id.ToString()));
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Should_Return_Success_When_User_Is_Admin()
    {
        var userEntity = BuildUserEntity();
        _unitOfWork.UserRepository.Insert(userEntity);
        await _unitOfWork.SaveChangesAsync();
        userEntity.Id.Should().NotBe(default(Guid));

        var response = await _testServer.CreateAuthenticatedClient(new[]
        {
            new Claim(JwtClaimTypes.Subject, Guid.NewGuid().ToString()),
            new Claim(JwtClaimTypes.Role, UserRoles.AppAdmin)
        }).GetAsync(GetEndpoint(userEntity.Id.ToString()));
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_Get_User_With_Success()
    {
        var userEntity = BuildUserEntity();
        _unitOfWork.UserRepository.Insert(userEntity);
        await _unitOfWork.SaveChangesAsync();
        userEntity.Id.Should().NotBe(default(Guid));

        var response = await _testServer
            .CreateAuthenticatedClient(new[] {new Claim(JwtClaimTypes.Subject, userEntity.Id.ToString())})
            .GetAsync(GetEndpoint(userEntity.Id.ToString()));
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var user = await response.Content.ReadFromJsonAsync<UserModel>();
        user?.Id.Should().Be(userEntity.Id.ToString());
    }

    [Fact]
    public async Task Should_Return_Not_Found_When_User_Does_Not_Exist()
    {
        var response = await _testServer.CreateAuthenticatedClient()
            .GetAsync(GetEndpoint(Guid.NewGuid().ToString()));
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private static UserEntity BuildUserEntity()
    {
        var user = new UserEntity(
            Guid.NewGuid(),
            FirstName.Create(NamesGenerator.FirstName()).Value,
            LastName.Create(NamesGenerator.LastName()).Value,
            Email.Create($"{Guid.NewGuid()}@{nameof(GetUserTests)}.com".ToLower()).Value,
            PhoneNumber.Create("4389000000", "CA").Value,
            CultureCode.EnCa
        );

        return user;
    }
}