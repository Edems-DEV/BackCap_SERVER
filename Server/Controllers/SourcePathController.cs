using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.DatabaseTables;
using Server.ParamClasses;

namespace Server.Controllers;
public class SourcePathController : Controller
{
    private readonly MyContext context = new MyContext();

    [HttpPost("SourcePath/Post/Paths/")]
    public void SourcePathPostNew([FromBody] List<PathsDto> paths)
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

    [HttpPut("SourcePath/Put/Path/")]
    public void SourcePathPutEdit(int id, [FromBody] PathsDto path)
    {
        Sources source = context.Sources.Find(id);

        source.id_Config = path.Id_Config;
        source.path = path.Path;

        context.Add(source);
        context.SaveChanges();
    }

    [HttpGet("SourcePath/Get/IdConfig/")]
    public List<Sources> SourcePathGetIdConfig(int id_config)
    {
        return context.Sources.Where(x => x.id_Config == id_config).ToList();
    }
}

