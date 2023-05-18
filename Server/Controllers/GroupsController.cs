using Microsoft.AspNetCore.Mvc;
using Server.DatabaseTables;
using Server.Dtos;
using Server.Validator;

namespace Server.Controllers;

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
        //temp
        return Ok(context.Groups.ToList().Select(x => new WebGroupDto(x, context)).ToList());
    }

    [HttpGet("names")]
    public ActionResult<List<WebNameDto>> GetNames()
    {
        return Ok(context.Groups.Select(x => new WebNameDto(x.Id, x.Name)).ToList());
    }

    [HttpGet("count")]
    public ActionResult<int> GetCount()
    {
        return Ok(context.Groups.Count());
    }

    [HttpGet("{Id}")]
    public ActionResult<WebGroupDto> Get(int Id)
    {
        Groups group = context.Groups.Find(Id);

        if (group == null)
            return NotFound();

        return Ok(new WebGroupDto(group, context));
    }

    [HttpPost]
    public ActionResult Post([FromBody] Groups group)
    {
        context.Groups.Add(group);
        context.SaveChanges();

        return Ok();
    }

    [HttpPut("{Id}")]
    public ActionResult Put(int Id, [FromBody] WebGroupDto groupDto)
    {
        Groups group = context.Groups.Find(Id);

        if (group == null)
            return NotFound();

        group = groupDto.UpdateGroup(group, context);

        context.SaveChanges();

        return Ok();
    }

    [HttpDelete("{Id}")]
    public ActionResult Delete(int Id)
    {
        Groups group = context.Groups.Find(Id);

        if (group == null)
            return NotFound();

        context.Groups.Remove(group);
        context.SaveChanges();
        return Ok();
    }
}
