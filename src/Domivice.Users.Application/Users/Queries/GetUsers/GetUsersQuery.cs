using System.Text.Json;
using Domivice.PagingSorting.Domain.Enumerations;
using Domivice.PagingSorting.Domain.Models;
using Domivice.Users.Application.Common.Interfaces;
using Domivice.Users.Application.Users.ReadModels;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domivice.Users.Application.Users.Queries.GetUsers;

public record GetUsersQuery : IRequest<Result<PaginatedList<BasicUserRm>>>
{
    public string? Search { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public Dictionary<string, SortOrder>? SortFields { get; set; }
}

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<PaginatedList<BasicUserRm>>>
{
    private readonly ILogger<GetUsersQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetUsersQueryHandler(ILogger<GetUsersQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginatedList<BasicUserRm>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling command {Action} with request: {Request}",
            nameof(GetUsersQueryHandler),
            JsonSerializer.Serialize(request)
        );

        var specs = new GetUsersSpecs(request);
        var userList = await _unitOfWork.UserRepository.GetListAsync(specs, cancellationToken);
        var count = await _unitOfWork.UserRepository.GetCountAsync(specs, cancellationToken);

        return new PaginatedList<BasicUserRm>(userList.ConvertAll(u => (BasicUserRm)u), count);
    }
}