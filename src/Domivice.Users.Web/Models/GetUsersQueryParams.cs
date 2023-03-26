using Domivice.PagingSorting.Web.Attributes;
using Domivice.PagingSorting.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Domivice.Users.Web.Models;

/// <summary>
/// 
/// </summary>
public class GetUsersQueryParams : BaseQuery
{
    /// <summary>
    ///     The search term. When provided, a search will be carried on certain user fields. See GetUsersSpecs.cs
    /// </summary>
    [FromQuery(Name = "search")]
    public string Search { get; set; }

    /// <summary>
    ///     Not a query parameter. This field is just present to enable sorting by firstName
    /// </summary>
    [SortableField(Name = "firstName", DbName = nameof(Domain.Entities.User.FirstName))]
    public string FirstName => null;

    /// <summary>
    ///     Not a query parameter. This field is just present to enable sorting by lastName
    /// </summary>
    [SortableField(Name = "lastName", DbName = nameof(Domain.Entities.User.LastName))]
    public string LastName => null;

    /// <summary>
    ///     Not a query parameter. This field is just present to enable sorting by phoneNumber
    /// </summary>
    [SortableField(
        Name = "phoneNumber",
        DbName = $"{nameof(Domain.Entities.User.PhoneNumber)}.{nameof(Domain.Entities.User.PhoneNumber.Number)}")
    ]
    public string PhoneNumber => null;

    /// <summary>
    ///     Not a query parameter. This field is just present to enable sorting by email
    /// </summary>
    [SortableField(Name = "email", DbName = nameof(Domain.Entities.User.Email))]
    public string Email => null;
}