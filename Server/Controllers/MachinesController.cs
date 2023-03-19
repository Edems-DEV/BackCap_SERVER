using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Server.DatabaseTables;
using Server.ParamClasses;
using System.Net.Mail;
using System.Xml.Linq;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MachinesController : Controller
{
    private readonly MyContext context = new MyContext();

    //// GET: api/jobs?limit=25&offset=50&orderBy=Id&isAscending=false   => UI datagrid                   
    //[HttpGet]
    //public IActionResult Get(int limit = 10, int offset = 0, string orderBy = null, bool isAscending = true)
    //{
    //    List<Machine> query;
    //    if (orderBy != null)
    //    {
    //        query = isAscending ?
    //               context.Machine.OrderBy(s => s.GetType().GetProperty(orderBy).GetValue(s)).ToList() :
    //               context.Machine.OrderByDescending(s => s.GetType().GetProperty(orderBy).GetValue(s)).ToList();
    //        query = query
    //                .Skip(offset)
    //                .Take(limit)
    //                .ToList();
    //    }
    //    else
    //    {
    //        query = context.Machine
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
    public ActionResult GetCount(bool active = true)
    {
        return Ok(context.Machine.Where(x => x.Is_Active == active).Count());
    }

    [HttpGet("{Id}")]
    public ActionResult<Machine> Get(int id)
    {
        try
        {
            return Ok(context.Machine.Find(id));
        }
        catch (MySqlException ex)
        {
            return NotFound("Object does not exists");
        }
    }

    [HttpGet("{Id}/jobs")]
    public ActionResult<List<Job>> GetJobs(int Id)
    {
        List<Job> jobs = context.Job.Where(x => x.Id_Machine == Id).ToList();

        if (jobs.Count == 0)
            return NotFound($"There are no job for machine id {Id}");

        return Ok(jobs);

    }
    //TODO: add by Status

    [HttpPost]
    public ActionResult Post([FromBody] MachineDto machine)
    {
        Machine NewMachine = new Machine()
        {
            Name = machine.Name,
            Description = machine.Description,
            Os = machine.Os,
            Ip_Address = machine.Ip_Address,
            Mac_Address = machine.Mac_Address,
            Is_Active = machine.Is_Active
        };

        context.Machine.Add(NewMachine);
        context.SaveChanges();

        return Ok();
    }

    [HttpPut("{Id}")]
    public ActionResult Put(int Id, [FromBody] MachineDto machine)
    {
        Machine ExistingMachine;
        try
        {
            ExistingMachine = context.Machine.Find(Id);
        }
        catch (MySqlException ex)
        {
            return NotFound("Object does not exists");
        }

        ExistingMachine.Name = machine.Name;
        ExistingMachine.Description = machine.Description;
        ExistingMachine.Os = machine.Os;
        ExistingMachine.Ip_Address = machine.Ip_Address;
        ExistingMachine.Mac_Address = machine.Mac_Address;
        ExistingMachine.Is_Active = machine.Is_Active;

        context.SaveChanges();

        return Ok();
    }
}
