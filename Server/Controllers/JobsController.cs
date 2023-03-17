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

    // GET: api/jobs?limit=25&offset=50   => UI datagrid
    //                       &orderBy=id&orderDirection=desc
    [HttpGet]
    public IActionResult Get(int limit = 10, int offset = 0)
    {
        var jobs = context.Job
        //.Include(x => x.Group)
        //.OrderBy(p => p.Id)
        .Skip(offset)
        .Take(limit)
        .ToList();

        if (jobs == null || jobs.Count == 0)
        {
            return NoContent(); //204
        }

        return Ok(jobs); //200
    }

    // GET: api/jobs/5   => specific job info
    [HttpGet("{id}")]
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
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] Job job)
    {
        Job existingJob = context.Job.Find(id);

        existingJob.id_Config = job.id_Config;
        existingJob.id_Group = job.id_Group;
        existingJob.id_Machine = job.id_Machine;
        existingJob.status = job.status;
        existingJob.time_schedule = job.time_schedule;
        existingJob.Bytes = job.Bytes;

        context.SaveChanges();
    }

    // For deamon to update job status
    [HttpPut("{id}/status")]
    public void PutStatus(int id, [FromBody] Job job)
    {
        Job existingJob = context.Job.Find(id);

        existingJob.status = job.status;

        context.SaveChanges();
    }

    #region Why?
    [HttpPut("{id}/time_end")]
    public void JobPutEnd_time(int id, Job job)
    {
        Job result = context.Job.Find(id);

        result.time_end = job.time_end;
        result.status = job.status;

        context.SaveChanges();
    }

    [HttpGet("{id}/ipAddress")]
    public Job GetJob(string id)
    {
        return context.Job.Include(x => x.Machine).Include(x => x.Config).Where(x => x.Machine.Ip_address == id && x.status == 0).FirstOrDefault();
    }

    #endregion
}

// Job object is broken ForeignKey