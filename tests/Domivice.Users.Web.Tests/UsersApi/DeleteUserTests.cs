using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Domivice.Domain.ValueObjects;
using Domivice.Users.Application.Common.Interfaces;
using Domivice.Users.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using UserEntity = Domivice.Users.Domain.Entities.User;
using UserModel = Domivice.Users.Web.Models.User;

namespace Domivice.Users.Web.Tests.UsersApi;

[Collection("TestCollection")]
public class DeleteUserTests
{
    private readonly HttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UsersDbContext _dbContext;
    
    public DeleteUserTests(TestServer server)
    {
        _httpClient = server.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:5005/");

        var scope = server.GetServiceScope();
        _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        _dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
    }
    
    private static string GetEndpoint(string userId)
    {
        return $"/v1/users/{userId}";
    }
    
    [Fact]
    public async Task Should_Return_Not_Found_When_User_Not_Exist()
    {
        var response = await _httpClient.DeleteAsync(GetEndpoint(Guid.NewGuid().ToString()));
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task Should_Delete_User_With_Success()
    {
        var userEntity = BuildUserEntity();
        _unitOfWork.UserRepository.Insert(userEntity);
        await _unitOfWork.SaveChangesAsync();
        userEntity.Id.Should().NotBe(default(Guid));  
            
        var response = await _httpClient.DeleteAsync(GetEndpoint(userEntity.Id.ToString()));
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        await _dbContext.Entry(userEntity).ReloadAsync();
        var userFromDb = _unitOfWork.UserRepository.GetById(userEntity.Id);
        userFromDb.Should().BeNull();
    }
    
    private static UserEntity BuildUserEntity()
    {
        var user = new UserEntity(
            Guid.NewGuid(),
            FirstName.Create(NamesGenerator.FirstName()).Value,
            LastName.Create(NamesGenerator.LastName()).Value,
            Email.Create($"{Guid.NewGuid()}@{nameof(DeleteUserTests)}.com".ToLower()).Value,
            PhoneNumber.Create("4389000000", "CA").Value,
            CultureCode.EnCa
        );

        return user;
    }
}