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
    private readonly MyContext context = new MyContext();
    private Validators validation = new Validators();

    // GET: api/jobs?limit=25&offset=50&orderBy=Id&isAscending=false
    [HttpGet]
    public IActionResult Get(int limit = 10, int offset = 0)
    {
        //int limit = 10, int offset = 0, string orderBy = "empty", bool isAscending = true
        string orderBy = "empty"; bool isAscending = true;
        
        string sql = "SELECT * FROM `Job`";

        var tables = new List<string> { "id", "status", "time_schedule", "time_start", "time_end", "bytes" };
        var direction = isAscending ? "ASC" : "DESC";

        if (tables.Contains(orderBy.ToLower())) //hope this is enough to stop sql injection
        {
            sql += $" ORDER BY `{orderBy}` {direction}";
        }

        List<Job> query = context.Job.FromSqlRaw(sql).ToList();// + " LIMIT {0} OFFSET {1}", limit, offset

        if (query == null || query.Count == 0)
        {
            return NoContent(); //204
        }

        List<WebJobDto> jobDtos = new();
        foreach (var job in query)
        {
            jobDtos.Add(new WebJobDto(job.Id, job.Status, job.Time_start, job.Time_end, job.Time_schedule, job.Id_Group, job.Id_Machine, context));
        }

        return Ok(jobDtos); //200
    } //&orderBy  => is required (idk how to make it optimal)

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

            case "Warning":
                context.Job.Where(x => x.Status == 4).ForEachAsync(x => { count++; });
                break;

            case "Failed":
                context.Job.Where(x => x.Status == 5).ForEachAsync(x => { count++; });
                break;

        }
        return count;
    }

    // GET: api/jobs/5   => specific job info
    [HttpGet("Id/Daemon")] // pro daemona neměnit
    public ActionResult<Job> Get(int Id)
    {
        Job job = context.Job.Find(Id);

        if (job == null)
            return NotFound("Object does not exists");

        HelpMethods helpMethods = new HelpMethods(context);
        job = helpMethods.AddToJob(job);

        return Ok(job);
    }

    [HttpGet("Id/Admin")] // pro daemona neměnit
    public ActionResult<WebJobDto> GetJob(int Id)
    {
        Job job = context.Job.Find(Id);

        if (job == null)
            return NotFound("Object does not exists");

        return Ok(new WebJobDto(job.Id, job.Status, job.Time_start, job.Time_end, job.Time_schedule, job.Id_Group, job.Id_Machine, context));
    }

    // for deamon for final stats after completing job
    [HttpPut("{Id}")]
    public ActionResult Put(int id, [FromBody] Job job)
    {
        try
        {
            validation.DateTimeValidator(job.Time_schedule.ToString());
            validation.DateTimeValidator(job.Time_start.ToString());
            validation.DateTimeValidator(job.Time_end.ToString());
            validation.IpValidator(job.Machine.Ip_Address.ToString());
            validation.MacValidator(job.Machine.Mac_Address.ToString());
            validation.DateTimeValidator(job.Config.Interval_end.ToString());
        }
        catch (Exception)
        {
            return BadRequest("Invalid");
        }
        
        Job existingJob = context.Job.Find(id);

        if (existingJob == null)
            return NotFound("Object does not exists");

        existingJob.Id_Config = job.Id_Config;
        existingJob.Id_Group = job.Id_Group;
        existingJob.Id_Machine = job.Id_Machine;
        existingJob.Status = job.Status;
        existingJob.Time_schedule = job.Time_schedule;
        existingJob.Bytes = job.Bytes;

        context.SaveChanges();
        return Ok();
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