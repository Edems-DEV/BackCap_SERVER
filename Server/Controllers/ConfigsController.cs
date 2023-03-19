using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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
    public IActionResult GetCount()
    {
        return Ok(context.Config.Count());
    }

    [HttpGet("{Id}")]
    public Config Get(int Id)
    {
        //Config config = context.Config.Find(Id);
        //config.Sources = context.Sources.Where(x => x.Id_Config == Id).ToList();
        //config.Destinations = context.Destination.Where(x => x.Id_Config == Id).ToList();

        return context.Config.Find(Id);

        //return config;
    }

    [HttpPost]
    public void Post([FromBody] Config config)
    {
        Config NewConfig = new Config()
        {
            Type = config.Type,
            Retention = config.Retention,
            packageSize = config.packageSize,
            IsCompressed = config.IsCompressed,
            Backup_interval = config.Backup_interval,
            Interval_end = config.Interval_end,
            Sources = config.Sources,
            Destinations = config.Destinations
        };

        context.Config.Add(NewConfig);
        context.Sources.AddRange(NewConfig.Sources);
        context.Destination.AddRange(NewConfig.Destinations);
        context.SaveChanges();
    }

    [HttpPut("{Id}")]
    public void Put(int id, [FromBody] Config config)
    {
        Config result = context.Config.Find(id);

        result.Type = config.Type;
        result.Retention = config.Retention;
        result.packageSize = config.packageSize;
        result.IsCompressed = config.IsCompressed;
        result.Backup_interval = config.Backup_interval;
        result.Interval_end = config.Interval_end;

        context.SaveChanges();
    }

    [HttpDelete("{Id}")]
    public IActionResult Delete(int id)
    {
        var config = context.Config.Find(id);
        if (config == null)
        {
            return NotFound();
        }
        context.Config.Remove(config);
        context.SaveChanges();
        return Ok($"Delete request received for config Id {id}.");
    }
}
