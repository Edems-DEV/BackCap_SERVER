using Microsoft.AspNetCore.Http;
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

    public int Retencion { get; set; }

    public string Interval { get; set; }

    public DateTime? EndOfInterval { get; set; }

    public List<Sources> Sources { get; set; }

    public List<Destination> Destinations { get; set; }

    public Machine Machine { get; set; }

    public Groups Group { get; set; }

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
        Sources = context.Sources.Where(x => x.Id_Config == Id).ToList();
        Destinations = context.Destination.Where(x => x.Id_Config == Id).ToList();
        Machine = context.Machine.Where(x => x.Id == job.Id_Machine).FirstOrDefault();
        Group = context.Groups.Where(x => x.Id == job.Id_Group).FirstOrDefault();
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

    public Int16 ConvertType(string type)
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
}
