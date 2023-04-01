using Domivice.IntegrationEvents;
using Domivice.Users.Application.Users.Commands.CreateUser;
using Domivice.Users.Domain.Errors;
using Domivice.Users.Infrastructure.Consumers.Exceptions;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domivice.Users.Infrastructure.Consumers;

public class UserIdentityCreatedConsumer : IConsumer<UserIdentityCreated>
{
    private readonly ILogger<UserIdentityCreatedConsumer> _logger;
    private readonly ISender _mediatorSender;

    public UserIdentityCreatedConsumer(ILogger<UserIdentityCreatedConsumer> logger, ISender mediatorSender)
    {
        _logger = logger;
        _mediatorSender = mediatorSender;
    }

    public async Task Consume(ConsumeContext<UserIdentityCreated> context)
    {
        _logger.LogInformation("Received new identity for user: {Email}", context.Message.Email);

        var result = await _mediatorSender.Send(new CreateUserCommand
        {
            UserId = context.Message.UserId,
            Email = context.Message.Email,
            DisplayLanguage = context.Message.CultureCode,
            PhoneCountryCode = context.Message.PhoneCountryCode,
            PhoneNumber = context.Message.PhoneNumber,
            FirstName = context.Message.FirstName,
            LastName = context.Message.LastName
        });

        if (result.IsFailed)
        {
            _logger.LogError("CreateUserCommand error: {Errors}", result.Errors);

            switch (result.Errors.First())
            {
                case ValidationError error:
                    throw new ValidationException("One or more fields failed validation", error.Errors);
                case BaseError error:
                    throw new CreateUserCommandException(error.Title, error.Message);
            }
        }

        _logger.LogInformation("Identity for user {Email} has been processed successfully", context.Message.Email);
    }
}