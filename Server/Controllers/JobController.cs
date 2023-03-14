using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.ParamClasses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class JobController : ControllerBase
{
    private readonly MyContext context = new MyContext();

    [HttpPost("Job/post/new/")]
    public void JobPostNew([FromBody] JobAdminDto job)
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

    [HttpPut("Job/put/edit/")]
    public void JobPostNew(int id, [FromBody] Job job)
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

    [HttpGet("Job/get/id/")]
    public Job JobGetId(int Job)
    {
        return context.Job.Find(Job);
    }

    [HttpGet("Job/get/machineid/")]
    public List<Job> MachineGetIdMachine(int idMachine)
    {
        return context.Job.Where(x => x.id_Machine == idMachine).ToList();
    }

    [HttpGet("Job/get/groupid/")]
    public List<Job> MachineGetIdGroup(int idGroup)
    {
        return context.Job.Where(x => x.id_Group == idGroup).ToList();
    }

    [HttpGet("Job/get/configid/")]
    public List<Job> MachineGetIdConfig(int idConfig)
    {
        return context.Job.Where(x => x.id_Config == idConfig).ToList();
    }

    //

    [HttpGet("Job/get/ipAddress/")]
    public Job JobGetId(string ipAddress)
    {
        return context.Job.Include(x => x.Machine).Include(x => x.Config).Where(x => x.Machine.Ip_address == ipAddress && x.status == 0).FirstOrDefault();
    }

    [HttpGet("SourcePath/get/id_config")]
    public List<Sources> SourcePathGetIdConfig(int idConfig)
    {
        List<Sources> sources = context.Sources.Where(x => x.id_Config == idConfig).ToList();
        return sources;
    }

    [HttpGet("DestPath/get/id_config")]
    public List<Destination> DestPathGetIdConfig(int idConfig)
    {
        List<Destination> destinations = context.Destination.Where(x => x.id_Config == idConfig).ToList();
        return destinations;
    }


    [HttpPut("Job/put/time_end/")]
    public void JobPutEnd_time(int jobId, Job job)
    {
        Job result = context.Job.Find(jobId);

        result.time_end = job.time_end;
        result.status = job.status;

        context.SaveChanges();
    }

    [HttpPost("Log/post/New/")]
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
