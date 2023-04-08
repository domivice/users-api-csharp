using Domivice.Users.Web.Filters.AuthorizationFilters;
using Microsoft.AspNetCore.Mvc;

namespace Domivice.Users.Web.Attributes;

/// <summary>
/// 
/// </summary>
public class AuthorizeUserAttribute : TypeFilterAttribute
{
    /// <summary>
    /// 
    /// </summary>
    public AuthorizeUserAttribute() : base(typeof(UserAuthorizationFilter))
    {
    }
}