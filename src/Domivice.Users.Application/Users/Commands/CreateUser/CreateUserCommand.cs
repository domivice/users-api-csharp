using System.Text.Json;
using Domivice.Domain.ValueObjects;
using Domivice.Users.Application.Common.Interfaces;
using Domivice.Users.Application.Users.ReadModels;
using Domivice.Users.Domain.Entities;
using Domivice.Users.Domain.Errors;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domivice.Users.Application.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<Result<UserRm>>
{
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneCountryCode { get; set; }
    public string PhoneNumber { get; set; }
    public string DisplayLanguage { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserRm>>
{
    private readonly ILogger<CreateUserCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(ILogger<CreateUserCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserRm>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling command {Action} with request: {Request}",
            nameof(CreateUserCommandHandler),
            JsonSerializer.Serialize(request)
        );
        
        var user = new User(
            Guid.Parse(request.UserId),
            FirstName.Create(request.FirstName).Value,
            LastName.Create(request.LastName).Value,
            Email.Create(request.Email).Value,
            PhoneNumber.Create(request.PhoneNumber, request.PhoneCountryCode).Value,
            CultureCode.Create(request.DisplayLanguage).Value
        );
        
        _unitOfWork.UserRepository.Insert(user);
        var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        if (saveResult == 0)
            return Result.Fail(new InternalServerError
            {
                Title = "Error inserting user",
                Message = "An error occured while inserting the user. Please try again"
            });
        
        return (UserRm) user;
    }
}