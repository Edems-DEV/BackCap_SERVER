using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.Dtos;
using Server.ParamClasses;
using Server.Services;
using Server.Validator;

namespace Server.Controllers;

[Authorize]
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
    public ActionResult<List<WebUserNoPass>> GetUsers()
    {
        return Ok(context.User.Select(x => new WebUserNoPass(x)).ToList());
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        return Ok(context.User.CountAsync());
    }

    [HttpGet("{Id}")]
    public async Task<ActionResult<WebUserNoPass>> Get(int Id)
    {
        var user = await context.User.FindAsync(Id);

        if (user == null)
            return NotFound();

        return Ok(new WebUserNoPass(user));
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] WebUserDto user, [FromServices] MailManager mail)
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

        await context.User.AddAsync(NewUser);
        await context.SaveChangesAsync();

        mail.AddUser(NewUser);

        return Ok();
    }

    [HttpPut("{Id}")]
    public async Task<ActionResult> Put(int Id, [FromBody] WebUserDto webUser, [FromServices] MailManager mail)
    {
        var user = await context.User.FindAsync(Id);
        string interval = user.Interval_Report;

        try
        {
            validation.EmailValidator(webUser.Email.ToString());

            if (user == null)
                return NotFound();
        }
        catch (Exception)
        {
            return BadRequest();
        }

        user.UpdateUser(webUser);
        await context.SaveChangesAsync();

        if (webUser.Interval_Report == interval)
            return Ok();

        mail.RemoveUser(user);
        mail.AddUser(user);


        return Ok();
    }

    [HttpDelete("{Id}")]
    public async Task<ActionResult> Delete(int Id, [FromServices] MailManager mail)
    {
        var user = await context.User.FindAsync(Id);

        if (user == null)
        {
            return NotFound();
        }

        context.User.Remove(user);
        mail.RemoveUser(user);
        await context.SaveChangesAsync();


        return Ok();
    }
}