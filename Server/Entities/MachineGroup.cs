using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.DatabaseTables;

[PrimaryKey("id_Machine", "id_Group", "id_Log")]
public class MachineGroup
{
    public int id_Machine { get; set; }

    public int id_Group { get; set; }

    public int id_Log { get; set; }

    [ForeignKey("id_Machine")]
    public virtual Machine Machine { get; set; }

    [ForeignKey("id_Group")]
    public virtual Groups Groups { get; set; }

    [ForeignKey("id_Log")]
    public virtual Log Log { get; set; }
}
