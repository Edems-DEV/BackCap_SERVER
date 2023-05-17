using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.Validator;
using Server.ParamClasses;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Server.Dtos;

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
    public IActionResult Get()
    {
        return Ok(context.Job.ToList().Select(x => new WebJobDto(x, context)).ToList()); 
    }

    [HttpGet("count/{command}")]
    public int GetCount(string command)
    {
        int count = 0;
        switch (command.ToLower())
        {
            case "all":
                context.Job.ForEachAsync(x => { count++; });
                    break;

            case "running":
                context.Job.Where(x => x.Status == 1).ForEachAsync(x => { count++; });
                break;

            case "waiting":
                context.Job.Where(x => x.Status == 2).ForEachAsync(x => { count++; });
                break;

            case "succesfull":
                context.Job.Where(x => x.Status == 3).ForEachAsync(x => { count++; });
                break;

            case "warning":
                context.Job.Where(x => x.Status == 4).ForEachAsync(x => { count++; });
                break;

            case "failed":
                context.Job.Where(x => x.Status == 5).ForEachAsync(x => { count++; });
                break;

        }
        return count;
    }

    [HttpGet("Id/Admin")]
    public ActionResult<WebJobDto> GetJob(int Id)
    {
        Job job = context.Job.Find(Id);

        context.Job.Remove(job);
        context.SaveChanges();

        if (job == null)
            return NotFound("Object does not exists");

        return Ok(new WebJobDto(job, context));
    }

    // daemon

    // GET: api/jobs/5   => specific job info
    [HttpGet("{Id}/Daemon")] // pro daemona neměnit
    public ActionResult<Job> Get(int Id)
    {
        Machine machine = context.Machine.Find(Id);
        if (machine.Is_Active == false)
            return BadRequest("UnAuthorized");

        Job job = context.Job.Where(x => x.Id_Machine == Id).FirstOrDefault()!;

        if (job == null)
            return NotFound("Object does not exists");

        HelpMethods helpMethods = new HelpMethods(context);
        job = helpMethods.AddToJob(job);

        return Ok(job);
    }

    // For deamon to update job Status
    [HttpPut("{Id}/StatusTime")]
    public ActionResult PutStatus(int id, [FromBody] DaemonJobStatusDto job)
    {
        try
        {
            validation.DateTimeValidator(job.Time_start.ToString());
            validation.DateTimeValidator(job.Time_end.ToString());
        }
        catch (Exception)
        {
            return BadRequest("Invalid");
        }

        Job existingJob = context.Job.Find(id);

        if (existingJob == null)
            return NotFound("Object does not exists");

        existingJob.Status = job.Status;

        if (job.Time_start != null)
            existingJob.Time_start = job.Time_start;

        if (job.Time_end != null)
            existingJob.Time_end = job.Time_end;

        if (job.Bytes != null)
            existingJob.Bytes = job.Bytes;

        context.SaveChanges();
        return Ok();
    }
}