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

    // GET: api/logs?limit=25&offset=50&orderBy=Id&isAscending=false   => UI datagrid                   
    [HttpGet]
    public IActionResult Get(int limit = 10, int offset = 0, string orderBy = null, bool isAscending = true)
    {
        List<Log> query;
        if (orderBy != null)
        {
            query = isAscending ?
                   context.Log.OrderBy(s => s.GetType().GetProperty(orderBy).GetValue(s)).ToList() :
                   context.Log.OrderByDescending(s => s.GetType().GetProperty(orderBy).GetValue(s)).ToList();
            query = query
                    .Skip(offset)
                    .Take(limit)
                    .ToList();
        }
        else
        {
            query = context.Log
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

    [HttpGet("{Id}")]
    public Log Get(int id)
    {
        return context.Log.Find(id);
    }

    [HttpPost]
    public void Post(LogDto log)
    {
        Log newLog = new Log()
        {
            Message = log.Message,
            Time = log.Time,
            Id_Job = log.Id_Job
        };

        context.Log.Add(newLog);
        context.SaveChanges();
    }
}
