using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Server.DatabaseTables;
using Server.ParamClasses;
using Server.Validator;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupsController : Controller
{
    private readonly MyContext context = new MyContext();
    private readonly Validators validators = new Validators();

    //// GET: api/groups?limit=25&offset=50&orderBy=Id&isAscending=false   => UI datagrid                   
    //[HttpGet]
    //public IActionResult Get(int limit = 10, int offset = 0, string orderBy = null, bool isAscending = true)
    //{
    //    List<Groups> query;
    //    if (orderBy != null)
    //    {
    //        query = isAscending ?
    //               context.Groups.OrderBy          (s => s.GetType().GetProperty(orderBy).GetValue(s)).ToList():
    //               context.Groups.OrderByDescending(s => s.GetType().GetProperty(orderBy).GetValue(s)).ToList();
    //        query = query
    //                .Skip(offset)
    //                .Take(limit)
    //                .ToList();
    //    }
    //    else
    //    {
    //        query = context.Groups
    //            .Skip(offset)
    //            .Take(limit)
    //            .ToList();
    //    }

    //    if (query == null || query.Count == 0)
    //    {
    //        return NoContent(); //204
    //    }

    //    return Ok(query); //200
    //}

    // GET: for stats
    [HttpGet("count")]
    public ActionResult<int> GetCount()
    {
        return Ok(context.Groups.Count());
    }

    [HttpGet("{Id}")]
    public ActionResult<Groups> Get(int Id)
    {
        Groups group = context.Groups.Find(Id);

        if (group == null)
            return NotFound("Object does not exists");

        return Ok(group);
    }

    [HttpPost]
    public ActionResult Post([FromBody] string name)
    {
        Groups NewGroup = new Groups();
        NewGroup.Name = name;

        context.Groups.Add(NewGroup);
        context.SaveChanges();

        return Ok();
    }

    [HttpPut("{Id}")]
    public ActionResult Put(int Id, [FromBody] string name)
    {
        Groups group = context.Groups.Find(Id);

        if (group == null)
            return NotFound("Object does not exists");

        group.Name = name;
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
