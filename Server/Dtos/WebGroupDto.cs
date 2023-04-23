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

    public WebGroupDto(int id, string name, string? description, MyContext context)
    {
        Id = id;
        Name = name;
        Description = description;

        Job job = context.Job.Where(x => x.Id_Group == id).FirstOrDefault();

        if (job == null)
            return;

        foreach (var configs in context.Config.Where(x => x.Id == job.Id_Config).ToList())
        {
            Configs.Add(new WebOthersDto(configs.Id, configs.Name));
        }

        foreach (var machines in context.Machine.Where(x => x.Id == job.Id_Machine).ToList())
        {
            Machines.Add(new WebOthersDto(machines.Id, machines.Name));
        }
    }

    public Groups UpdateGroup(Groups group, MyContext context)
    {
        this.AddJobs(group.Id, context);
        group = this.AddMachines(group, context);


        group.Name = this.Name;
        group.Description = this.Description;

        return group;
    }

    public void AddJobs(int Id, MyContext context)
    {
        List<int> existingConfigs = new();
        context.Job.Where(x => x.Id_Group == Id).ToList().ForEach(x => existingConfigs.Add(x.Id_Config));

        List<int> configsToAdd = new();
        Configs.Where(x => !existingConfigs.Contains(x.Id)).ToList().ForEach(x => configsToAdd.Add(x.Id));

        List<int> configsToDel = new();
        existingConfigs.ForEach(x => configsToDel.Add(x));

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

    public Groups AddMachines(Groups group, MyContext context)
    {
        List<MachineGroup> machineGroups = context.MachineGroup.Where(x => x.Id_Group == group.Id).ToList();
        machineGroups.ForEach(x => context.MachineGroup.Remove(x));
        Machines.ForEach(x => context.MachineGroup.Add(new MachineGroup() { Id_Machine = x.Id, Id_Group = group.Id}));
        return group;
    }
}
