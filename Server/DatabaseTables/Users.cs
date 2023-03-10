using System.ComponentModel.DataAnnotations.Schema;

namespace Server.DatabaseTables;

[Table("Users")]
public class Users
{
    public int id { get; set; }

    public string name { get; set; }

    public string password { get; set; }

    public string email { get; set; }

    //[Column("interval_report")]
    //public string interval_report { get; set; }
}
