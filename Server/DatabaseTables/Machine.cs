namespace Server.DatabaseTables;

public class Machine
{
    public int id { get; set; }

    public string name { get; set; }

    public string description { get; set; }

    public string os { get; set; }

    public string ip_address { get; set; }

    public string mac_address { get; set; }

    public bool is_active { get; set; }
}
