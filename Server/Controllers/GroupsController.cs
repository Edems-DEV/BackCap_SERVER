using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.ParamClasses;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupsController : Controller
{
    private readonly MyContext context = new MyContext();

    // GET: api/Groups?limit=25&offset=50&orderBy=id&orderDirection=desc   => UI datagrid                   
    [HttpGet]
    public IActionResult Get(int limit = 10, int offset = 0) //string orderBy = "id", string orderDirection = "asc"
    {
        var machine = context.Machine
        .OrderBy(p => p.Id)
        .Skip(offset)
        .Take(limit)
        .ToList();

        if (machine == null || machine.Count == 0)
        {
            return NoContent(); //204
        }

        return Ok(machine); //200
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
}
