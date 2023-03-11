using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Server.DatabaseTables;

public class Config
{
    public int id { get; set; }

    public Int16 type { get; set; }

    public int retention { get; set; }

    public int packageSize { get; set; }

    public bool isCompressed { get; set; }

    public string? Backup_interval { get; set; }

    public DateTime? interval_end { get; set; }
}
