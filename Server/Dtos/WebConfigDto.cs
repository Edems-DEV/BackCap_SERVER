using Microsoft.AspNetCore.Http;
using Server.DatabaseTables;
using Server.ParamClasses;
using Server.Services;

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

    public List<WebOthersDto> Sources { get; set; } = new();

    public List<WebOthersDto> Destinations { get; set; } = new();

    public List<WebOthersDto> Machines { get; set; } = new();

    public List<WebOthersDto> Groups { get; set; } = new();

    public WebConfigDto() // overloadnutý konstruktor kvůli put. Jinak spadne protože konstruktor je už occupied
    {
        
    }

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
            .Select(x => new WebOthersDto(x.Id, x.Path))
            .ToList();

        this.Destinations = context
            .Destination
            .Where(x => x.Id_Config == Id)
            .Select(x => new WebOthersDto(x.Id, x.DestPath))
            .ToList();


        Job job = context.Job.Where(x => x.Id_Config == Id).FirstOrDefault();

        if (job == null)
            return;

        List<Job> jobs = context
            .Job
            .Where(x => x.Id_Config == this.Id)
            .Where(x => x.Id_Machine != null)
            .ToList();

        Machines = jobs
            .Select(x => new WebOthersDto(Convert.ToInt32(x.Id_Machine), context.Machine.Find(x.Id_Machine)!.Name!))
            .ToList();

        jobs = context
            .Job
            .Where(x => x.Id_Config == this.Id)
            .Where(x => x.Id_Group != null)
            .ToList();

        Groups = jobs
            .Select(x => new WebOthersDto(Convert.ToInt32(x.Id_Group), context.Groups.Find(x.Id_Group)!.Name))
            .ToList();
    }

    public Config GetConfig(MyContext context)
    {
        this.DatabaseUpdate(new DatabaseManager(context));

        return new Config()
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
    }

    public string ConvertType(int type)
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

    public short ConvertType(string type)
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

    private void DatabaseUpdate(DatabaseManager databaseManager)
    {
        this.Sources.ForEach(x => databaseManager.AddNotExistent(x.GetSources(this.Id)));
        this.Destinations.ForEach(x => databaseManager.AddNotExistent(x.GetDestination(this.Id)));      
    }
}
