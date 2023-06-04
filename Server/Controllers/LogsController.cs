using Microsoft.AspNetCore.Mvc;
using Server.DatabaseTables;
using Server.ParamClasses;
using Server.Validator;

namespace Server.Controllers;

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

    [Authorize]
    [HttpGet]
    public ActionResult<List<WebLogDto>> Get()
    {
        return Ok(context.Log.Select(x => new WebLogDto(x)).ToList());
    }

    [Authorize]
    [HttpGet("{Id}")]
    public async Task<ActionResult<WebLogDto>> Get(int Id)
    {
        var log = await context.Log.FindAsync(Id);

        if (log == null)
            return NotFound();

        return Ok(new WebLogDto(log));
    }

    [Authorize]
    [HttpGet("Job/{IdJob}")]
    public ActionResult<List<WebLogDto>> GetLogs (int IdJob)
    {
        List<WebLogDto> logDtos = context.Log.Where(x => x.Id_Job == IdJob).Select(x => new WebLogDto(x)).ToList();

        if (logDtos.Count == 0)
            return NotFound();

        return Ok(logDtos);
    }

    [HttpPost("Add")]
    public async Task<ActionResult> Post([FromBody]WebLogDto log)
    {
        await context.Log.AddAsync(log.GetLog());

        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{Id}")]
    public async Task<ActionResult> Put(int Id, WebLogDto log)
    {
        try
        {
            validation.DateTimeValidator(log.Time.ToString());
        }
        catch (Exception)
        {
            return BadRequest("Invalid");
        }

        var Exlog = await context.Log.FindAsync(Id);

        if (Exlog == null)
            return NotFound("Object does not exists");

        Exlog.UpdateData(log.GetLog());

        await context.SaveChangesAsync();
        return Ok();
    }
}
