using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Server.DatabaseTables;
using Server.Dtos;
using Server.ParamClasses;
using Server.Validator;
using System.Net.Mail;
using System.Xml.Linq;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MachinesController : Controller
{
    private readonly MyContext context = new MyContext();
    private Validators validation = new Validators();


    // GET: api/machines?limit=25&offset=50&orderBy=Id&isAscending=false
    [HttpGet]
    public ActionResult<List<WebMachineDto>> Get(int limit = 10, int offset = 0)
    {
        //int limit = 10, int offset = 0, string orderBy = "empty", bool isAscending = true
        string orderBy = "empty"; bool isAscending = true;
        string sql = "SELECT * FROM `Machine`";

        var tables = new List<string> { "id", "name", "description", "os", "ip_adress", "mac_adress", "is_active" };
        var direction = isAscending ? "ASC" : "DESC";

        if (tables.Contains(orderBy.ToLower())) //hope this is enough to stop sql injection
        {
            sql += $" ORDER BY `{orderBy}` {direction}";
        }

        List<Machine> query = context.Machine.FromSqlRaw(sql + " LIMIT {0} OFFSET {1}", limit, offset).ToList();

        if (query == null || query.Count == 0)
        {
            return NoContent(); //204
        }

        List<WebMachineDto> machineDtos = new();
        foreach (var machine in query)
        {
            machineDtos.Add(new WebMachineDto(machine, context));
        }

        return Ok(machineDtos); //200
    } //&orderBy  => is required (idk how to make it optimal)

    [HttpGet("names")]
    public ActionResult<List<WebOthersDto>> GetNames()
    {
        List<WebOthersDto> names = new();

        foreach (var machine in context.Machine.ToList())
        {
            names.Add(new WebOthersDto(machine.Id, machine.Name));
        }

        if (names.Count == 0)
        {
            return NoContent();
        }

        return Ok(names);
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

    [HttpPost]
    public ActionResult Post([FromBody] WebMachineDto machine)
    {
        try
        {
            validation.IpValidator(machine.Ip_Address.ToString());
        }
        catch (Exception)
        {
            return BadRequest("Invalid");
        }

        //Machine NewMachine = new Machine()
        //{
        //    Name = machine.Name,
        //    Description = machine.Description,
        //    Os = machine.Os,
        //    Ip_Address = machine.Ip_Address,
        //    Mac_Address = machine.Mac_Address,
        //    Is_Active = machine.Is_Active
        //};

        //context.Machine.Add(NewMachine);
        context.SaveChanges();

        return Ok();
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
            return NotFound("Invalid");
        }

        Machine ExistingMachine = context.Machine.Find(Id);

        if (ExistingMachine == null)
            return NotFound("Object does not exists");

        ExistingMachine = machine.UpdateMachine(ExistingMachine);

        context.SaveChanges();

        return Ok();
    }

    [HttpPost("register")]
    public ActionResult PostRegister([FromBody] MachineDto machine)
    {
        try
        {
            validation.IpValidator(machine.Ip_Address.ToString());
            validation.MacValidator(machine.Mac_Address.ToString());
        }
        catch (Exception)
        {
            return NotFound("Invalid");
        }

        Machine NewMachine = new Machine()
        {
            Name = machine.Name,
            Description = machine.Description,
            Os = machine.Os,
            Ip_Address = machine.Ip_Address,
            Mac_Address = machine.Mac_Address,
            Is_Active = false //není activní -> čeká se na admianovo potvrzení (?)(nemám ponětí jak t má fungovat lol)
        };

        context.Machine.Add(NewMachine);
        context.SaveChanges();

        return Ok();
    }
}
