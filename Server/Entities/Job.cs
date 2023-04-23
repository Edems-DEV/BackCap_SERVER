using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.DatabaseTables;

public class Job
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int? Id_Machine { get; set; }

    public int? Id_Group { get; set; }

    [Required]
    public int Id_Config { get; set; }

    [Required]
    [Range(0, 1, ErrorMessage = "Value must be within 0 and 1")]
    public Int16 Status { get; set; }

    [Required]
    public DateTime Time_schedule { get; set; }

    public DateTime? Time_start { get; set; }

    public DateTime? Time_end { get; set; }

    public int? Bytes { get; set; }

    [ForeignKey("Id_Machine")]
    public virtual Machine Machine { get; set; }
    
    [ForeignKey("Id_Group")]
    public virtual Groups Groups { get; set; }

    [ForeignKey("Id_Config")]
    public virtual Config Config { get; set; }
}
