using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Server.DatabaseTables;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class Test : ControllerBase
{
    private readonly MyContext context = new MyContext();

    //--DEAMON--
    //job
    [HttpPost("DEAMON/Job/post/new/")]
    public void JobPostNew (int id_Machine, int id_Group, int id_Config, Int16 status, DateTime time_schedule, DateTime time_start, DateTime time_end, int Bytes)
    {
        Job NewJob = new Job();
        NewJob.id_Machine = id_Machine;
        NewJob.id_Group = id_Group;
        NewJob.id_Config = id_Config;
        NewJob.status = status;
        NewJob.time_schedule = time_schedule;
        NewJob.time_start = time_start;
        NewJob.time_end = time_end;
        NewJob.Bytes = Bytes;

        context.Job.Add(NewJob);
        context.SaveChanges();
    }

    [HttpGet("DEAMON/Job/get/id/")]
    public Job JobGetId(int jobId)
    {
        Job NewJob = context.Job.Find(jobId);
        return NewJob;
    }

    [HttpGet("DEAMON/Job/get/machine/")]
    public List<Job> JobGetMachine(int machineId)
    {        
        return context.Job.Where(x => x.id_Machine == machineId).ToList();
    }

    [HttpGet("DEAMON/Job/get/group/")]
    public List<Job> JobGetGroup(int groupId)
    {
        return context.Job.Where(x => x.id_Group == groupId).ToList();
    }

    [HttpPut("DEAMON/Job/put/time_end/")]
    public void JobPutEnd_time(int jobId,DateTime time_end)
    {
        Job result = context.Job.Find(jobId);
        result.time_end = time_end;
        context.SaveChanges();
    }

    [HttpPut("DEAMON/Job/put/status/")]
    public void JobPutStatus(int jobId, Int16 status)
    {
        Job result = context.Job.Find(jobId);
        result.status = status;
        context.SaveChanges();
    }

    //[HttpDelete("DEAMON/Job/delete/id/")]
    //public void JobDeleteId(int jobId)
    //{
    //    //musí se ještě smazat log s stejným foraign key

    //    //Job removeJob = ;
    //    context.Job.Remove(context.Job.Find(jobId));
    //    context.SaveChanges();
    //}

    //log

    [HttpPost("DEAMON/Log/post/New/")]
    public void LogPostNew(string messgae,DateTime time,int idJob)
    {
        Log newLog = new Log();
        newLog.message = messgae;
        newLog.time = time;
        newLog.id_Job = idJob;

        //string dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        context.Log.Add(newLog);
        context.SaveChanges();
    }

    [HttpGet("DEAMON/Log/get/id/")]
    public Log LogGetId(int logId)
    {
        return context.Log.Find(logId);
    }

    [HttpGet("DEAMON/Log/get/jobId/")]
    public List<Log> LogGetJobId(int jobId)
    {
        return context.Log.Where(x => x.id_Job == jobId).ToList();
    }



    //--SERVER--
    //machine
    [HttpPost("SERVER/machine/post/new/")]
    public void MachinePostNew (string name,string description,string os,string ip_address,string mac_address,bool is_active)
    {
        Machine NewMachine = new Machine();
        NewMachine.Name = name;
        NewMachine.Description = description;
        NewMachine.Os = os;
        NewMachine.Ip_address = ip_address;
        NewMachine.Mac_address = mac_address;
        NewMachine.Is_active = is_active;

        context.Machine.Add(NewMachine);
        context.SaveChanges();
    }

    [HttpPut("SERVER/machine/put/edit/")]
    public void MachinePutEdit(int id, string name=null, string description = null, string os = null, string ip_address = null, string mac_address = null, bool is_active=true)
    {
        Machine result = context.Machine.Find(id);
        if (name != null)
            result.Name = name;
        if (description != null)
            result.Description = description;
        if (os != null)
            result.Os = os;
        if (ip_address != null)
            result.Ip_address = ip_address;
        if (mac_address != null)
            result.Mac_address = mac_address;
        if (is_active != null)
            result.Is_active = is_active;

        context.SaveChanges();
    }

    [HttpGet("SERVER/machine/get/id/")]
    public Machine MachineGetId(int machineId)
    {
        Machine NewMachine = context.Machine.Find(machineId);
        return NewMachine;
    }

    //group
    [HttpPost("SERVER/group/post/new/")]
    public void GroupPostNew(string name)
    {
        Groups NewGroup = new Groups();
        NewGroup.Name = name;

        context.Groups.Add(NewGroup);
        context.SaveChanges();
    }

    [HttpPut("SERVER/group/put/edit/")]
    public void GroupPutEdit(int id, string name)
    {
        Groups result = context.Groups.Find(id);
        result.Name = name;
        context.SaveChanges();
    }

    [HttpGet("SERVER/group/get/id/")]
    public Groups GroupGetId(int groupId)
    {
        Groups NewGroup = context.Groups.Find(groupId);
        return NewGroup;
    }    

    //config
    [HttpPost("SERVER/config/post/new/")]
    public void ConfigPostNew(Int16 type, int retention, int packageSize, bool isCompressed, string Backup_interval, DateTime interval_end)
    {
        Config NewConfig = new Config();
        NewConfig.type = type;
        NewConfig.retention = retention;
        NewConfig.packageSize = packageSize;
        NewConfig.isCompressed = isCompressed;
        NewConfig.Backup_interval = Backup_interval;
        NewConfig.interval_end = interval_end;

        context.Config.Add(NewConfig);
        context.SaveChanges();
    }

    [HttpPut("SERVER/config/put/edit/")]
    public void ConfigPutEdit(int id, Int16 type)
    {
        Config result = context.Config.Find(id);
        result.type = type;
        context.SaveChanges();
    }

    [HttpGet("SERVER/config/get/id/")]
    public Config ConfigGetId(int configId)
    {
        Config NewConfig = context.Config.Find(configId);
        return NewConfig;
    }

    //user
    [HttpPost("SERVER/user/post/new/")]
    public void UserPostNew(string name, string password, string email, string interval_report)
    {
        User NewUser = new User();
        NewUser.name = name;
        NewUser.password = password;
        NewUser.email = email;
        NewUser.interval_report = interval_report;

        context.User.Add(NewUser);
        context.SaveChanges();
    }

    [HttpGet("SERVER/user/get/email/")]
    public List<User> UserGetEmail(string email)
    {
        List<User> NewUsers = context.User.Where(x => x.email == email).ToList();
        return NewUsers;
    }

    [HttpGet("SERVER/user/get/username/")]
    public List<User> UserGetUsername(string name)
    {
        List<User> NewUsers = context.User.Where(x => x.name == name).ToList();
        return NewUsers;
    }
}
