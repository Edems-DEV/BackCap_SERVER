using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.ParamClasses;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConfigsController : Controller
{
    private readonly MyContext context = new MyContext();

    // GET: api/Config?limit=25&offset=50&orderBy=id&orderDirection=desc   => UI datagrid                   
    [HttpGet]
    public IActionResult Get(int limit = 10, int offset = 0) //string orderBy = "id", string orderDirection = "asc"
    {
        var config = context.Config
        .OrderBy(p => p.id)
        .Skip(offset)
        .Take(limit)
        .ToList();

        if (config == null || config.Count == 0)
        {
            return NoContent(); //204
        }

        return Ok(config); //200
    }

    // GET: for stats
    [HttpGet("count")]
    public IActionResult GetCount()
    {
        return Ok(context.Config.Count()); //idk if it works
    }

    [HttpGet("{id}")]
    public Config Get(int id)
    {
        return context.Config.Find(id);
    }

    [HttpGet("{id}/sources")]  //moved from sources
    public List<Sources> GetSources(int id)
    {
        return context.Sources.Where(x => x.id_Config == id).ToList();
    }

    [HttpPost]
    public void Post([FromBody] Config config)
    {
        Config NewConfig = new Config()
        {
            type = config.type,
            retention = config.retention,
            packageSize = config.packageSize,
            isCompressed = config.isCompressed,
            Backup_interval = config.Backup_interval,
            interval_end = config.interval_end
        };

        context.Config.Add(NewConfig);
        context.SaveChanges();
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] Config config)
    {
        Config result = context.Config.Find(id);

        result.type = config.type;
        result.retention = config.retention;
        result.packageSize = config.packageSize;
        result.isCompressed = config.isCompressed;
        result.Backup_interval = config.Backup_interval;
        result.interval_end = config.interval_end;

        context.SaveChanges();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var config = context.Config.Find(id);
        if (config == null)
        {
            return NotFound();
        }
        context.Config.Remove(config);
        context.SaveChanges();
        return Ok($"Delete request received for config id {id}.");
    }
}
