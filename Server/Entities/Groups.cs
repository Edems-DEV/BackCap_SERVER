using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.DatabaseTables;

public class Groups
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
}
