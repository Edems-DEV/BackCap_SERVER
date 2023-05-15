using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using MySql.Data.MySqlClient;
using Server.DatabaseTables;
using Server.Dtos;
using Server.ParamClasses;
using Server.Services;
using Server.Validator;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConfigsController : Controller
{
    private readonly Validators validation;
    private readonly MyContext context = new MyContext();

    public ConfigsController(Validators validation)
    {
        this.validation = validation;
    }

    // GET: api/configs?limit=25&offset=50&orderBy=Id&isAscending=false
    [HttpGet]
    public ActionResult<List<WebConfigDto>> Get(int limit = 10, int offset = 0)
    {
        //int limit = 10, int offset = 0, string orderBy = "empty", bool isAscending = true
        string orderBy = "empty"; bool isAscending = true;
        string sql = "SELECT * FROM `Config`";

        var tables = new List<string> { "id", "type", "name", "description", "retention", "packageSize", "isCompressed", "Backup_interval", "interval_end" };
        var direction = isAscending ? "ASC" : "DESC";

        if (tables.Contains(orderBy)) //hope this is enough to stop sql injection
        {
            sql += $" ORDER BY `{orderBy}` {direction}";
        }

        List<Config> query = context.Config.FromSqlRaw(sql).ToList();// + " LIMIT {0} OFFSET {1}", limit, offset
        List<WebConfigDto> configDtos = new List<WebConfigDto>();

        if (query == null || query.Count == 0)
        {
            return NoContent(); //204
        }

        foreach (var config in query)
        {
            configDtos.Add(new WebConfigDto(config, context, config.Id));
        }

        return Ok(configDtos); //200
    } //&orderBy  => is required (idk how to make it optimal)

    [HttpGet("names")]
    public ActionResult<List<WebOthersDto>> GetNames()
    {
        List<WebOthersDto> names = new();

        foreach (var config in context.Config.ToList())
        {
            names.Add(new WebOthersDto(config.Id, config.Name));
        }

        if (names.Count == 0) 
        {
            return NoContent();
        }

        return Ok(names);
    }

    // GET: for stats
    [HttpGet("count")]
    public ActionResult<int> GetCount()
    {
        return Ok(context.Config.Count());
    }

    [HttpGet("{Id}")]
    public ActionResult<WebConfigDto> Get(int Id)
    {
        Config config = context.Config.Find(Id);

        if (config == null)
            return NotFound("Object does not exists");

        return Ok(new WebConfigDto(config, context, Id));
    }

    [HttpPost]
    public ActionResult Post([FromBody] Config config) // vytvoří config a k němu přidružené sourcy a destinace. Položku id config to ingoruje
    {   
        try
        {
            validation.DateTimeValidator(config.Interval_end.ToString());
        }
        catch (Exception)
        {
            return NotFound("Invalid");
        }

        context.Config.Add(config);
        context.Sources.AddRange(config.Sources);
        context.Destination.AddRange(config.Destinations);
        context.SaveChanges();
        return Ok();
    }

    [HttpPut("{Id}")]
    public ActionResult Put(int Id, [FromBody] WebConfigDto config)
    {
        config.Id = Id;
        DatabaseManager databaseManager = new(context);

        
        try
        {
            validation.DateTimeValidator(config.Interval_end.ToString());
        }
        catch (Exception)
        {
            return BadRequest("Invalid DateTime");
        }

        Config result = context.Config.Find(Id);

        if (result == null)
            return NotFound("Object does not exists");

        // update configu // uloží nové cesty
        result.GetData(config.GetConfig(context));

        // update group a pc
        Job job = context.Job.Where(x => x.Id_Config == config.Id).FirstOrDefault();

        job.Id_Group = config.Group.Id;
        job.Id_Machine = config.Machine.Id;


        context.SaveChanges();
        return Ok();
    }

    [HttpDelete("{Id}")]
    public ActionResult Delete(int Id)
    {
        Config config = context.Config.Find(Id);

        if (config == null)
            return NotFound("Object does not exists");

        context.Sources.Where(x => x.Id_Config == config.Id).ToList().ForEach(y => context.Sources.Remove(y));
        context.Destination.Where(x => x.Id_Config == config.Id).ToList().ForEach(y => context.Destination.Remove(y));
        context.Config.Remove(config);
        context.SaveChanges();
        return Ok();
    }
}
