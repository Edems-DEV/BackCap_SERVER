using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.ParamClasses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MachineController : ControllerBase
{
    private readonly MyContext context = new MyContext();

    [HttpPost]
    public void MachinePostNew([FromBody] MachineDto machine)
    {
        Machine NewMachine = new Machine()
        {
            Name = machine.Name,
            Description = machine.Description,
            Os = machine.Os,
            Ip_address = machine.Ip_Adress,
            Mac_address = machine.Mac_Adress,
            Is_active = machine.Is_Active
        };

        context.Machine.Add(NewMachine);
        context.SaveChanges();
    }

    [HttpPut("{id}")]
    public void MachinePutEdit(int id, [FromBody] Machine machine)
    {
        Machine result = context.Machine.Find(id);
        if (machine.Name != "string")
            result.Name = machine.Name;
        if (machine.Description != "string")
            result.Description = machine.Description;
        if (machine.Os != "string")
            result.Os = machine.Os;
        if (machine.Ip_address != "string")
            result.Ip_address = machine.Ip_address;
        if (machine.Mac_address != "string")
            result.Mac_address = machine.Mac_address;

        result.Is_active = machine.Is_active;

        context.SaveChanges();
    }

    [HttpGet("{id}")]
    public Machine MachineGetId(int machineId)
    {
        return context.Machine.Find(machineId);
    }
}
