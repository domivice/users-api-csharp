using FluentResults;

namespace Domivice.Users.Domain.Errors;

public abstract class BaseError : IError
{
    public abstract string Title { get; set; }
    public abstract int StatusCode { get; }
    public abstract string Message { get; set; }
    public Dictionary<string, object> Metadata { get; set; }
    public List<IError> Reasons { get; set; }
}