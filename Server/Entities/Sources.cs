using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.DatabaseTables;

public class Sources
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int Id_Config { get; set; }

    [Required]
    public string Path { get; set; }
}
