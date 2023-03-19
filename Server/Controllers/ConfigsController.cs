using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using MySql.Data.MySqlClient;
using Server.DatabaseTables;
using Server.ParamClasses;
using Server.Validator;
using System.ComponentModel.DataAnnotations;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConfigsController : Controller
{
    private readonly MyContext context = new MyContext();
    private Validators validation = new Validators();
           
    // GET: for stats
    [HttpGet("count")]
    public ActionResult<int> GetCount()
    {
        return Ok(context.Config.Count());
    }

    [HttpGet("{Id}")]
    public ActionResult<Config> Get(int Id)
    {
        Config config = context.Config.Find(Id);

        if (config == null)
            return NotFound("Object does not exists");

        config.Sources = context.Sources.Where(x => x.Id_Config == Id).ToList();
        config.Destinations = context.Destination.Where(x => x.Id_Config == Id).ToList();

        return Ok(config);
    }

    [HttpPost]
    public void Post([FromBody] Config config) // vytvoří config a k němu přidružené sourcy a destinace. Položku id config to ingoruje
    {
        Config newConfig = config;

        context.Config.Add(newConfig);
        context.Sources.AddRange(newConfig.Sources);
        context.Destination.AddRange(newConfig.Destinations);
        context.SaveChanges();
    }

    [HttpPut("{Id}")]
    public ActionResult Put(int Id, [FromBody] ConfigDto config) // upravuje pouze samostatný config bez cest
    {
        Config result = context.Config.Find(Id);

        if (result == null)
            return NotFound("Object does not exists");

        result.Retention = config.Retention;
        result.Interval_end = config.Interval_end;
        result.PackageSize = config.PackageSize;
        result.Backup_interval = config.Backup_Interval;
        result.IsCompressed = config.IsCompressed;
        result.Type = config.Type;

        context.SaveChanges();
        return Ok();
    }

    [HttpDelete("{Id}")]
    public ActionResult Delete(int Id)
    {
        Config config = context.Config.Find(Id);

        if (config == null)
            return NotFound("Object does not exists");

        context.Config.Remove(config);
        context.SaveChanges();
        return Ok($"Delete request received for config Id {Id}.");
    }
}
