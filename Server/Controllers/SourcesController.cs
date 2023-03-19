using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Server.DatabaseTables;
using Server.ParamClasses;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SourcesController : Controller
{
    private readonly MyContext context = new MyContext();


    [HttpGet("{Id}")]
    public ActionResult<Sources> GetSource(int Id)
    {
        try
        {
            return Ok(context.Sources.Find(Id));
        }
        catch (MySqlException ex)
        {
            return NotFound("Object does not exists");
        }
    }

    [HttpGet("{IdConfig}/Sources")]
    public ActionResult<List<Sources>> GetSources(int IdConfig)
    {
        List<Sources> sources = context.Sources.Where(x => x.Id_Config == IdConfig).ToList();

        if (sources.Count == 0)
            return NotFound();
        else
            return Ok(sources);
    }

    [HttpPost] // funguje, ale je potřeba dát do id config již nějaké existující id configu jinak ta kontrola na webovkách se dodrbe
    public ActionResult Post([FromBody] PathsDto path)
    {
        if (!context.Config.Any(x => x.Id == path.Id_Config))
        {
            return BadRequest("Object doesn't have existing id in Config");
        }

        Sources source = new Sources
        {
            Id_Config = path.Id_Config,
            Path = path.Path
        };

        context.Sources.Add(source);
        context.SaveChanges();

        return Ok();
    }

    [HttpPut("{Id}")]
    public ActionResult Put(int Id, [FromBody] PathsDto path)
    {
        Sources source;

        try
        {
            source = context.Sources.Find(Id);
        }
        catch (MySqlException ex)
        {
            return BadRequest("Object does not exists");
        }

        source.Id_Config = path.Id_Config;
        source.Path = path.Path;

        context.SaveChanges();

        return Ok();
    }
}