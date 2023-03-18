using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.ParamClasses;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SourcesController : Controller
{
    private readonly MyContext context = new MyContext();

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
        Sources source = context.Sources.Find(id);

        source.id_Config = path.Id_Config;
        source.path = path.Path;

        context.Add(source);
        context.SaveChanges();
    }
}