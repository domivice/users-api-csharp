using System.Text.Json;
using Domivice.Users.Application.Common.Interfaces;
using Domivice.Users.Domain.Errors;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domivice.Users.Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(string UserId) : IRequest<Result>;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly ILogger<DeleteUserCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(ILogger<DeleteUserCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling command {Action} with request: {Request}",
            nameof(DeleteUserCommandHandler),
            JsonSerializer.Serialize(request)
        );

        var user = await _unitOfWork.UserRepository.GetByIdAsync(Guid.Parse(request.UserId), cancellationToken);

        if (user is null)
            return Result.Fail(new NotFoundError
            {
                Title = "User not found",
                Message = "We could not find a user with the provided id. Please verify the user id and try again."
            });

        _unitOfWork.UserRepository.Delete(user);

        var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (saveResult == 0)
            return Result.Fail(new InternalServerError
            {
                Title = "Error deleting user",
                Message = "An error occured while deleting the user. Please try again"
            });

        return Result.Ok();
    }
}