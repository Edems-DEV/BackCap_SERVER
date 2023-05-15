using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Server.DatabaseTables;
using Server.Dtos;
using Server.ParamClasses;
using Server.Validator;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupsController : Controller
{
    private readonly Validators validators;
    private readonly MyContext context = new MyContext();

    public GroupsController(Validators validators)
    {
        this.validators = validators;
    }

    // GET: api/groups?limit=25&offset=50&orderBy=Id&isAscending=false
    [HttpGet]
    public ActionResult Get(int limit = 10, int offset = 0)
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

        List<Groups> query = context.Groups.FromSqlRaw(sql).ToList();// + " LIMIT {0} OFFSET {1}", limit, offset

        if (query == null || query.Count == 0)
        {
            return NoContent(); //204
        }

        List<WebGroupDto> groupDtos = new();
        foreach (var group in query)
        {
            groupDtos.Add(new WebGroupDto(group.Id, group.Name, group.Description, context));
        }

        return Ok(groupDtos); //200
    } //&orderBy  => is required (idk how to make it optimal)

    [HttpGet("names")]
    public ActionResult<List<WebOthersDto>> GetNames()
    {
        List<WebOthersDto> names = new();

        foreach (var group in context.Groups.ToList())
        {
            names.Add(new WebOthersDto(group.Id, group.Name));
        }

        if (names.Count == 0)
        {
            return NoContent();
        }

        return Ok(names);
    }

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

        return Ok(new WebGroupDto(group.Id, group.Name, group.Description, context));
    }

    [HttpPost]
    public ActionResult Post([FromBody] WebGroupNew group)
    {
        context.Groups.Add(new Groups() { Name = group.Name, Description = group.Description});
        context.SaveChanges();

        return Ok();
    }

    [HttpPut("{Id}")]
    public ActionResult Put(int Id, [FromBody] WebGroupDto groupDto)
    {
        Groups group = context.Groups.Find(Id);

        if (group == null)
            return NotFound("Object does not exists");

        group = groupDto.UpdateGroup(group, context);

        context.SaveChanges();

        return Ok();
    }

    [HttpDelete("{Id}")]
    public ActionResult Delete(int Id)
    {
        Groups group = context.Groups.Find(Id);

        if (group == null)
            return NotFound("Object does not exists");

        context.Groups.Remove(group);
        context.SaveChanges();
        return Ok(); //$"Delete request received for group Id {Id}."
    }
}
