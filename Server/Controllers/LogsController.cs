using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Server.DatabaseTables;
using Server.ParamClasses;
using Server.Validator;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogsController : Controller
{
    private readonly MyContext context = new MyContext();
    private Validators validation = new Validators();

    // GET: api/groups?limit=25&offset=50&orderBy=Id&isAscending=false
    [HttpGet]
    public IActionResult Get(int limit = 10, int offset = 0)
    {
        //int limit = 10, int offset = 0, string orderBy = "empty", bool isAscending = true
        string orderBy = "empty"; bool isAscending = true;
        string sql = "SELECT * FROM `Log`";

        var tables = new List<string> { "id", "message", "time" };
        var direction = isAscending ? "ASC" : "DESC";

        if (tables.Contains(orderBy.ToLower())) //hope this is enough to stop sql injection
        {
            sql += $" ORDER BY `{orderBy}` {direction}";
        }

        List<Log> query = context.Log.FromSqlRaw(sql + " LIMIT {0} OFFSET {1}", limit, offset).ToList();

        if (query == null || query.Count == 0)
        {
            return NoContent(); //204
        }

        return Ok(query); //200
    } //&orderBy  => is required (idk how to make it optimal)

    [HttpGet("{Id}")]
    public ActionResult<Log> Get(int Id)
    {
        Log Exlog = context.Log.Find(Id);

        if (Exlog == null)
            return NotFound("Object does not exists");

        return Ok(Exlog);
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
        try
        {
            validation.DateTimeValidator(log.Time.ToString());
        }
        catch (Exception)
        {
            return NotFound("Invalid");
        }

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

    [HttpPut("{Id}")]
    public ActionResult Put(int Id, LogDto log)
    {
        try
        {
            validation.DateTimeValidator(log.Time.ToString());
        }
        catch (Exception)
        {
            return NotFound("Invalid");
        }

        Log Exlog = context.Log.Find(Id);

        if (Exlog == null)
            return NotFound("Object does not exists");

        Exlog.Id_Job = log.Id_Job;
        Exlog.Time = log.Time;
        Exlog.Message = log.Message;

        context.SaveChanges();
        return Ok();
    }
}
