using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;

namespace Server.Dtos;

public class WebConfigDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public string Type { get; set; }

    public bool IsCompressed { get; set; }

    public int PackageSize { get; set; }

    public int Retention { get; set; }

    public string Interval { get; set; }

    public DateTime? Interval_end { get; set; }

    public List<WebNameDto> Sources { get; set; } = new();

    public List<WebNameDto> Destinations { get; set; } = new();

    public List<WebNameDto> Machines { get; set; } = new();

    public List<WebNameDto> Groups { get; set; } = new();

    public WebConfigDto() { } // overloadnutý konstruktor kvůli put. Jinak spadne protože konstruktor je už occupied

    public WebConfigDto(Config config, MyContext context)
    {
        this.Id = config.Id;
        this.Name = config.Name;
        this.Description = config.Description;
        this.Type = this.ConvertType(config.Type);
        this.IsCompressed = config.IsCompressed;
        this.PackageSize = config.PackageSize;
        this.Retention = config.Retention;
        this.Interval = config.Backup_interval;
        this.Interval_end = config.Interval_end;

        this.Sources = context
            .Sources
            .Where(x => x.Id_Config == Id)
            .Select(x => new WebNameDto(x.Id, x.Path))
            .ToList();

        this.Destinations = context
            .Destination
            .Where(x => x.Id_Config == Id)
            .Select(x => new WebNameDto(x.Id, x.DestPath))
            .ToList();

        this.Machines = this.GetExistingMachines(context);
        this.Groups = this.GetExistingGroups(context);
    }

    private List<WebNameDto> GetExistingMachines(MyContext context)
    {
        List<Job> jobs = context
            .Job
            .Where(x => x.Id_Config == this.Id)
            .Where(x => x.Id_Machine != null)
            .ToList();

        return jobs
            .Select(x => new WebNameDto(Convert.ToInt32(x.Id_Machine), context.Machine.Find(x.Id_Machine)!.Name!))
            .ToList();
    }

    private List<WebNameDto> GetExistingGroups(MyContext context)
    {
        List<Job> jobs = context
            .Job
            .Where(x => x.Id_Config == this.Id)
            .Where(x => x.Id_Group != null)
            .ToList();

        return jobs
            .Select(x => new WebNameDto(Convert.ToInt32(x.Id_Group), context.Groups.Find(x.Id_Group)!.Name))
            .ToList();
    }

    public async Task AddConfig(MyContext context)
    {
        var config = new Config()
        {
            Name = this.Name,
            Description = this.Description,
            Type = this.ConvertType(this.Type),
            Retention = this.Retention,
            PackageSize = this.PackageSize,
            Backup_interval = this.Interval,
            IsCompressed = this.IsCompressed,
            Interval_end = this.Interval_end
        };

        await context.AddAsync(config);

        await context.SaveChangesAsync();

        this.Id = config.Id;


        await this.UpdatePaths(context);
        await this.UpdateJobs(context);
    }

    public async Task<Config> GetConfig(Config config, MyContext context)
    {
        await this.UpdatePaths(context);
        await this.UpdateJobs(context);

        config.Name = this.Name;
        config.Description = this.Description;
        config.Type = this.ConvertType(this.Type);
        config.Retention = this.Retention;
        config.PackageSize = this.PackageSize;
        config.Backup_interval = this.Interval;
        config.IsCompressed = this.IsCompressed;
        config.Interval_end = this.Interval_end;

        return config;
    }

    private async Task UpdatePaths(MyContext context)
    {
        context.Sources
            .Where(x => x.Id_Config == this.Id)
            .ToList()
            .ForEach(x => context.Sources.Remove(x));

        context.Destination
            .Where(x => x.Id_Config == this.Id)
            .ToList()
            .ForEach(x => context.Destination.Remove(x));

        Sources
            .Select(x => x.GetSources(this.Id))
            .ToList()
            .ForEach(x => context.Sources.AddAsync(x));

        Destinations
            .Select(x => x.GetDestination(this.Id))
            .ToList()
            .ForEach(x => context.Destination.AddAsync(x));

        await context.SaveChangesAsync();
    }
    private async Task UpdateJobs(MyContext context)
    {
        IEnumerable<int> groupIds = Groups
            .Select(x => x.Id)
            .ToList();

        IEnumerable<int> machineIds = Machines
            .Select(x => x.Id)
            .ToList();

        IEnumerable<int> existingGroups = GetExistingGroups(context)
            .Select(x => x.Id)
            .ToList();

        IEnumerable<int> existingMachines = GetExistingMachines(context)
            .Select(x => x.Id)
            .ToList();

        IList<int> groupsToAdd = groupIds
            .Where(x => !existingGroups.Contains(x))
            .ToList();

        IList<int> machinesToAdd = machineIds
            .Where(x => !existingMachines.Contains(x))
            .ToList();

        IEnumerable<int> groupsToDelete = existingGroups
            .Where(x => !groupIds.Contains(x) && !groupsToAdd.Contains(x))
            .ToList();

        IEnumerable<int> machinesToDelete = existingMachines
            .Where(x => !machineIds.Contains(x) && !machinesToAdd.Contains(x))
            .ToList();

        // odebrat ty co už tam nepatří, a zároveň nechat joby kde alespoň jeden zůstává
        IEnumerable<Job> jobs = await context.Job.Where(x => x.Id_Config == this.Id).ToListAsync();

        foreach (Job job in jobs)
        {
            if (job.Id_Group != null)
                if (groupsToDelete.Contains((int)job.Id_Group))
                    job.Id_Group = null;

            if (job.Id_Machine != null)
                if (machinesToDelete.Contains((int)job.Id_Machine))
                    job.Id_Machine = null;

            if (job.Id_Machine == null && job.Id_Group == null)
                context.Job.Remove(job);
        }

        // add jobů

        for (int i = 0; i < Math.Max(groupsToAdd.Count, machinesToAdd.Count); i++)
        {
            int? groupToAdd = 0;
            int? machineToAdd = 0;

            if (groupsToAdd.Count == i)
                groupToAdd = null;
            else
                groupToAdd = groupsToAdd[i];

            if (machinesToAdd.Count == i)
                machineToAdd = null;
            else
                machineToAdd = machinesToAdd[i];

            context.Job.Add(new Job()
            {
                Id_Group = groupToAdd,
                Id_Machine = machineToAdd,
                Id_Config = this.Id,
                Status = 1,
                Time_schedule = DateTime.Now,
            });
        }

        await context.SaveChangesAsync();
    }

    private string ConvertType(int type)
    {
        switch (type)
        {
            case 0:
                return "Full";

            case 1:
                return "Diff";

            case 2:
                return "Inc";

            default:
                return "Full";
        }
    }

    private short ConvertType(string type)
    {
        switch (type.ToLower())
        {
            case "full":
                return 0;

            case "diff":
                return 1;

            case "inc":
                return 2;

            default:
                return 0;
        }
    }
}
