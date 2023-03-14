using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.ParamClasses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class GroupController : ControllerBase
{
    private readonly MyContext context = new MyContext();

    //group
    [HttpPost("Group/post/new/")]
    public void GroupPostNew([FromBody] string name)
    {
        Groups NewGroup = new Groups();
        NewGroup.Name = name;

        context.Groups.Add(NewGroup);
        context.SaveChanges();
    }

    [HttpPut("Group/put/edit/")]
    public void GroupPutEdit(int id, [FromBody] string name)
    {
        Groups result = context.Groups.Find(id);
        result.Name = name;
        context.SaveChanges();
    }

    [HttpGet("Group/get/id/")]
    public Groups GroupGetId(int groupId)
    {
        return context.Groups.Find(groupId);
    }
}
