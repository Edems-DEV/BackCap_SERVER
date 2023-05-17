using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.Dtos;
using Server.ParamClasses;
using Server.Services;
using Server.Validator;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly Validators validation;
    private readonly MyContext context = new MyContext();

    public UsersController(Validators validation)
    {
        this.validation = validation;
    }

    [HttpGet]
    //[Decrypt]
    public ActionResult<List<WebUserNoPass>> GetUsers()
    {
        return Ok(context.User.Select(x => new WebUserNoPass(x)).ToListAsync());
    }

    [HttpGet("count")]
    //[Decrypt]
    public ActionResult<int> GetCount()
    {
        return Ok(context.User.Count());
    }

    [HttpGet("{Id}")]
    //[Decrypt]
    public ActionResult<WebUserNoPass> Get(int Id)
    {
        User user = context.User.Find(Id);

        if (user == null)
            return NotFound();

        return Ok(new WebUserNoPass(user));
    }

    [HttpPost]
    //[Encrypt]
    public ActionResult Post([FromBody] WebUserDto user)
    {
        try
        {
            validation.EmailValidator(user.Email.ToString());
        }
        catch (Exception)
        {
            return BadRequest();
        }

        User NewUser = new User(user);

        context.User.Add(NewUser);
        context.SaveChanges();

        return Ok();
    }

    [HttpPut("{Id}")]
    //[Encrypt]
    public ActionResult Put(int Id, [FromBody] WebUserDto webUser)
    {
        User user = context.User.Find(Id);

        try
        {
            validation.EmailValidator(webUser.Email.ToString());

            if (user == null)
                return NotFound();

            if (webUser.Password == "")
                return BadRequest();
        }
        catch (Exception)
        {
            return BadRequest();
        }

        user.UpdateUser(webUser);

        context.SaveChanges();
        
        return Ok();
    }

    [HttpDelete("{Id}")]
    public ActionResult Delete(int Id)
    {
        User user = context.User.Find(Id);

        if (user == null)
        {
            return NotFound();
        }

        context.User.Remove(user);
        context.SaveChanges();
        return Ok();
    }
}