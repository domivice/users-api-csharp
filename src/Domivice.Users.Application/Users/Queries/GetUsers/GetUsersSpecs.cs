using Domivice.Users.Application.Common.Specification;
using Domivice.Users.Domain.Entities;

namespace Domivice.Users.Application.Users.Queries.GetUsers;

public class GetUsersSpecs : BaseSpecification<User>
{
    public GetUsersSpecs(GetUsersQuery query) : base(u =>
        string.IsNullOrEmpty(query.Search)
        || Convert.ToString(u.FirstName).Contains(query.Search)
        || Convert.ToString(u.LastName).Contains(query.Search)
        || Convert.ToString(u.Email).Contains(query.Search)
        || Convert.ToString(u.PhoneNumber.Number).Contains(query.Search)
    )
    {
        SetPagination(query.Page, query.PageSize);

        if (query.SortFields is not null)
        {
            AddSorting(query.SortFields);
        }
    }
}