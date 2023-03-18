using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.ParamClasses;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DestinationsController : Controller
{
    private readonly MyContext context = new MyContext();

    [HttpGet("{id}")]
    public List<Sources> Get(int id)
    {
        return context.Sources.Where(x => x.id == id).ToList();
    }

    [HttpPost]
    public void Post([FromBody] List<PathsDto> paths)
    {
        List<Sources> sources = new List<Sources>();
        foreach (PathsDto item in paths)
        {
            Sources source = new Sources
            {
                id_Config = item.Id_Config,
                path = item.Path
            };

            sources.Add(source);
        }

        context.AddRange(sources);
        context.SaveChanges();
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] PathsDto path)
    {
        Destination destinations = context.Destination.Find(id);

        destinations.id_Config = path.Id_Config;
        destinations.DestPath = path.Path;

        context.Add(destinations);
        context.SaveChanges();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var destination = context.Destination.Find(id);
        if (destination == null)
        {
            return NotFound();
        }
        context.Destination.Remove(destination);
        context.SaveChanges();
        return Ok($"Delete request received for destination id {id}.");
    }
}

// for what?