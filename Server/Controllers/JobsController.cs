using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.ParamClasses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class JobsController : ControllerBase
{
    private readonly MyContext context = new MyContext();

    [HttpPost]
    public void PostJob([FromBody] JobAdminDto job)
    {
        Job NewJob = new Job()
        {
            id_Config = job.Id_Config,
            id_Group = job.Id_Group,
            id_Machine = job.Id_Machine,
            status = job.Status,
            time_schedule = job.Time_Schedule,
            Bytes = job.Bytes
        };

        context.Job.Add(NewJob);
        context.SaveChanges();
    }

    [HttpPut("{id}")]
    public void PostJob(int id, [FromBody] Job job)
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

    [HttpGet("{id}")]
    public Job GetJob(int id)
    {
        return context.Job.Find(id);
    }

    [HttpGet("machines/{id}")]
    public List<Job> MachineGetIdMachine(int id)
    {
        return context.Job.Where(x => x.id_Machine == id).ToList();
    }

    [HttpGet("groups/{id}")]
    public List<Job> MachineGetIdGroup(int id)
    {
        return context.Job.Where(x => x.id_Group == id).ToList();
    }

    [HttpGet("configs/{id}")]
    public List<Job> MachineGetIdConfig(int id)
    {
        return context.Job.Where(x => x.id == id).ToList();
    }

    [HttpGet("ipAddress/{id}")]
    public Job GetJob(string id)
    {
        return context.Job.Include(x => x.Machine).Include(x => x.Config).Where(x => x.Machine.Ip_address == id && x.status == 0).FirstOrDefault();
    }

    [HttpGet("SourcePath/id_config")]
    public List<Sources> SourcePathGetIdConfig(int idConfig)
    {
        List<Sources> sources = context.Sources.Where(x => x.id_Config == idConfig).ToList();
        return sources;
    }

    [HttpGet("DestPath/id_config")]
    public List<Destination> DestPathGetIdConfig(int idConfig)
    {
        List<Destination> destinations = context.Destination.Where(x => x.id_Config == idConfig).ToList();
        return destinations;
    }


    [HttpPut("time_end/{id}")]
    public void JobPutEnd_time(int id, Job job)
    {
        Job result = context.Job.Find(id);

        result.time_end = job.time_end;
        result.status = job.status;

        context.SaveChanges();
    }

    [HttpPost("Log/New/")]
    public void LogPostNew(LogDto log)
    {
        Log newLog = new Log()
        {
            message = log.Message,
            time = log.Time,
            id_Job = log.Id_Job
        };

        context.Log.Add(newLog);
        context.SaveChanges();
    }
}

// include only Jobs, for other data create own controller