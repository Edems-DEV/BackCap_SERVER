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

    public int Retencion { get; set; }

    public string Interval { get; set; }

    public DateTime? EndOfInterval { get; set; }

    public List<WebOthersDto> Sources { get; set; } = new();

    public List<WebOthersDto> Destinations { get; set; } = new();

    public WebOthersDto Machine { get; set; }

    public List<WebOthersDto> Groups { get; set; } = new();

    public WebConfigDto() // overloadnutý konstruktor kvůli put. Jinak spadne protože konstruktor je už occupied
    {
        
    }

    public WebConfigDto(Config config, MyContext context, int Id)
    {
        Job job = context.Job.Where(x => x.Id_Config == Id).FirstOrDefault();

        this.Id = config.Id;
        Name = config.Name;
        Description = config.Description;
        Type = this.ConvertType(config.Type);
        IsCompressed = config.IsCompressed;
        PackageSize = config.PackageSize;
        Retencion = config.Retention;
        Interval = config.Backup_interval;
        EndOfInterval = config.Interval_end;

        foreach (var source in context.Sources.Where(x => x.Id_Config == Id).ToList())
        {
            Sources.Add(new WebOthersDto(source.Id, source.Path));
        }

        foreach (var destination in context.Destination.Where(x => x.Id_Config == Id).ToList())
        {
            Destinations.Add(new WebOthersDto(destination.Id, destination.DestPath));
        }

        foreach (var groups in context.Groups.Where(x => x.Id == job.Id_Group).ToList())
        {
            Groups.Add(new WebOthersDto(groups.Id, groups.Name));
        }

        //sem je potřeba přidělat opravu, při chybných/ null datech.Zatim netušim co je nejlepší možnost
        Machine machine = context.Machine.Where(x => x.Id == job.Id_Machine).FirstOrDefault();
        Machine = new WebOthersDto(machine.Id, machine.Name);
    }

    public Config GetConfig(MyContext context)
    {
        this.DatabaseUpdate(context);

        return new Config()
        {
            Name = this.Name,
            Description = this.Description,
            Type = this.ConvertType(this.Type),
            Retention = this.Retencion,
            PackageSize = this.PackageSize,
            Backup_interval = this.Interval,
            IsCompressed = this.IsCompressed,
            Interval_end = this.EndOfInterval
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
                return "Incr";

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

            case "incr":
                return 2;

            default:
                return 0;
        }
    }

    private void DatabaseUpdate(MyContext context)
    {
        DatabaseManager databaseManager = new(context);

        foreach (var source in Sources)
        {
            databaseManager.AddNotExistent(source.GetSources(this.Id));
        }

        foreach (var destination in Destinations)
        {
            databaseManager.AddNotExistent(destination.GetDestination(this.Id));
        }
    }
}
