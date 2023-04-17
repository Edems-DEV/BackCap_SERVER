using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Entities;

[Route("api/[controller]")]
[ApiController]
public class SessionsController : ControllerBase
{
    private AuthenticationService auth = new AuthenticationService();

    [HttpPost]
    public JsonResult Login(Credentials credentials)
    {
        try
        {
            return new JsonResult(this.auth.Authenticate(credentials));
        }
        catch
        {
            return new JsonResult("Invalid username or password") { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
