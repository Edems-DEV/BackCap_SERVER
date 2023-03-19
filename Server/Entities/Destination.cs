using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.DatabaseTables;

public class Destination
{
    [Key]
    public int Id { get; set; }

    public int Id_Config { get; set; }

    public string DestPath { get; set; }
}
