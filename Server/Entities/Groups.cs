using System.ComponentModel.DataAnnotations;

namespace Server.DatabaseTables;

public class Groups
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }
}
