using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.DatabaseTables;
using Server.Dtos;
using Server.Entities;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SessionsController : ControllerBase
{
    string keySecret = "super-secret-foobar"; //your-256-bit-secret
    private MyContext context = new MyContext();

    // POST api/<SessionsController>
    //[HttpPost]
    //public IActionResult Post(WebLoginDto model)
    //{
    //    try
    //    {
    //        User login = this.context.User.Where(x => x.Email == model.Email).First();

    //        if (login.Password == model.Password)
    //        {
    //            string token = JwtBuilder.Create()
    //              .WithAlgorithm(new HMACSHA256Algorithm())
    //              .WithSecret(keySecret)
    //              .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds()) //AddSeconds(30) after this token will expire and will not anymore be valid
    //              .AddClaim("login", login.Email)
    //              .Encode();

    //            return Ok(token);
    //            //return Ok(new { token = token });
    //        }

    //        return Unauthorized(new { message = "Invalid credentials" });
    //    }
    //    catch
    //    {
    //        return Unauthorized(new { message = "Invalid credentials" });
    //    }
    //}

    [HttpPost]
    public JsonResult Post(WebLoginDto model)
    {
        try
        {
            User login = this.context.User.FirstOrDefault(x => x.Email == model.Email)
             ?? this.context.User.FirstOrDefault(x => x.Name == model.Email);

            if (login == null) { return new JsonResult("Invalid username or password") { StatusCode = StatusCodes.Status401Unauthorized }; }

            if (login.Password == model.Password)
            {
                string token = JwtBuilder.Create()
                  .WithAlgorithm(new HMACSHA256Algorithm())
                  .WithSecret(keySecret)
                  .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds()) //AddSeconds(30) after this token will expire and will not anymore be valid
                  .AddClaim("userId", login.Id)
                  .Encode();

                return new JsonResult(token);
            }
            return new JsonResult("Invalid username or password") { StatusCode = StatusCodes.Status401Unauthorized };
        }
        catch
        {
            return new JsonResult("Invalid username or password") { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }


    /*
    // DELETE api/<SessionsController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
    */
}
