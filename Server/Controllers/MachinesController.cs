using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.ParamClasses;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MachinesController : Controller
{
    private readonly MyContext context = new MyContext();

    // GET: api/jobs?limit=25&offset=50&orderBy=Id&isAscending=false   => UI datagrid                   
    [HttpGet]
    public IActionResult Get(int limit = 10, int offset = 0, string orderBy = null, bool isAscending = true)
    {
        List<Machine> query;
        if (orderBy != null)
        {
            query = isAscending ?
                   context.Machine.OrderBy(s => s.GetType().GetProperty(orderBy).GetValue(s)).ToList() :
                   context.Machine.OrderByDescending(s => s.GetType().GetProperty(orderBy).GetValue(s)).ToList();
            query = query
                    .Skip(offset)
                    .Take(limit)
                    .ToList();
        }
        else
        {
            query = context.Machine
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
    public IActionResult GetCount(bool active = true)
    {
        return Ok(context.Machine.Where(x => x.Is_active == active).Count());
    }

    [HttpGet("{Id}")]
    public IActionResult Get(int id)
    {
        try
        {
            return Ok(context.Machine.Find(id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving the jobs: {ex.Message}");
        }
    }

    [HttpGet("{Id}/jobs")]
    public IActionResult GetJobs(int id)
    {
        try
        {
            return Ok(context.Job.Where(x => x.Id_Machine == id).ToList());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving the jobs: {ex.Message}");
        }
    }
    //TODO: add by Status

    [HttpPost]
    public void Post([FromBody] MachineDto machine)
    {
        Machine NewMachine = new Machine()
        {
            Name = machine.Name,
            Description = machine.Description,
            Os = machine.Os,
            Ip_address = machine.Ip_Adress,
            Mac_address = machine.Mac_Adress,
            Is_active = machine.Is_Active
        };

        context.Machine.Add(NewMachine);
        context.SaveChanges();
    }

    [HttpPut("{Id}")]
    public void Put(int id, [FromBody] Machine machine)
    {
        Machine result = context.Machine.Find(id);
        if (machine.Name != "string")                  //what is this?
            result.Name = machine.Name;
        if (machine.Description != "string")
            result.Description = machine.Description;
        if (machine.Os != "string")
            result.Os = machine.Os;
        if (machine.Ip_address != "string")
            result.Ip_address = machine.Ip_address;
        if (machine.Mac_address != "string")
            result.Mac_address = machine.Mac_address;

        result.Is_active = machine.Is_active;

        context.SaveChanges();
    }
}
