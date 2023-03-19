using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.DatabaseTables;

public class Log
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int Id_Job { get; set; }

    public string Message { get; set; }

    public DateTime Time { get; set; }

    [ForeignKey("Id_Job")]
    public virtual Job Job { get; set; }
}

