using FluentValidation;

namespace Domivice.Users.Application.Users.Queries.GetUsers;

public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(query => query.Page).GreaterThan(0);
    }
}