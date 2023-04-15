using Microsoft.AspNetCore.Mvc;
using Server.DatabaseTables;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class WebController : ControllerBase
{
    private readonly MyContext context = new MyContext();
    //GET: api/<WebController>
    [HttpGet("{Id}")]
    public ActionResult<Job> Get(int Id)
    {
        Job job = context.Job.Find(Id);

        return Ok(job);
    }
}
