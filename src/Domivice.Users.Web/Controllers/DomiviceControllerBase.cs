using System.Linq;
using Domivice.Users.Domain.Errors;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Domivice.Users.Web.Controllers;

/// <summary>
/// </summary>
[ApiController]
public class DomiviceControllerBase : ControllerBase
{
    /// <summary>
    /// </summary>
    /// <param name="result"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected IActionResult Problem<T>(Result<T> result)
    {
        switch (result.Errors.First())
        {
            case ValidationError error:
            {
                var validationProblem = new ValidationProblemDetails(error.Errors)
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Instance = HttpContext.Request.Path.ToString()
                };

                return ValidationProblem(validationProblem);
            }
            case BaseError error:
                return Problem(
                    statusCode: error.StatusCode,
                    instance: HttpContext.Request.Path.ToString(),
                    title: error.Title,
                    detail: error.Message
                );
            default:
                return Problem();
        }
    }
}