using FluentValidation;

namespace Domivice.Users.Application.Users.Queries.GetUser;

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(query => query.UserId).NotEmpty().NotNull().WithMessage("A user id is required to get a user.");
    }
}