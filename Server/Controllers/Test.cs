using Microsoft.AspNetCore.Mvc;
using Server.DatabaseTables;
using System.Diagnostics.CodeAnalysis;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class Test : ControllerBase
{
    private readonly MyContext context = new MyContext();

    //--DEAMON--
    //job
    [HttpPost("NewJob/{Job}")]
    public void NewJob([FromBody] Job newJob)
    {
        context.Job.Add(newJob);
        context.SaveChanges();
    }

    [HttpGet("id/{jobId}")]
    public Job IdGetJob(int jobId)
    {
        return context.Job.Find(jobId);
    }

    [HttpGet("machine/{machineId}")]
    public List<Job> MachineGetJob(int machineId)
    {        
        return context.Job.Where(x => x.id_Machine == machineId).ToList();
    }

    [HttpGet("group/{groupId}")]
    public List<Job> GroupGetJob(int groupId)
    {
        return context.Job.Where(x => x.id_Group == groupId).ToList();
    }

    [HttpPut("time_end/{JobId}")]
    public void JobEndTime(int jobId,DateTime time_end)
    {
        Job result = context.Job.Find(jobId);
        result.time_end = time_end;
        context.SaveChanges();
    }

    [HttpPut("time_end/{JobId}")]
    public void JobStatus(int jobId, Int16 status)
    {
        Job result = context.Job.Find(jobId);
        result.status = status;
        context.SaveChanges();
    }

    [HttpDelete("delteJob/{jobId}")]
    public void DeleteJob(int jobId)
    {
        context.Job.Remove(context.Job.Find(jobId));
    }

    //log

    [HttpPost("NewLog/{Log}")]
    public void NewLog([FromBody] Log NewLog)
    {
        context.Log.Add(NewLog);
        context.SaveChanges();
    }

    [HttpGet("id/{logId}")]
    public Log IdGetLog(int logId)
    {
        return context.Log.Find(logId);
    }

    [HttpGet("jobId/{jobId}")]
    public List<Log> JobIdGetLog(int jobId)
    {
        return context.Log.Where(x => x.id_Job == jobId).ToList();
    }

    //--SERVER--
    //machine
    [HttpPost("NewMachine/{Machine}")]
    public void NewMachine([FromBody] Machine newMachine)
    {
        context.Machine.Add(newMachine);
        context.SaveChanges();
    }

    //[HttpPut("MachineEdit/{id}")]
    //public void MachineEdit(int id, *TO CO SE ZMĚNÍ*)
    //{
    //    Machine result = context.Machine.Find(id);
    //    result.*TO CO SE ZMĚNÍ* = *TO CO SE ZMĚNÍ*;
    //    context.SaveChanges();
    //}

    [HttpGet("id/{machineId}")]
    public Machine machineId(int machineId)
    {
        return context.Machine.Find(machineId);
    }

    //group
    [HttpPost("NewGroup/{Group}")]
    public void NewGroup([FromBody] Groups newGroup)
    {
        context.Groups.Add(newGroup);
        context.SaveChanges();
    }

    //[HttpPut("GroupEdit/{id}")]
    //public void GroupEdit(int id, *TO CO SE ZMĚNÍ*)
    //{
    //    Group result = context.Group.Find(id);
    //    result.*TO CO SE ZMĚNÍ* = *TO CO SE ZMĚNÍ*;
    //    context.SaveChanges();
    //}

    [HttpGet("id/{groupId}")]
    public Groups groupId(int groupId)
    {
        return context.Groups.Find(groupId);
    }    

    //config
    [HttpPost("NewConfig/{Config}")]
    public void NewConfig([FromBody] Config newConfig)
    {
        context.Config.Add(newConfig);
        context.SaveChanges();
    }

    //[HttpPut("ConfigEdit/{id}")]
    //public void ConfigEdit(int id, *TO CO SE ZMĚNÍ*)
    //{
    //    Config result = context.Config.Find(id);
    //    result.*TO CO SE ZMĚNÍ* = *TO CO SE ZMĚNÍ*;
    //    context.SaveChanges();
    //}

    [HttpGet("id/{configId}")]
    public Config configId(int configId)
    {
        return context.Config.Find(configId);
    }

    //user
    [HttpPost("NewUser/{User}")]
    public void NewUser([FromBody] User newUser)
    {
        context.User.Add(newUser);
        context.SaveChanges();
    }
}
