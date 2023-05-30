using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.Validator;
using Server.Dtos;
using Server.Services;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobsController : Controller
{
    private readonly Validators validation;
    private readonly MyContext context = new MyContext();

    public JobsController(Validators validation)
    {
        this.validation = validation;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        return Ok(context.Job.ToList().Select(x => new WebJobDto(x, context)).ToList()); 
    }

    [HttpGet("count/{command}")]
    [Authorize]
    public async Task<int> GetCount(string command)
    {
        int count = 0;
        switch (command.ToLower())
        {
            case "all":
                await context.Job.ForEachAsync(x => { count++; });
                    break;

            case "running":
                await context.Job.Where(x => x.Status == 1).ForEachAsync(x => { count++; });
                break;

            case "waiting":
                await context.Job.Where(x => x.Status == 2).ForEachAsync(x => { count++; });
                break;

            case "succesfull":
                await context.Job.Where(x => x.Status == 3).ForEachAsync(x => { count++; });
                break;

            case "warning":
                await context.Job.Where(x => x.Status == 4).ForEachAsync(x => { count++; });
                break;

            case "failed":
                await context.Job.Where(x => x.Status == 5).ForEachAsync(x => { count++; });
                break;

        }
        return count;
    }

    [HttpGet("Id/Admin")]
    [Authorize]
    public async Task<ActionResult<WebJobDto>> GetJob(int Id)
    {
        var job = await context.Job.FindAsync(Id);

        if (job == null)
            return NotFound("Object does not exists");

        return Ok(new WebJobDto(job, context));
    }



    // daemon

    // GET: api/jobs/5   => specific job info
    [HttpGet("{Id}/Daemon")] // pro daemona neměnit
    public async Task<ActionResult<Job>> Get(int Id)
    {
        var machine = await context.Machine.FindAsync(Id);

        if (machine.Is_Active == false)
            return BadRequest("UnAuthorized");

        Job? job = context.Job.Where(x => x.Id_Machine == Id).FirstOrDefault();

        if (job == null)
            return NoContent();

        HelpMethods helpMethods = new HelpMethods(context);
        return Ok(helpMethods.AddToJob(job));
    }

    [HttpPut("{Id}/StartTimeStatus")]
    public async Task<ActionResult> TimeStartUpdate(int id, [FromBody] DaemonTimeStartStatus job)
    {
        var existingJob = await context.Job.FindAsync(id);

        if (existingJob == null)
            return NotFound();

        existingJob.Status = job.Status;
        existingJob.Time_start = job.Time_start;

        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{Id}/EndTimeStatus")]
    public async Task<ActionResult> TimeEndUpdate(int Id, [FromBody] DaemonTimeEndStatus job)
    {
        var existingJob = await context.Job.FindAsync(Id);

        if (existingJob == null)
            return NotFound();

        existingJob.Status = job.Status;
        existingJob.Time_end = job.Time_End;

        if (job.Bytes != null)
            existingJob.Bytes = job.Bytes;

        await context.SaveChangesAsync();
        return Ok();
    }
}