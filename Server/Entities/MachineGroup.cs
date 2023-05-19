using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.DatabaseTables;

[PrimaryKey("Id_Machine", "Id_Group")]
public class MachineGroup
{
    public int Id_Machine { get; set; }

    public int Id_Group { get; set; }

    [ForeignKey("Id_Machine")]
    public virtual Machine Machine { get; set; }

    [ForeignKey("Id_Group")]
    public virtual Groups Groups { get; set; }

    public MachineGroup()
    {
        
    }

    public MachineGroup(int id_Machine, int id_Group)
    {
        this.Id_Machine = id_Machine;
        this.Id_Group = id_Group;
    }
}
