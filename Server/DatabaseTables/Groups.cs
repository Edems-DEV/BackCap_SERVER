using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.DatabaseTables;

public class Groups
{
    [Key]
    public int id { get; set; }

    public string Name { get; set; }

    [ForeignKey("id_Group")]
    public virtual List<MachineGroup> MachineGroups { get; set; }

    [ForeignKey("id_Group")]
    public virtual List<Job> Jobs { get; set; }
}
