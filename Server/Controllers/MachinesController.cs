using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.Dtos;
using Server.ParamClasses;
using Server.Validator;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MachinesController : Controller
{
    private readonly Validators validation;
    private readonly MyContext context = new MyContext();

    public MachinesController(Validators validation)
    {
        this.validation = validation;
    }

    [HttpGet]
    public ActionResult<List<WebMachineDto>> Get()
    {
        return Ok(context.Machine.ToList().Select(x => new WebMachineDto(x, context)).ToList());
    }

    [HttpGet("names")]
    public ActionResult<List<WebNameDto>> GetNames()
    {
        return Ok(context.Machine.Select(x => new WebNameDto(x.Id, x.Name)).ToListAsync());
    }

    // GET: for stats
    [HttpGet("count")]
    public ActionResult<int> GetCount(bool active = true)
    {
        return Ok(context.Machine.Where(x => x.Is_Active == active).Count());
    }

    [HttpGet("{Id}")]
    public ActionResult<WebMachineDto> Get(int Id)
    {
        Machine machine = context.Machine.Find(Id);

        if (machine == null)
            return NotFound("Object does not exists");

        return Ok(new WebMachineDto(machine, context));
    }

    [HttpPut("{Id}")]
    public ActionResult Put(int Id, [FromBody] WebMachineDto machine)
    {
        try
        {
            validation.IpValidator(machine.Ip_Address.ToString());
        }
        catch (Exception)
        {
            return BadRequest("Invalid");
        }

        Machine ExistingMachine = context.Machine.Find(Id);

        if (ExistingMachine == null)
            return NotFound("Object does not exists");

        ExistingMachine = machine.UpdateMachine(ExistingMachine, context);

        context.SaveChanges();

        return Ok();
    }

    [HttpPost("register")]
    public ActionResult<int> PostRegister([FromBody] MachineDto machine)
    {
        try
        {
            validation.IpValidator(machine.Ip_Address.ToString());
            validation.MacValidator(machine.Mac_Address.ToString());
        }
        catch (Exception)
        {
            return BadRequest("Invalid");
        }

        Machine NewMachine = machine.GetMachine();

        NewMachine.Mac_Address = NewMachine.Mac_Address.Replace("-", string.Empty);

        context.Machine.Add(NewMachine);
        context.SaveChanges();

        return Ok(NewMachine.Id);
    }

    [HttpDelete("Id")]
    public ActionResult Delete(int Id)
    {
        Machine machine = context.Machine.Find(Id);

        if (machine == null)
            return NotFound();

        context.Machine.Remove(machine);
        context.SaveChanges();

        return Ok();
    }
}
