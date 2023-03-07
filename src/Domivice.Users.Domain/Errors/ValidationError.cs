using FluentValidation.Results;

namespace Domivice.Users.Domain.Errors;

public class ValidationError : BadRequestError
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationError(IEnumerable<ValidationFailure> failures)
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }
}