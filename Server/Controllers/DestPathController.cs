using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.ParamClasses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DestPathController : ControllerBase
{
    private readonly MyContext context = new MyContext();


    [HttpPost("DestPath/Post/Paths/")]
    public void DestPathPostNew([FromBody] List<PathsDto> paths)
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

    [HttpPut("DestPath/Put/Path/")]
    public void DestPathPutEdit(int id, [FromBody] PathsDto path)
    {
        Destination destinations = context.Destination.Find(id);

        destinations.id_Config = path.Id_Config;
        destinations.DestPath = path.Path;

        context.Add(destinations);
        context.SaveChanges();
    }

    [HttpGet("DestPath/Get/IdConfig/")]
    public List<Sources> DestPathGetIdConfig(int id_config)
    {
        return context.Sources.Where(x => x.id_Config == id_config).ToList();
    }
}
