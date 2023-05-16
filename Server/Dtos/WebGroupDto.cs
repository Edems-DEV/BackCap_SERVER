using Server.DatabaseTables;

namespace Server.Dtos;

public class WebGroupDto
{
    public int Id { get; set; } 

    public string Name { get; set; }

    public string? Description { get; set; }

    public List<WebOthersDto> Configs { get; set; } = new();

    public List<WebOthersDto> Machines { get; set; } = new();

    public WebGroupDto() { }

    public WebGroupDto(Groups group, MyContext context)
    {
        Id = group.Id;
        Name = group.Name;
        Description = group.Description;

        Configs = GetConfigs(Id, context);
        Machines = GetMachine(Id, context);
    }

    private List<WebOthersDto> GetConfigs(int id, MyContext context)
    {
        return context
            .Job
            .Where(x => x.Id_Group == id)
            .Select(x => new WebOthersDto() { Id = x.Id_Config, Name = x.Config.Name }) 
            .ToList();
    }

    private List<WebOthersDto> GetMachine(int id, MyContext context)
    {
        return context
            .MachineGroup
            .Where(x => x.Id_Group == id)
            .Select(x => new WebOthersDto { Id = x.Id_Machine, Name = x.Machine.Name })
            .ToList();
    }

    public Groups UpdateGroup(Groups group, MyContext context)
    {
        this.AddJobs(group.Id, context);
        this.AddMachines(group, context);

        group.Name = this.Name;
        group.Description = this.Description;

        return group;
    }

    private void AddJobs(int Id, MyContext context)
    {
        List<int> existingConfigs = context.Job.Where(x => x.Id_Group == Id).Select(x => x.Id_Config).ToList();

        List<int> configsToAdd = Configs.Where(x => !existingConfigs.Contains(x.Id)).Select(x => x.Id).ToList();

        List<int> configsToDel = existingConfigs.Select(x => x).ToList();

        List<int> temp = new();
        foreach (var item in configsToDel)
        {
            foreach (var item1 in Configs)
            {
                if (item == item1.Id)
                {
                    temp.Add(item);
                    break;
                }
            }

            foreach (var item2 in configsToAdd)
            {
                if (item2 == item)
                {
                    temp.Add(item);
                    break;
                }
            }
        }

        foreach (var item in temp)
        {
            configsToDel.Remove(item);
        }

        if (configsToAdd.Count != 0)
        {
            configsToAdd.ForEach(x => context.Job.Add(new Job() { Id_Config = x, Id_Group = Id, Status = 1, Time_schedule = DateTime.Now }));
            context.SaveChanges();
        }

        if (configsToDel.Count != 0)
        {
            List<Job> jobsToRemove = new();
            foreach (var item in configsToDel)
            {
                jobsToRemove.AddRange(context.Job.Where(y => y.Id_Group == Id && y.Id_Config == item).ToList());
            }

            List<Log> logsToRemove = new();
            foreach (var item in jobsToRemove)
            {
                logsToRemove.AddRange(context.Log.Where(x => x.Id_Job == item.Id).ToList());
            }

            logsToRemove.ForEach(x => context.Log.Remove(x));
            jobsToRemove.ForEach(x => context.Job.Remove(x));

            context.SaveChanges();
        }
    }

    private void AddMachines(Groups group, MyContext context)
    {
        List<MachineGroup> machineGroups = context.MachineGroup.Where(x => x.Id_Group == group.Id).ToList();
        machineGroups.ForEach(x => context.MachineGroup.Remove(x));
        Machines.ForEach(x => context.MachineGroup.Add(new MachineGroup() { Id_Machine = x.Id, Id_Group = group.Id}));
    }
}
