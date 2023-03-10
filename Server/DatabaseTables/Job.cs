namespace Server.DatabaseTables;

public class Job
{
    public int id { get; set; }

    public int id_Machine { get; set; }

    public int id_Group { get; set; }

    public int id_Config { get; set; }

    public Int16 status { get; set; }

    public DateTime time_schedule { get; set; }

    public DateTime start { get; set; }

    public DateTime end { get; set; }

    public int Bytes { get; set; }
}
