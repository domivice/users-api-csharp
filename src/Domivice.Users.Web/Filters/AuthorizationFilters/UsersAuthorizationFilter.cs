using System;
using System.Security.Claims;
using Domivice.Users.Application.Users.Queries.GetUser;
using Domivice.Users.Web.Constants;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Domivice.Users.Web.Filters.AuthorizationFilters;

/// <summary>
/// 
/// </summary>
public class UserAuthorizationFilter : IAuthorizationFilter
{
    private readonly ISender _mediatorSender;
    private const string UserIdRouteKey = "userId";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    public UserAuthorizationFilter(ISender sender)
    {
        _mediatorSender = sender;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var parsedContext = FilterContextParser.Parse(context);

        if (!Guid.TryParse(parsedContext.GetRouteValue(UserIdRouteKey), out var routeUserId))
        {
            context.Result = new NotFoundResult();
            return;
        }

        var userResult = _mediatorSender.Send(new GetUserQuery(routeUserId.ToString())).GetAwaiter().GetResult();

        if (userResult.IsFailed)
        {
            context.Result = new NotFoundResult();
            return;
        }

        if (parsedContext.UserHasRole(UserRoles.AppAdmin))
        {
            return;
        }

        var userIdClaim = parsedContext.GetUserClaim(ClaimTypes.NameIdentifier);

        if (userResult.Value.Id.ToString() == userIdClaim)
        {
            return;
        }

        context.Result = new ForbidResult();
    }
}