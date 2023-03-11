using System.ComponentModel.DataAnnotations;

namespace Server.DatabaseTables;

public class Groups
{
    [Key]
    public int id { get; set; }

    public string Name { get; set; }
}
