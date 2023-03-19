using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.ParamClasses;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly MyContext context = new MyContext();

    // GET: api/users?limit=25&offset=50&orderBy=Id&isAscending=false
    [HttpGet]
    public IActionResult Get(int limit = 10, int offset = 0, string orderBy = "empty", bool isAscending = true)
    {
        string sql = "SELECT * FROM `User`";

        var tables = new List<string> { "id", "name", "email", "interval_report" };
        var direction = isAscending ? "ASC" : "DESC";

        if (tables.Contains(orderBy.ToLower())) //hope this is enough to stop sql injection
        {
            sql += $" ORDER BY `{orderBy}` {direction}";
        }

        List<User> query = context.User.FromSqlRaw(sql + " LIMIT {0} OFFSET {1}", limit, offset).ToList();

        if (query == null || query.Count == 0)
        {
            return NoContent(); //204
        }

        return Ok(query); //200
    } //&orderBy  => is required (idk how to make it optimal)

    // GET: for stats
    [HttpGet("count")]
    public ActionResult<int> GetCount()
    {
        return Ok(context.User.Count());
    }

    // GET api/users/5
    [HttpGet("{Id}")]
    public ActionResult<User> Get(int Id)
    {
        User user = context.User.Find(Id);

        if (user == null)
            return NotFound("Object does not Exists");

        return Ok(user);
    }

    // POST api/users
    [HttpPost]
    public ActionResult Post([FromBody] UserDto user)
    {
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
    public ActionResult Put(int Id, [FromBody] UserDto user)
    {
        User ExUser = context.User.Find(Id);

        if (ExUser == null)
            return NotFound("Object does not exists");

        ExUser.Interval_Report = user.Interval_Report;
        ExUser.Email = user.Email;
        ExUser.Name = user.Name;
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
        return Ok($"Delete request received for user Id {Id}.");
    }
}