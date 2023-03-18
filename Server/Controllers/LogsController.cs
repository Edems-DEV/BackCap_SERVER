using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.ParamClasses;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogsController : Controller
{
    private readonly MyContext context = new MyContext();

    // GET: api/Logs?limit=25&offset=50&orderBy=id&orderDirection=desc   => UI datagrid                   
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

    [HttpGet("{id}")]
    public Log Get(int id)
    {
        return context.Log.Find(id);
    }

    [HttpPost]
    public void Post(LogDto log)
    {
        Log newLog = new Log()
        {
            message = log.Message,
            time = log.Time,
            id_Job = log.Id_Job
        };

        context.Log.Add(newLog);
        context.SaveChanges();
    }
}
