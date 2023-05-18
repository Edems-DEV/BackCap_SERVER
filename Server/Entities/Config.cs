using MySqlX.XDevAPI.Common;
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

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Retencion value must be 1 or greater")]
    public int Retention { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Package size must be 1 or greater")]
    public int PackageSize { get; set; }

    [Required]
    public bool IsCompressed { get; set; }

    [Required]
    public string Backup_interval { get; set; }

    public DateTime? Interval_end { get; set; }

    [ForeignKey("Id_Config")]
    public virtual List<Sources> Sources { get; set; }

    [ForeignKey("Id_Config")]
    public virtual List<Destination> Destinations { get; set; }

    public void GetData(Config config)
    {
        this.Name = config.Name;
        this.Description = config.Description;
        this.Retention = config.Retention;
        this.PackageSize = config.PackageSize;
        this.Backup_interval = config.Backup_interval;
        this.Interval_end = config.Interval_end;
    }

}
