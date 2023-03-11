using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.DatabaseTables;

public class Log
{
    [Key]
    public int id { get; set; }

    public int id_Job { get; set; }

    public string message { get; set; }

    public DateTime time { get; set; }

    [ForeignKey("id_Job")]
    public virtual Job Job { get; set; }
}
