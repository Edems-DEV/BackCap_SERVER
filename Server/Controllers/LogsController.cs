using Microsoft.AspNetCore.Mvc;
using Server.DatabaseTables;
using Server.ParamClasses;
using Server.Validator;

namespace Server.Controllers;

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class LogsController : Controller
{
    private readonly Validators validation;
    private readonly MyContext context = new MyContext();

    public LogsController(Validators validation)
    {
        this.validation = validation;
    }

    [HttpGet]
    public ActionResult<List<WebLogDto>> Get()
    {
        return Ok(context.Log.Select(x => new WebLogDto(x)).ToList());
    }

    [HttpGet("{Id}")]
    public ActionResult<WebLogDto> Get(int Id)
    {
        Log log = context.Log.Find(Id);

        if (log == null)
            return NotFound();

        return Ok(new WebLogDto(log));
    }

    [HttpGet("Job/{IdJob}")]
    public ActionResult<List<WebLogDto>> GetLogs (int IdJob)
    {
        List<WebLogDto> logDtos = context.Log.Where(x => x.Id_Job == IdJob).Select(x => new WebLogDto(x)).ToList();

        if (logDtos.Count == 0)
            return NotFound();

        return Ok(logDtos);
    }

    [HttpPost]
    public ActionResult Post(Log log)
    {
        context.Log.Add(log);

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
