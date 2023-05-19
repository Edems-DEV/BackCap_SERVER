using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.Dtos;
using Server.Validator;

namespace Server.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class GroupsController : Controller
{
    private readonly Validators validators;
    private readonly MyContext context = new MyContext();

    public GroupsController(Validators validators)
    {
        this.validators = validators;
    }

    [HttpGet]
    public ActionResult Get()
    {
        return Ok(context.Groups.ToList().Select(x => new WebGroupDto(x, context)).ToList());
    }

    [HttpGet("names")]
    public async Task<ActionResult<List<WebNameDto>>> GetNames()
    {
        return Ok(await context.Groups.Select(x => new WebNameDto(x.Id, x.Name)).ToListAsync());
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        return Ok(await context.Groups.CountAsync());
    }

    [HttpGet("{Id}")]
    public async Task<ActionResult<WebGroupDto>> Get(int Id)
    {
        var group = await context.Groups.FindAsync(Id);

        if (group == null)
            return NotFound("object does not exists");

        return Ok(new WebGroupDto(group, context));
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] WebGroupDto group)
    {
        await group.AddGroup(context);
        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("{Id}")]
    public async Task<ActionResult> Put(int Id, [FromBody] WebGroupDto groupDto)
    {
        var group = await context.Groups.FindAsync(Id);
        groupDto.Id = Id;

        if (group == null)
            return NotFound();

        group = await groupDto.GetGroup(group, context);

        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{Id}")]
    public async Task<ActionResult> Delete(int Id)
    {
        var group = await context.Groups.FindAsync(Id);

        if (group == null)
            return NotFound();

        context.Groups.Remove(group);
        await context.SaveChangesAsync();
        return Ok();
    }
}
