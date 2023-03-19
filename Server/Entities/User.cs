using System.ComponentModel.DataAnnotations;

namespace Server.DatabaseTables;

public class User
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Email { get; set; }

    public string Interval_Report { get; set; }
}
