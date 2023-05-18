namespace Server.DatabaseTables;

public class Machine
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Os { get; set; }

    public string? Ip_Address { get; set; }

    public string? Mac_Address { get; set; }

    public bool Is_Active { get; set; }
}
