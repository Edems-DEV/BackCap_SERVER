namespace Server.DatabaseTables;

public class Log
{
    public int id { get; set; }

    public int id_Job { get; set; }

    public string message { get; set; }

    public DateTime time { get; set; }
}
