using Microsoft.AspNetCore.Http;

namespace Domivice.Users.Domain.Errors;

public class ConflictError : BaseError
{
    public override string Message { get; set; }
    public override string Title { get; set; }
    public override int StatusCode { get; } = StatusCodes.Status409Conflict;
}