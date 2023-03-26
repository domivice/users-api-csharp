using System.Text.Json;
using Domivice.Users.Application.Common.Interfaces;
using Domivice.Users.Application.Users.ReadModels;
using Domivice.Users.Domain.Errors;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domivice.Users.Application.Users.Queries.GetUser;

public record GetUserQuery(string UserId) : IRequest<Result<UserRm>>;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserRm>>
{
    private readonly ILogger<GetUserQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetUserQueryHandler(ILogger<GetUserQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserRm>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling query {Action} with request: {Request}",
            nameof(GetUserQuery),
            JsonSerializer.Serialize(request)
        );
        
        var user = await _unitOfWork.UserRepository.GetByIdAsync(Guid.Parse(request.UserId), cancellationToken);

        if (user is null)
            return Result.Fail<UserRm>(new NotFoundError
            {
                Title = "User not found",
                Message = "We could not find a user with the provided id. Please verify the user id and try again."
            });

        return (UserRm) user;
    }
}