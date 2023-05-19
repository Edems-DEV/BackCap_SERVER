using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.Dtos;
using Server.Validator;

namespace Server.Controllers;

//[Authorize]
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

    [HttpGet]
    public ActionResult<List<WebConfigDto>> Get()
    {
        return Ok(context.Config.ToList().Select(x => new WebConfigDto(x, context)).ToList());
    }

    [HttpGet("names")]
    public async Task<ActionResult<List<WebNameDto>>> GetNames()
    {
        return Ok(await context.Config.Select(x => new WebNameDto(x.Id, x.Name)).ToListAsync());
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        return Ok(await context.Config.CountAsync());
    }

    [HttpGet("{Id}")]
    public async Task<ActionResult<WebConfigDto>> Get(int Id)
    {
        var config = await context.Config.FindAsync(Id);

        if (config == null)
            return NotFound("Object does not exists");

        return Ok(new WebConfigDto(config, context));
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] WebConfigDto webConfig)
    {   
        await webConfig.AddConfig(context);
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] WebConfigDto config)
    {
        config.Id = id;

        var result = await context.Config.FindAsync(id);

        if (result == null)
            return NotFound("Object does not exists");

        // update configu a jobů k němu
        result = await config.GetConfig(result, context);

        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var config = await context.Config.FindAsync(id);

        if (config == null)
            return NotFound("Object does not exists");

        context.Config.Remove(config);
        await context.SaveChangesAsync();
        return Ok();
    }
}
