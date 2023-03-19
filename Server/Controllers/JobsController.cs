using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.Validator;
using Server.ParamClasses;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobsController : Controller
{
    private readonly MyContext context = new MyContext();
    private Validators validation = new Validators();

    // GET: api/jobs?limit=25&offset=50&orderBy=Id&isAscending=false
    [HttpGet]
    public IActionResult Get(int limit = 10, int offset = 0, string orderBy = "empty", bool isAscending = true)
    {
        string sql = "SELECT * FROM `Job`";

        var tables = new List<string> { "id", "status", "time_schedule", "time_start", "time_end", "bytes" };
        var direction = isAscending ? "ASC" : "DESC";

        if (tables.Contains(orderBy.ToLower())) //hope this is enough to stop sql injection
        {
            sql += $" ORDER BY `{orderBy}` {direction}";
        }

        List<Job> query = context.Job.FromSqlRaw(sql + " LIMIT {0} OFFSET {1}", limit, offset).ToList();

        if (query == null || query.Count == 0)
        {
            return NoContent(); //204
        }

        return Ok(query); //200
    } //&orderBy  => is required (idk how to make it optimal)

    // GET: for stats
    [HttpGet("count")]
    public ActionResult<int> GetCount()
    {
        return Ok(context.Job.Count());
    }

    // GET: api/jobs/5   => specific job info
    [HttpGet("Id")]
    public ActionResult<Job> Get(int Id)
    {
        Job job = context.Job.Find(Id);

        if (job == null)
            return NotFound("Object does not exists");

        HelpMethods helpMethods = new HelpMethods(context);
        job = helpMethods.AddToJob(job);

        return Ok(job);
    }

    [HttpGet("{IpAddress}/Daemon")]
    public ActionResult<Job> Get(string IpAddress)
    {
        Job job = context.Job.Where(x => x.Machine.Ip_Address == IpAddress && x.Status == 0).FirstOrDefault();

        if (job == null)
            return NotFound("Object does not exists");

        job.Config = context.Config.Find(job.Id_Config);
        job.Config.Sources = context.Sources.Where(x => x.Id_Config == job.Id).ToList();
        job.Config.Destinations = context.Destination.Where(x => x.Id_Config == job.Id).ToList();

        return Ok(job);
    }

    [HttpGet("{IdMachine}/Machine")]
    public ActionResult<Job> GetJobIdMachine(int IdMachine)
    {
        List<Job> jobs = context.Job.Where(x => x.Id_Machine == IdMachine).ToList();

        if (jobs == null)
            return NotFound("Object does not exists");

        HelpMethods helpMethods = new HelpMethods(context);

        List<Job> fullJob = new();
        foreach (Job job in jobs)
        {
            fullJob.Add(helpMethods.AddToJob(job));
        }

        return Ok(fullJob);
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
            return NotFound("Invalid");
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
    public ActionResult PutStatus(int id, [FromBody] Job job)
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
            return NotFound("Invalid");
        }

        Job existingJob = context.Job.Find(id);

        if (existingJob == null)
            return NotFound("Object does not exists");

        existingJob.Status = job.Status;
        existingJob.Time_start = job.Time_start;
        existingJob.Time_end = job.Time_end;

        context.SaveChanges();
        return Ok();
    }
}