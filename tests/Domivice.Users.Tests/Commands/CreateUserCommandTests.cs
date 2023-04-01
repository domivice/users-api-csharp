using System;
using System.Threading.Tasks;
using Domivice.Domain.ValueObjects;
using Domivice.Users.Application.Users.Commands.CreateUser;
using Domivice.Users.Domain.Errors;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Domivice.Users.Tests.Commands;

[Collection("TestCollection")]
public class CreateUserCommandTests
{
    private readonly ISender _mediatorSender;

    public CreateUserCommandTests(TestServer server)
    {
        var scope = server.GetServiceScope();
        _mediatorSender = scope.ServiceProvider.GetRequiredService<ISender>();
    }

    [Fact]
    public async Task Should_Create_User_With_Success()
    {
        var command = BuildCreateUserCommand();
        var result = await _mediatorSender.Send(command);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Should_Return_Error_When_Creating_Same_User_Twice()
    {
        var command = BuildCreateUserCommand();
        var result1 = await _mediatorSender.Send(command);
        result1.IsSuccess.Should().BeTrue();
        var result2 = await _mediatorSender.Send(command);
        result2.IsSuccess.Should().BeFalse();
        result2.HasError<ConflictError>().Should().BeTrue();
    }

    public static TheoryData<CreateUserCommand, string> InvalidCreateUserCommands => new()
    {
        {BuildCreateUserCommand(userId:"invalid-guid"), "UserId"},
        {BuildCreateUserCommand(firstName:"!nvalid first n@me"), "FirstName"},
        {BuildCreateUserCommand(lastName:"!nvalid last n@me"), "LastName"},
        {BuildCreateUserCommand(phoneNumber:"!nvalid ph0ne number"), "PhoneNumber"},
        {BuildCreateUserCommand(phoneCountryCode:"!nvalid c0untry c0de"), "PhoneCountryCode"},
        {BuildCreateUserCommand(displayLanguage:"!nvalid language c0de"), "DisplayLanguage"},
    };

    [Theory]
    [MemberData(nameof(InvalidCreateUserCommands))]
    public async Task Should_Return_Validation_Errors(CreateUserCommand command, string errorField)
    {
        var result = await _mediatorSender.Send(command);
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().OnlyContain(e => (e as ValidationError)!.Errors.ContainsKey(errorField));
    }

    private static CreateUserCommand BuildCreateUserCommand(string? userId = null, string? firstName = null,
        string? lastName = null, string? email = null, string? phoneNumber = null, string? phoneCountryCode = null,
        string? displayLanguage = null)
    {
        firstName ??= NamesGenerator.FirstName();
        lastName ??= NamesGenerator.LastName();
        email ??= $"{firstName}.{lastName}@{nameof(CreateUserCommandTests)}.com".ToLower();

        return new CreateUserCommand
        {
            UserId = userId ?? Guid.NewGuid().ToString(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber ?? "4389000000",
            PhoneCountryCode = phoneCountryCode ?? CountryCode.Ca,
            DisplayLanguage = displayLanguage ?? CultureCode.EnCa
        };
    }
}