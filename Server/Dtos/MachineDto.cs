using Server.DatabaseTables;

namespace Server.ParamClasses;

public class MachineDto
{
    public string? Name { get; set; } = null;

    public string? Description { get; set; } = null;

    public string? Os { get; set; } = null;

    public string Ip_Address { get; set; }

    public string Mac_Address { get; set; }

    public Machine GetMachine()
    {
        return new Machine
        {
            Name = Name,
            Description = Description,
            Os = Os,
            Ip_Address = Ip_Address,
            Mac_Address = Mac_Address
        };
    }
}
