namespace Server.ParamClasses;

public class ConfigDto
{

    public Int16 Type { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int Retention { get; set; }

    public int PackageSize { get; set; }

    public bool IsCompressed { get; set; }

    public string? Backup_Interval { get; set; }

    public DateTime? Interval_end { get; set; }
}
