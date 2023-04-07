using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Domivice.Users.Web.Filters;

/// <summary>
/// 
/// </summary>
public class FilterContextParser
{
    private readonly RouteData _routeData;
    private readonly HttpContext _httpContext;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    private FilterContextParser(AuthorizationFilterContext context)
    {
        _routeData = context.RouteData;
        _httpContext = context.HttpContext;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static FilterContextParser Parse(AuthorizationFilterContext context)
    {
        return new FilterContextParser(context);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="routeKey"></param>
    /// <returns></returns>
    public string GetRouteValue(string routeKey)
    {
        return _routeData.Values.GetValueOrDefault(routeKey)?.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="claimType"></param>
    /// <returns></returns>
    public string GetUserClaim(string claimType)
    {
        return _httpContext.User.Claims.FirstOrDefault(claim => claim.Type == claimType)?.Value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    public bool UserHasRole(string role)
    {
        return _httpContext.User.HasClaim(ClaimTypes.Role, role);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public bool DoesRequestContainPath(string path)
    {
        return _httpContext.Request.Path.ToString().Contains(path);
    }
}