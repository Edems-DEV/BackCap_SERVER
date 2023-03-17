using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.ParamClasses;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExampleController : Controller
{
    private readonly MyContext context = new MyContext();

    [HttpGet]
    public IActionResult Get()
    {
        return NoContent(); //204
    }
}
