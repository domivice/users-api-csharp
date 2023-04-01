namespace Domivice.Users.Infrastructure.Consumers.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(IDictionary<string, string[]> errors)
    {
        Errors = errors;
    }

    public ValidationException(string? message, IDictionary<string, string[]> errors) : base(message)
    {
        Errors = errors;
    }

    public IDictionary<string, string[]> Errors { get; }
}