using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Server.DatabaseTables;
using Server.ParamClasses;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogsController : Controller
{
    private readonly MyContext context = new MyContext();

    // GET: api/logs?limit=25&offset=50&orderBy=Id&isAscending=false   => UI datagrid                   
    //[HttpGet]
    //public IActionResult Get(int limit = 10, int offset = 0, string orderBy = null, bool isAscending = true)
    //{
    //    List<Log> query;
    //    if (orderBy != null)
    //    {
    //        query = isAscending ?
    //               context.Log.OrderBy(s => s.GetType().GetProperty(orderBy).GetValue(s)).ToList() :
    //               context.Log.OrderByDescending(s => s.GetType().GetProperty(orderBy).GetValue(s)).ToList();
    //        query = query
    //                .Skip(offset)
    //                .Take(limit)
    //                .ToList();
    //    }
    //    else
    //    {
    //        query = context.Log
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

    [HttpGet("{Id}")]
    public ActionResult<Log> Get(int Id)
    {
        try
        {
            return Ok(context.Log.Find(Id));
        }
        catch (MySqlException ex)
        {
            return NotFound("Object does not exists");
        }
    }

    [HttpGet("Job/{IdJob}")]
    public ActionResult<List<Log>> GetLogs (int IdJob)
    {
        List<Log> logs = context.Log.Where(x => x.Id_Job == IdJob).ToList();

        if (logs.Count == 0)
            return NotFound($"There are no logs for job id {IdJob}");

        return Ok(logs);
    }

    [HttpPost]
    public ActionResult Post(LogDto log)
    {
        Log newLog = new Log()
        {
            Message = log.Message,
            Time = log.Time,
            Id_Job = log.Id_Job
        };

        context.Log.Add(newLog);
        context.SaveChanges();
        return Ok();
    }
}
