using System.ComponentModel.DataAnnotations;

namespace Server.DatabaseTables;

public class Machine
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Os { get; set; }

    public string? Ip_address { get; set; }

    public string? Mac_address { get; set; }

    public bool Is_active { get; set; }
}
