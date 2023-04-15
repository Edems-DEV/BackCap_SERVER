using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.DatabaseTables;

public class Config
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    [Range (0, 2, ErrorMessage = "Value must be within 0 and 2")]
    public Int16 Type { get; set; }

    public int Retention { get; set; }

    public int PackageSize { get; set; }

    public bool IsCompressed { get; set; }

    public string? Backup_interval { get; set; }

    public DateTime? Interval_end { get; set; }

    [ForeignKey("Id_Config")]
    public virtual List<Sources> Sources { get; set; }

    [ForeignKey("Id_Config")]
    public virtual List<Destination> Destinations { get; set; }
}
