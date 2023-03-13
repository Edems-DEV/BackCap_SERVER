using Microsoft.AspNetCore.Mvc;
using Server.DatabaseTables;
using Server.ParamClasses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class Admin : ControllerBase
{
    private readonly MyContext context = new MyContext();

    #region Job
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
    public void JobPostNew(int id, [FromBody] JobAdminDto job)
    {
        Job existingJob = context.Job.Find(id);

        existingJob.id_Config = job.Id_Config;
        existingJob.id_Group = job.Id_Group;
        existingJob.id_Machine = job.Id_Machine;
        existingJob.status = job.Status;
        existingJob.time_schedule = job.Time_Schedule;
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
    #endregion

    #region Machine
    [HttpPost("Machine/post/new/")]
    public void MachinePostNew([FromBody] MachineDto machine)
    {
        Machine NewMachine = new Machine()
        {
            Name = machine.Name,
            Description = machine.Description,
            Os = machine.Os,
            Ip_address = machine.Ip_Adress,
            Mac_address = machine.Mac_Adress,
            Is_active = machine.Is_Active
        };

        context.Machine.Add(NewMachine);
        context.SaveChanges();
    }

    [HttpPut("Machine/put/edit/")]
    public void MachinePutEdit(int id, [FromBody] MachineDto machine)
    {
        Machine result = context.Machine.Find(id);
        if (machine.Name != "string")
            result.Name = machine.Name;
        if (machine.Description != "string")
            result.Description = machine.Description;
        if (machine.Os != "string")
            result.Os = machine.Os;
        if (machine.Ip_Adress != "string")
            result.Ip_address = machine.Ip_Adress;
        if (machine.Mac_Adress != "string")
            result.Mac_address = machine.Mac_Adress;

        result.Is_active = machine.Is_Active;

        context.SaveChanges();
    }

    [HttpGet("Machine/get/id/")]
    public Machine MachineGetId(int machineId)
    {
        return context.Machine.Find(machineId);
    }

    #endregion

    #region Group
    //group
    [HttpPost("Group/post/new/")]
    public void GroupPostNew([FromBody] string name)
    {
        Groups NewGroup = new Groups();
        NewGroup.Name = name;

        context.Groups.Add(NewGroup);
        context.SaveChanges();
    }

    [HttpPut("Group/put/edit/")]
    public void GroupPutEdit(int id, [FromBody] string name)
    {
        Groups result = context.Groups.Find(id);
        result.Name = name;
        context.SaveChanges();
    }

    [HttpGet("Group/get/id/")]
    public Groups GroupGetId(int groupId)
    {
        return context.Groups.Find(groupId);
    }

    #endregion

    #region config

    //config
    [HttpPost("Config/post/new/")]
    public void ConfigPostNew([FromBody] ConfigDto config)
    {
        Config NewConfig = new Config() {
            type = config.Type,
            retention = config.Retention,
            packageSize = config.PackageSize,
            isCompressed = config.IsCompressed,
            Backup_interval = config.Backup_Interval,
            interval_end = config.Interval_end
    };

        context.Config.Add(NewConfig);
        context.SaveChanges();
    }

    [HttpPut("Config/put/id/")]
    public void ConfigPutEdit(int id, [FromBody] ConfigDto config)
    {
        Config result = context.Config.Find(id);
        
        result.type = config.Type;
        result.retention = config.Retention;
        result.packageSize = config.PackageSize;
        result.isCompressed = config.IsCompressed;
        result.Backup_interval = config.Backup_Interval;
        result.interval_end = config.Interval_end;

        context.SaveChanges();
    }

    [HttpGet("Config/get/id/")]
    public Config ConfigGetId(int configId)
    {
        return context.Config.Find(configId);
    }

    #endregion

    #region User

    //user
    [HttpPost("User/post/new/")]
    public void UserPostNew([FromBody] UserDto user)
    {
        User NewUser = new User()
        {
            name = user.Name,
            email = user.Email,
            password = user.Password,
            interval_report = user.IntervalReport
        };

        context.User.Add(NewUser);
        context.SaveChanges();
    }

    [HttpPut("User/put/id/")]
    public void UserPutEdit(int id, [FromBody] UserDto user)
    {
        User existingUser = context.User.Find(id);

        existingUser.name = user.Name;
        existingUser.email = user.Email;
        existingUser.password = user.Password;
        existingUser.interval_report = user.IntervalReport;

        context.SaveChanges();
    }

    [HttpGet("User/get/email/")]
    public List<User> UserGetEmail(string email)
    {
        return context.User.Where(x => x.email == email).ToList();
    }

    [HttpGet("User/get/username/")]
    public List<User> UserGetUsername(string name)
    {
        return context.User.Where(x => x.name == name).ToList();
    }

    [HttpDelete("User/Delete/id/")]
    public void UserDelete(int id)
    {
        User user = context.User.Find(id);
        context.User.Remove(user);
        context.SaveChanges();
    }

    #endregion

    #region SourcePath

    [HttpPost("SourcePath/Post/Paths/")]
    public void SourcePathPostNew([FromBody] List<PathsDto> paths)
    {
        List<Sources> sources = new List<Sources>();
        foreach (PathsDto item in paths)
        {
            Sources source = new Sources
            {
                id_Config = item.Id_Config,
                path = item.Path
            };

            sources.Add(source);
        }

        context.AddRange(sources);
        context.SaveChanges();
    }

    [HttpPut("SourcePath/Put/Path/")]
    public void SourcePathPutEdit(int id, [FromBody] PathsDto path)
    {
        Sources source = context.Sources.Find(id);

        source.id_Config = path.Id_Config;
        source.path = path.Path;

        context.Add(source);
        context.SaveChanges();
    }

    [HttpGet("SourcePath/Get/IdConfig/")]
    public List<Sources> SourcePathGetIdConfig(int id_config)
    {
        return context.Sources.Where(x => x.id_Config == id_config).ToList();
    }

    #endregion

    #region DestPath

    [HttpPost("DestPath/Post/Paths/")]
    public void DestPathPostNew([FromBody] List<PathsDto> paths)
    {
        List<Sources> sources = new List<Sources>();
        foreach (PathsDto item in paths)
        {
            Sources source = new Sources
            {
                id_Config = item.Id_Config,
                path = item.Path
            };

            sources.Add(source);
        }

        context.AddRange(sources);
        context.SaveChanges();
    }

    [HttpPut("DestPath/Put/Path/")]
    public void DestPathPutEdit(int id, [FromBody] PathsDto path)
    {
        Destination destinations = context.Destination.Find(id);

        destinations.id_Config = path.Id_Config;
        destinations.DestPath = path.Path;

        context.Add(destinations);
        context.SaveChanges();
    }

    [HttpGet("DestPath/Get/IdConfig/")]
    public List<Sources> DestPathGetIdConfig(int id_config)
    {
        return context.Sources.Where(x => x.id_Config == id_config).ToList();
    }

    #endregion
}
