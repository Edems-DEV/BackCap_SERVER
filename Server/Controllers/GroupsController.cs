using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Server.DatabaseTables;
using Server.Dtos;
using Server.ParamClasses;
using Server.Validator;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupsController : Controller
{
    private readonly MyContext context = new MyContext();
    private readonly Validators validators = new Validators();

    // GET: api/groups?limit=25&offset=50&orderBy=Id&isAscending=false
    [HttpGet]
    public IActionResult Get(int limit = 10, int offset = 0)
    {
        //int limit = 10, int offset = 0, string orderBy = "empty", bool isAscending = true
        string orderBy = "empty"; bool isAscending = true;
        
        string sql = "SELECT * FROM `Groups`";

        var tables = new List<string> { "id", "name" };
        var direction = isAscending ? "ASC" : "DESC";

        if (tables.Contains(orderBy.ToLower())) //hope this is enough to stop sql injection
        {
            sql += $" ORDER BY `{orderBy}` {direction}";
        }

        List<Groups> query = context.Groups.FromSqlRaw(sql + " LIMIT {0} OFFSET {1}", limit, offset).ToList();

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
        return Ok(context.Groups.Count());
    }

    [HttpGet("{Id}")]
    public ActionResult<WebGroupDto> Get(int Id)
    {
        Groups group = context.Groups.Find(Id);

        if (group == null)
            return NotFound("Object does not exists");

        return Ok(new WebGroupDto(group.Id, group.Name, group.Description));
    }

    [HttpPost]
    public ActionResult Post([FromBody] string name /*[FromBody] string description*/) // předělat
    {
        Groups NewGroup = new Groups();
        NewGroup.Name = name;
        //NewGroup.Description = description;

        context.Groups.Add(NewGroup);
        context.SaveChanges();

        return Ok();
    }

    [HttpPut("{Id}")]
    public ActionResult Put(int Id, [FromBody] string name /*[FromBody] string description*/)
    {
        Groups group = context.Groups.Find(Id);

        if (group == null)
            return NotFound("Object does not exists");

        group.Name = name;
        //group.Description = description;
        context.SaveChanges();

        return Ok();
    }

    [HttpDelete("{Id}")]
    public IActionResult Delete(int Id)
    {
        Groups group = context.Groups.Find(Id);

        if (group == null)
            return NotFound("Object does not exists");

        context.Groups.Remove(group);
        context.SaveChanges();
        return Ok($"Delete request received for group Id {Id}.");
    }
}
