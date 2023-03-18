using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using Server.DatabaseTables;
using Server.ParamClasses;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupsController : Controller
{
    private readonly MyContext context = new MyContext();

    // GET: api/groups?limit=25&offset=50&orderBy=id&isAscending=false   => UI datagrid                   
    [HttpGet]
    public IActionResult Get(int limit = 10, int offset = 0, string orderBy = null, bool isAscending = true)
    {
        List<Groups> query;
        if (orderBy != null)
        {
            query = isAscending ?
                   context.Groups.OrderBy          (s => s.GetType().GetProperty(orderBy).GetValue(s)).ToList():
                   context.Groups.OrderByDescending(s => s.GetType().GetProperty(orderBy).GetValue(s)).ToList();
            query = query
                    .Skip(offset)
                    .Take(limit)
                    .ToList();
        }
        else
        {
            query = context.Groups
                .Skip(offset)
                .Take(limit)
                .ToList();
        }

        if (query == null || query.Count == 0)
        {
            return NoContent(); //204
        }

        return Ok(query); //200
    }

    // GET: for stats
    [HttpGet("count")]
    public IActionResult GetCount()
    {
        return Ok(context.Groups.Count()); //idk if it works
    }

    [HttpGet("{id}")]
    public Groups Get(int id)
    {
        return context.Groups.Find(id);
    }

    [HttpPost]
    public void Post([FromBody] string name)
    {
        Groups NewGroup = new Groups();
        NewGroup.Name = name;

        context.Groups.Add(NewGroup);
        context.SaveChanges();
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string name)
    {
        Groups result = context.Groups.Find(id);
        result.Name = name;
        context.SaveChanges();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var group = context.Groups.Find(id);
        if (group == null)
        {
            return NotFound();
        }
        context.Groups.Remove(group);
        context.SaveChanges();
        return Ok($"Delete request received for group id {id}.");
    }
}
