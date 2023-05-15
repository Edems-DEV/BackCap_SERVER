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
    [Decrypt]
    public ActionResult<List<User>> GetUsers()
    {
        List<WebUserNoPass> newUsers = new();
        foreach (User user in context.User.ToList())
        {
            newUsers.Add(new WebUserNoPass(user.Id, user.Name, user.Email, user.Interval_Report));
        }

        return Ok(newUsers);
    }

    // GET: for stats
    [HttpGet("count")]
    [Decrypt]
    public ActionResult<int> GetCount()
    {
        return Ok(context.User.Count());
    }

    // GET api/users/5
    [HttpGet("{Id}")]
    [Decrypt]
    public ActionResult<WebUserNoPass> Get(int Id)
    {
        User user = context.User.Find(Id);

        if (user == null)
            return NotFound("Object does not Exists");

        return Ok(new WebUserNoPass(user.Id, user.Name, user.Email, user.Interval_Report));
    }

    // POST api/users
    [HttpPost]
    [Encrypt]
    public ActionResult Post([FromBody] WebUserDto user)
    {

        try
        {
            validation.EmailValidator(user.Email.ToString());
        }
        catch (Exception)
        {

            return NotFound("Invalid");
        }

        User NewUser = new User()
        {
            Interval_Report = user.Interval_Report,
            Email = user.Email,
            Password = user.Password,
            Name = user.Name
        };

        context.User.Add(NewUser);
        context.SaveChanges();

        return Ok();
    }

    // PUT api/users/5
    [HttpPut("{Id}")]
    [Encrypt]
    public ActionResult Put(int Id, [FromBody] WebUserDto user)
    {
        try
        {
            validation.EmailValidator(user.Email.ToString());
        }
        catch (Exception)
        {
            return NotFound("Invalid");
        }

        User ExUser = context.User.Find(Id);

        if (ExUser == null)
            return NotFound("Object does not exists");

        ExUser.Interval_Report = user.Interval_Report;
        ExUser.Email = user.Email;
        ExUser.Name = user.Name;
        if (user.Password != "" && user.Password != null)
            ExUser.Password = user.Password;

        context.SaveChanges();
        
        return Ok();
    }

    // DELETE api/users/5
    [HttpDelete("{Id}")]
    public ActionResult Delete(int Id)
    {
        var user = context.User.Find(Id);
        if (user == null)
        {
            return NotFound();
        }
        context.User.Remove(user);
        context.SaveChanges();
        return Ok(); //$"Delete request received for user Id {Id}."
    }
}