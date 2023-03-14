using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;
using Server.ParamClasses;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ConfigController : ControllerBase
{
    private readonly MyContext context = new MyContext();

    [HttpPost]
    public void ConfigPostNew([FromBody] Config config)
    {
        Config NewConfig = new Config()
        {
            type = config.type,
            retention = config.retention,
            packageSize = config.packageSize,
            isCompressed = config.isCompressed,
            Backup_interval = config.Backup_interval,
            interval_end = config.interval_end
        };

        context.Config.Add(NewConfig);
        context.SaveChanges();
    }

    [HttpPut("{id}")]
    public void ConfigPutEdit(int id, [FromBody] Config config)
    {
        Config result = context.Config.Find(id);

        result.type = config.type;
        result.retention = config.retention;
        result.packageSize = config.packageSize;
        result.isCompressed = config.isCompressed;
        result.Backup_interval = config.Backup_interval;
        result.interval_end = config.interval_end;

        context.SaveChanges();
    }

    [HttpGet("{id}")]
    public Config ConfigGetId(int id)
    {
        return context.Config.Find(id);
    }
}
