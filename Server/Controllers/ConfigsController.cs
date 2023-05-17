using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using MySql.Data.MySqlClient;
using Server.DatabaseTables;
using Server.Dtos;
using Server.ParamClasses;
using Server.Services;
using Server.Validator;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConfigsController : Controller
{
    private readonly Validators validation;
    private readonly MyContext context = new MyContext();

    public ConfigsController(Validators validation)
    {
        this.validation = validation;
    }

    [HttpGet]
    public ActionResult<List<WebConfigDto>> Get()
    {
        return Ok(context.Config.ToList().Select(x => new WebConfigDto(x, context, x.Id)).ToList());
    }

    [HttpGet("names")]
    public ActionResult<List<WebOthersDto>> GetNames()
    {
        return Ok(context.Config.Select(x => new WebOthersDto(x.Id, x.Name)).ToList());
    }

    [HttpGet("count")]
    public ActionResult<int> GetCount()
    {
        return Ok(context.Config.Count());
    }

    [HttpGet("{Id}")]
    public ActionResult<WebConfigDto> Get(int Id)
    {
        Config config = context.Config.Find(Id);

        if (config == null)
            return NotFound();

        return Ok(new WebConfigDto(config, context, Id));
    }

    [HttpPost]
    public ActionResult Post([FromBody] Config config)
    {   
        try
        {
            validation.DateTimeValidator(config.Interval_end.ToString());
        }
        catch (Exception)
        {
            return BadRequest();
        }

        context.Config.Add(config);
        context.Sources.AddRange(config.Sources);
        context.Destination.AddRange(config.Destinations);
        context.SaveChanges();
        return Ok();
    }

    [HttpPut("{Id}")]
    public ActionResult Put(int Id, [FromBody] WebConfigDto config)
    {
        config.Id = Id;
        DatabaseManager databaseManager = new(context);

        
        try
        {
            validation.DateTimeValidator(config.Interval_end.ToString());
        }
        catch (Exception)
        {
            return BadRequest("Invalid DateTime");
        }

        Config result = context.Config.Find(Id);

        if (result == null)
            return NotFound("Object does not exists");

        // update configu // uloží nové cesty
        result.GetData(config.GetConfig(context));

        // update group a pc
        Job job = context.Job.Where(x => x.Id_Config == config.Id).FirstOrDefault();

        job.Id_Group = config.Group.Id;
        job.Id_Machine = config.Machine.Id;


        context.SaveChanges();
        return Ok();
    }

    [HttpDelete("{Id}")]
    public ActionResult Delete(int Id)
    {
        Config config = context.Config.Find(Id);

        if (config == null)
            return NotFound("Object does not exists");

        context.Sources.Where(x => x.Id_Config == config.Id).ToList().ForEach(y => context.Sources.Remove(y));
        context.Destination.Where(x => x.Id_Config == config.Id).ToList().ForEach(y => context.Destination.Remove(y));
        context.Config.Remove(config);
        context.SaveChanges();
        return Ok();
    }
}
