using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Services;

namespace Server.Controllers;
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    // optional attribute parameter - not used now
    public string Role { get; set; }

    private Services.AuthenticationService auth = new Services.AuthenticationService(); //fixed? Services.

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string token = context.HttpContext.Request.Headers["Authorization"].ToString().Split(' ').Last();

        if (!this.auth.VerifyToken(token))
        {
            context.Result = new JsonResult("authentication failed") { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}