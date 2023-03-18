using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.DatabaseTables;

public class Sources
{
    [Key]
    public int id { get; set; }

    public int id_Config { get; set; }

    public string path { get; set; }
}
