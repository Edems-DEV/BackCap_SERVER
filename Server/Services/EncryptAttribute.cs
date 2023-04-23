using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Server.Services;

public class EncryptAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            string token = context.HttpContext.Request.Headers["Authorization"].ToString().Split(' ').Last();

            var json = JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret("super-secret-foobar")
                .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                .AddClaim("unencrypted string", token)
                .Encode();
        }
        catch
        {
            context.Result = new JsonResult(new { message = "Invalid token" })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }
}
