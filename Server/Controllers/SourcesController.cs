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
    public async Task<ActionResult<Sources>> GetSource(int Id)
    {
        var source = await context.Sources.FindAsync(Id);

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
    public  async Task<ActionResult> Post([FromBody] PathsDto path)
    {
        if (await context.Config.FindAsync(path.Id_Config) == null)
        {
            return BadRequest("Object doesn't have existing id in Configs");
        }

        Sources source = new Sources
        {
            Id_Config = path.Id_Config,
            Path = path.Path
        };

        await context.Sources.AddAsync(source);
        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("{Id}")]
    public async Task<ActionResult> Put(int Id, [FromBody] PathsDto path)
    {
        var source = await context.Sources.FindAsync(Id);

        if (source == null)
            return NotFound("Object does not exists");

        source.Id_Config = path.Id_Config;
        source.Path = path.Path;

        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{Id}")]
    public async Task<ActionResult> Delete(int Id)
    {
        Sources source = await context.Sources.FindAsync(Id);

        if (source == null)
            return NotFound();

        context.Sources.Remove(source);
        await context.SaveChangesAsync();

        return Ok();
    }
}