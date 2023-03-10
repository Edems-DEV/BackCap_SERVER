namespace Server.DatabaseTables;

public class Config
{
    public int id { get; set; }

    public Int16 type { get; set; }

    public int retencion { get; set; }

    public int packageSize { get; set; }

    public bool isCompressed { get; set; }

    public string backup_interval { get; set; }

    public DateTime interval_end { get; set; }
}
