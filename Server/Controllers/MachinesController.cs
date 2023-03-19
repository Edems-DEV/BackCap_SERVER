using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Server.DatabaseTables;
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
    public IActionResult Get(int limit = 10, int offset = 0, string orderBy = "empty", bool isAscending = true)
    {
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

        return Ok(query); //200
    } //&orderBy  => is required (idk how to make it optimal)

    // GET: for stats
    [HttpGet("count")]
    public ActionResult<int> GetCount(bool active = true)
    {
        return Ok(context.Machine.Where(x => x.Is_Active == active).Count());
    }

    [HttpGet("{Id}")]
    public ActionResult<Machine> Get(int Id)
    {
        Machine machine = context.Machine.Find(Id);

        if (machine == null)
            return NotFound("Object does not exists");

        return Ok(machine);
    }

    [HttpPost]
    public ActionResult Post([FromBody] MachineDto machine)
    {
        try
        {
            validation.IpValidator(machine.Ip_Address.ToString());
            validation.MacValidator(machine.Mac_Address.ToString());
        }
        catch (Exception)
        {

            throw;
        }

        Machine NewMachine = new Machine()
        {
            Name = machine.Name,
            Description = machine.Description,
            Os = machine.Os,
            Ip_Address = machine.Ip_Address,
            Mac_Address = machine.Mac_Address,
            Is_Active = machine.Is_Active
        };

        context.Machine.Add(NewMachine);
        context.SaveChanges();

        return Ok();
    }

    [HttpPut("{Id}")]
    public ActionResult Put(int Id, [FromBody] MachineDto machine)
    {
        try
        {
            validation.IpValidator(machine.Ip_Address.ToString());
            validation.MacValidator(machine.Mac_Address.ToString());
        }
        catch (Exception)
        {

            throw;
        }

        Machine ExistingMachine = context.Machine.Find(Id);

        if (ExistingMachine == null)
            return NotFound("Object does not exists");

        ExistingMachine.Name = machine.Name;
        ExistingMachine.Description = machine.Description;
        ExistingMachine.Os = machine.Os;
        ExistingMachine.Ip_Address = machine.Ip_Address;
        ExistingMachine.Mac_Address = machine.Mac_Address;
        ExistingMachine.Is_Active = machine.Is_Active;

        context.SaveChanges();

        return Ok();
    }
}
