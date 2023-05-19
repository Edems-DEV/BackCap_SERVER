using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;

namespace Server.Dtos;

public class WebGroupDto
{
    public int Id { get; set; } 

    public string Name { get; set; }

    public string? Description { get; set; }

    public ICollection<WebNameDto> Configs { get; set; } = new List<WebNameDto>();

    public List<WebNameDto> Machines { get; set; } = new List<WebNameDto>();

    public WebGroupDto() { }

    public WebGroupDto(Groups group, MyContext context)
    {
        Id = group.Id;
        Name = group.Name;
        Description = group.Description;

        //Configs = group.Jobs.Select(x => new WebNameDto(x.Config.Id, x.Config.Name)).ToList();
        //Machines = group.Jobs.Select(x => new WebNameDto(x.Machines.Id, x.Machines.Name)).ToList();

        Configs = this.GetConfigs(Id, context);
        Machines = this.GetMachine(Id, context);
    }

    private List<WebNameDto> GetConfigs(int id, MyContext context)
    {
        return context
            .Job
            .Where(x => x.Id_Group == id)
            .Select(x => new WebNameDto() { Id = x.Id_Config, Name = x.Config.Name }) 
            .ToList();
    }

    private List<WebNameDto> GetMachine(int id, MyContext context)
    {
        return context
            .MachineGroup
            .Where(x => x.Id_Group == id)
            .Select(x => new WebNameDto { Id = x.Id_Machine, Name = x.Machine.Name })
            .ToList();
    }

    public async Task AddGroup(MyContext context)
    {
        Groups group = new Groups()
        {
            Name = this.Name,
            Description = this.Description
        };

        await context.Groups.AddAsync(group);
        await context.SaveChangesAsync();

        this.Id = group.Id;

        await this.AddJobs(context);
        await this.AddMachines(context);
    }

    public async Task<Groups> GetGroup(MyContext context)
    {
        await this.AddJobs(context);
        await this.AddMachines(context);

        return new Groups()
        {
            Name = this.Name,
            Description = this.Description
        };
    }

    private async Task AddJobs(MyContext context)
    {
        List<int> existingConfigs = await context
            .Job
            .Where(x => x.Id_Group == this.Id)
            .Select(x => x.Id_Config)
            .ToListAsync();

        List<int> configsToAdd = 
            Configs
            .Where(x => !existingConfigs.Contains(x.Id))
            .Select(x => x.Id)
            .ToList();

        List<int> configsToDel = existingConfigs
            .Select(x => x)
            .ToList();

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
            configsToAdd.ForEach(x => context.Job.Add(new Job() { Id_Config = x, Id_Group = this.Id, Status = 1, Time_schedule = DateTime.Now }));
            await context.SaveChangesAsync();
        }

        if (configsToDel.Count != 0)
        {
            List<Job> jobsToRemove = new();
            foreach (var item in configsToDel)
            {
                jobsToRemove.AddRange(context.Job.Where(y => y.Id_Group == this.Id && y.Id_Config == item).ToList());
            }

            jobsToRemove.ForEach(x => context.Job.Remove(x));

            await context.SaveChangesAsync();
        }
    }

    private async Task AddMachines(MyContext context)
    {
        List<MachineGroup> machineGroups = await context
            .MachineGroup
            .Where(x => x.Id_Group == this.Id)
            .ToListAsync();

        machineGroups
            .ForEach(x => context.MachineGroup.Remove(x));

        Machines
            .ForEach(x => context.Add(new MachineGroup() { Id_Machine = x.Id, Id_Group = this.Id }));

        await context.SaveChangesAsync();
    }
}
