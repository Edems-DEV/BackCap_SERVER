using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.ParamClasses;

namespace Server.Controllers;

[Authorize]
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
        return Ok(context.Sources.Where(x => x.Id_Config == IdConfig).ToListAsync());
    }

    [HttpPost]
    public ActionResult Post([FromBody] PathsDto path)
    {
        if (context.Config.Find(path.Id_Config) == null)
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

    [HttpDelete("{Id}")]
    public ActionResult Delete(int Id)
    {
        Sources source = context.Sources.Find(Id);

        if (source == null)
            return NotFound();

        context.Sources.Remove(source);
        context.SaveChanges();

        return Ok();
    }
}