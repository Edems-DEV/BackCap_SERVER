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
        return Ok(context.Config.ToList().Select(x => new WebConfigDto(x, context)).ToList());
    }

    [HttpGet("names")]
    public ActionResult<List<WebNameDto>> GetNames()
    {
        return Ok(context.Config.Select(x => new WebNameDto(x.Id, x.Name)).ToList());
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

        return Ok(new WebConfigDto(config, context));
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

        try
        {
            //validation.DateTimeValidator(config.Interval_end.ToString());
        }
        catch (Exception)
        {
            return BadRequest("Invalid DateTime");
        }

        Config result = context.Config.Find(Id);

        if (result == null)
            return NotFound("Object does not exists");

        // update configu a jobů k němu
        result.GetData(config.GetConfig(context));

        context.SaveChanges();
        return Ok();
    }

    [HttpDelete("{Id}")]
    public ActionResult Delete(int Id)
    {
        Config config = context.Config.Find(Id);

        if (config == null)
            return NotFound("Object does not exists");

        context.Config.Remove(config);
        context.SaveChanges();
        return Ok();
    }
}
