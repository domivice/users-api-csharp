using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Domivice.Domain.ValueObjects;
using Domivice.Users.Infrastructure.Persistence;
using Domivice.Users.Web.Constants;
using Domivice.Users.Web.Models;
using FluentAssertions;
using IdentityModel;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using User = Domivice.Users.Domain.Entities.User;

namespace Domivice.Users.Tests.UsersApi;

[Collection("TestCollection")]
public class GetUsersTests
{
    private readonly List<User> _testUsers;
    private readonly TestServer _testServer;

    public GetUsersTests(TestServer server)
    {
        _testServer = server;
        var scope = server.GetServiceScope();
        _testUsers = BuildUserEntities(50);
        var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        dbContext.Users.AddRange(_testUsers);
        dbContext.SaveChanges();
    }

    private static string GetEndpoint()
    {
        return "/v1/users";
    }
    
    [Fact]
    public async Task Should_Return_Forbidden_When_User_Does_Not_Have_The_Required_Permissions()
    {
        var response = await _testServer
            .CreateAuthenticatedClient()
            .GetAsync(GetEndpoint());
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Should_Return_UnAuthorized_When_User_Not_Authenticated()
    {
        var response = await _testServer
            .CreateUnAuthenticatedClient()
            .GetAsync(GetEndpoint());
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_Return_UserList_Success()
    {
        var response = await _testServer
            .CreateAuthenticatedClient(new[] {new Claim(JwtClaimTypes.Role, UserRoles.AppAdmin)})
            .GetAsync(GetEndpoint());
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var userList = await response.Content.ReadFromJsonAsync<UserList>();
        userList?.PreviousPage.Should().BeNull();
        userList?.NextPage.Should().NotBeNull();
        userList?.Data.Count.Should().BePositive();
    }

    [Theory]
    [InlineData("FirstName")]
    [InlineData("LastName")]
    [InlineData("Email")]
    public async Task Should_Return_UserList_As_Search_Result(string searchFieldName)
    {
        var firstUser = _testUsers.First();
        var searchTerm = firstUser.GetType().GetProperty(searchFieldName)?.GetValue(firstUser)?.ToString();
        var queryString = new Dictionary<string, string> {{"search", searchTerm!}};
        var response = await _testServer
            .CreateAuthenticatedClient(new[] {new Claim(JwtClaimTypes.Role, UserRoles.AppAdmin)})
            .GetAsync(QueryHelpers.AddQueryString(GetEndpoint(), queryString!));
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var userList = await response.Content.ReadFromJsonAsync<UserList>();
        userList?.Data.Should().OnlyContain(user =>
            user.FirstName.Contains(searchTerm!)
            || user.LastName.Contains(searchTerm!)
            || user.Email.Contains(searchTerm!)
        );
    }

    [Fact]
    public async Task Should_Return_UserList_As_Search_By_PhoneNumber_Result()
    {
        const string searchTerm = "438";
        var queryString = new Dictionary<string, string> {{"search", searchTerm}};
        var response = await _testServer
            .CreateAuthenticatedClient(new[] {new Claim(JwtClaimTypes.Role, UserRoles.AppAdmin)})
            .GetAsync(QueryHelpers.AddQueryString(GetEndpoint(), queryString!));
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var userList = await response.Content.ReadFromJsonAsync<UserList>();
        userList?.Data.Should().OnlyContain(user => user.PhoneNumber.Contains(searchTerm));
    }

    public static TheoryData<Dictionary<string, string>, string> SortTheoryData => new()
    {
        {new Dictionary<string, string> {{"sort", "firstName"}}, "FirstName"},
        {new Dictionary<string, string> {{"sort", "firstName:desc"}}, "FirstName"},
        {new Dictionary<string, string> {{"sort", "lastName"}}, "LastName"},
        {new Dictionary<string, string> {{"sort", "lastName:desc"}}, "LastName"},
        {new Dictionary<string, string> {{"sort", "phoneNumber"}}, "PhoneNumber"},
        {new Dictionary<string, string> {{"sort", "phoneNumber:desc"}}, "PhoneNumber"},
        {new Dictionary<string, string> {{"sort", "email"}}, "Email"},
        {new Dictionary<string, string> {{"sort", "email:desc"}}, "Email"}
    };

    [Theory]
    [MemberData(nameof(SortTheoryData))]
    public async Task Should_Return_Sorted_UserList(Dictionary<string, string> queryString, string sortFieldName)
    {
        var response = await _testServer
            .CreateAuthenticatedClient(new[] {new Claim(JwtClaimTypes.Role, UserRoles.AppAdmin)})
            .GetAsync(QueryHelpers.AddQueryString(GetEndpoint(), queryString!));
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var userList = await response.Content.ReadFromJsonAsync<UserList>();
        var sortedUsesListData = queryString["sort"].Contains("desc")
            ? userList?.Data.OrderByDescending(u => u.GetType().GetProperty(sortFieldName)!.GetValue(u)).ToList()
            : userList?.Data.OrderBy(u => u.GetType().GetProperty(sortFieldName)!.GetValue(u)).ToList();
        sortedUsesListData!.SequenceEqual(userList!.Data).Should().BeTrue();
    }

    private static List<User> BuildUserEntities(int numberOfUsers)
    {
        return Enumerable.Range(1, numberOfUsers).Select(i =>
        {
            var firstName = NamesGenerator.FirstName();
            var lastName = NamesGenerator.LastName();
            var email = $"{firstName}.{lastName}@{nameof(GetUsersTests)}.com".ToLower();
            return new User(
                Guid.NewGuid(),
                FirstName.Create(firstName).Value,
                LastName.Create(lastName).Value,
                Email.Create(email).Value,
                PhoneNumber.Create("4389000000", "CA").Value,
                CultureCode.EnCa
            );
        }).ToList();
    }
}