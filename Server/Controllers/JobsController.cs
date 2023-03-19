using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.ParamClasses;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobsController : Controller
{
    private readonly MyContext context = new MyContext();

    // GET: api/jobs?limit=25&offset=50&orderBy=Id&isAscending=false   => UI datagrid
    [HttpGet]
    public IActionResult Get(int limit = 10, int offset = 0, string orderBy = null, bool isAscending = true)
    {
        List<Job> query;
        if (orderBy != null)
        {
            query = isAscending ?
                   context.Job.OrderBy          (s => s.GetType().GetProperty(orderBy).GetValue(s)).ToList():
                   context.Job.OrderByDescending(s => s.GetType().GetProperty(orderBy).GetValue(s)).ToList();
            query = query
                    .Skip(offset)
                    .Take(limit)
                    .ToList();
        }
        else
        {
            query = context.Job
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

    // GET: for stats
    [HttpGet("count")]
    public IActionResult GetCount()
    {
        return Ok(context.Job.Count()); //idk if it works
    }

    // GET: api/jobs/5   => specific job info
    [HttpGet("{Id}")]
    public IActionResult Get(int id)
    {
        try
        {
            return Ok(context.Job.Find(id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving the jobs: {ex.Message}");
        }
    }

    // for deamon for final stats after completing job
    [HttpPut("{Id}")]
    public void Put(int id, [FromBody] Job job)
    {
        Job existingJob = context.Job.Find(id);

        existingJob.Id_Config = job.Id_Config;
        existingJob.Id_Group = job.Id_Group;
        existingJob.Id_Machine = job.Id_Machine;
        existingJob.Status = job.Status;
        existingJob.Time_schedule = job.Time_schedule;
        existingJob.Bytes = job.Bytes;

        context.SaveChanges();
    }

    // For deamon to update job Status
    [HttpPut("{Id}/Status")]
    public void PutStatus(int id, [FromBody] Job job)
    {
        Job existingJob = context.Job.Find(id);

        existingJob.Status = job.Status;

        context.SaveChanges();
    }

    #region Why?
    [HttpPut("{Id}/Time_end")]
    public void JobPutEnd_time(int id, Job job)
    {
        Job result = context.Job.Find(id);

        result.Time_end = job.Time_end;
        result.Status = job.Status;

        context.SaveChanges();
    }

    [HttpGet("{Id}/ipAddress")]
    public Job GetJob(string id)
    {
        return context.Job.Include(x => x.Machine).Include(x => x.Config).Where(x => x.Machine.Ip_address == id && x.Status == 0).FirstOrDefault();
    }

    #endregion
}

// Job object is broken ForeignKey