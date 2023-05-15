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
        Sources source = context.Sources.Find(Id);

        if (source == null)
            return NotFound("Object does not exists");

        return Ok(source);
    }

    [HttpGet("Configs/{IdConfig}")]
    public ActionResult<List<Sources>> GetSources(int IdConfig)
    {
        List<Sources> source = context.Sources.Where(x => x.Id_Config == IdConfig).ToList();

        if (source.Count == 0)
            return NotFound();
        else
            return Ok(source);
    }

    [HttpPost] // funguje, ale je potřeba dát do id config již nějaké existující id configu jinak ta kontrola na webovkách se dodrbe
    public ActionResult Post([FromBody] PathsDto path)
    {
        if (!context.Config.Any(x => x.Id == path.Id_Config))
        {
            return BadRequest("Object doesn't have existing id in Configs");
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
        Sources source = context.Sources.Find(Id);

        if (source == null)
            return NotFound("Object does not exists");

        source.Id_Config = path.Id_Config;
        source.Path = path.Path;

        context.SaveChanges();

        return Ok();
    }
}