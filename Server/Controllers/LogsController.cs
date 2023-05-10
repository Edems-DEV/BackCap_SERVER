using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using Server.DatabaseTables;
using Server.ParamClasses;
using Server.Validator;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogsController : Controller
{
    private readonly Validators validation;
    private readonly MyContext context;

    public LogsController(MyContext context, Validators validation)
    {
        this.context = context;
        this.validation = validation;
    }

    // GET: api/groups?limit=25&offset=50&orderBy=Id&isAscending=false
    [HttpGet]
    public ActionResult<List<WebLogDto>> Get(int limit = 10, int offset = 0)
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

        List<Log> query = context.Log.FromSqlRaw(sql).ToList();// + " LIMIT {0} OFFSET {1}", limit, offset

        if (query == null || query.Count == 0)
        {
            return NoContent(); //204
        }

        List<WebLogDto> logDtos = new();
        foreach (var log in query) 
        {
            logDtos.Add(new WebLogDto(log.Id, log.Id_Job, log.Message, log.Time));
        }  

        return Ok(logDtos); //200
    } //&orderBy  => is required (idk how to make it optimal)

    [HttpGet("{Id}")]
    public ActionResult<WebLogDto> Get(int Id)
    {
        Log log = context.Log.Find(Id);

        if (log == null)
            return NotFound("Object does not exists");

        return Ok(new WebLogDto(log.Id, log.Id_Job, log.Message, log.Time));
    }

    [HttpGet("Job/{IdJob}")]
    public ActionResult<List<WebLogDto>> GetLogs (int IdJob)
    {
        List<WebLogDto> logDtos = new();
        foreach (Log log in context.Log.Where(x => x.Id_Job == IdJob).ToList())
        {
            logDtos.Add(new WebLogDto(log.Id, log.Id_Job, log.Message, log.Time));
        }

        if (logDtos.Count == 0)
            return NotFound($"There are no logs for job id {IdJob}");

        return Ok(logDtos);
    }

    [HttpPost]
    public ActionResult Post(List<Log> logs)
    {
        foreach (Log log in logs)
        {
            try
            {
                validation.DateTimeValidator(log.Time.ToString());
            }
            catch (Exception)
            {
                return BadRequest("Invalid");
            }

            context.Log.Add(log);
        }

        context.SaveChanges();
        return Ok();
    }

    [HttpPut("{Id}")]
    public ActionResult Put(int Id, WebLogDto log)
    {
        try
        {
            validation.DateTimeValidator(log.Time.ToString());
        }
        catch (Exception)
        {
            return BadRequest("Invalid");
        }

        Log Exlog = context.Log.Find(Id);

        if (Exlog == null)
            return NotFound("Object does not exists");

        Exlog.UpdateData(log.GetLog());

        context.SaveChanges();
        return Ok();
    }
}
