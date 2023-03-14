using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.DatabaseTables;

public class Job
{
    [Key]
    public int id { get; set; }

    public int id_Machine { get; set; }

    public int? id_Group { get; set; }

    public int id_Config { get; set; }

    public Int16 status { get; set; }

    public DateTime time_schedule { get; set; }

    public DateTime? time_start { get; set; }

    public DateTime? time_end { get; set; }

    public int? Bytes { get; set; }

    [ForeignKey("id_Machine")]
    public virtual Machine Machine { get; set; }

    [ForeignKey("id_Group")]
    public virtual Groups Groups { get; set; }

    [ForeignKey("id_Config")]
    public virtual Config Config { get; set; }
}
