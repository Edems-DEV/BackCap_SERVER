using System.ComponentModel.DataAnnotations;

namespace Server.DatabaseTables;

public class User
{
    [Key]
    public int id { get; set; }

    public string name { get; set; }

    public string password { get; set; }

    public string email { get; set; }

    public string interval_report { get; set; }
}
