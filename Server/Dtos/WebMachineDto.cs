using Server.DatabaseTables;
using Server.Services;

namespace Server.Dtos;

public class WebMachineDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Ip_Address { get; set; }

    public bool Is_Active { get; set; }

    public List<WebOthersDto> Configs { get; set; } = new();

    public List<WebOthersDto> Groups { get; set; } = new();

    public WebMachineDto()
    {
        
    }

    public WebMachineDto(Machine machine, MyContext context)
    {
        this.Id = machine.Id;
        this.Name = machine.Name;
        this.Description = machine.Description;
        this.Ip_Address = machine.Ip_Address;
        this.Is_Active = machine.Is_Active;

        Job job = context.Job.Where(x => x.Id_Machine == machine.Id).FirstOrDefault();
        Config config = context.Config.Find(job.Id_Config);
        Groups group = context.Groups.Find(job.Id_Group);


        foreach (var configs in context.Config.Where(x => x.Id == job.Id_Config).ToList())
        {
            Configs.Add(new WebOthersDto(configs.Id, configs.Name));
        }

        foreach (var item in context.MachineGroup.Where(x => x.Id_Machine == Id).ToList())
        {
            Groups groups = context.Groups.Find(item.Id_Group);
            Groups.Add(new WebOthersDto(groups.Id, groups.Name));
        }
    }

    public Machine UpdateMachine(Machine machine, MyContext context)
    {
        this.AddJobs(machine.Id, context);


        machine.Name = this.Name;
        machine.Description = this.Description;
        machine.Ip_Address = this.Ip_Address;
        machine.Is_Active = this.Is_Active;

        return machine;
    }

    public void AddJobs(int Id, MyContext context)
    {
        List<int> existingConfigs = new();
        context.Job.Where(x => x.Id_Machine == Id).ToList().ForEach(x => existingConfigs.Add(x.Id_Config));

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
            configsToAdd.ForEach(x => context.Job.Add(new Job() { Id_Config = x, Id_Machine = Id, Status = 1, Time_schedule = DateTime.Now }));
            context.SaveChanges();
        }

        if (configsToDel.Count != 0)
        {
            List<Job> jobsToRemove = new();
            foreach (var item in configsToDel)
            {
                jobsToRemove.AddRange(context.Job.Where(y => y.Id_Machine == Id && y.Id_Config == item).ToList());
            }

            jobsToRemove.ForEach(x => context.Remove(x));
            context.SaveChanges();
        }

    }
}
