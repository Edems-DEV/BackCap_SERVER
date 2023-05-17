﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Server.DatabaseTables;
using Server.ParamClasses;
using Server.Validator;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DestinationsController : Controller
{
    private readonly MyContext context = new MyContext();

    [HttpGet("{Id}")]
    public ActionResult<Destination> Get(int Id)
    {
        Destination destination = context.Destination.Find(Id);

        if (destination == null)
            return NotFound("Object does not exists");

        return Ok(destination);
    }

    [HttpGet("Configs/{IdConfig}")]
    public ActionResult<Destination> GetDestinations(int IdConfig)
    {
        List<Destination> sources = context.Destination.Where(x => x.Id_Config == IdConfig).ToList();

        if (sources.Count == 0)
            return NotFound();

        return Ok(sources);
    }

    [HttpPost]
    public ActionResult Post([FromBody] PathsDto path)
    {
        if (context.Config.Find(path.Id_Config) == null)
        {
            return BadRequest("Object doesn't have existing id in Configs");
        }

        Destination destination = new()
        {
            Id_Config = path.Id_Config,
            DestPath = path.Path
        };

        context.Add(destination);
        context.SaveChanges();

        return Ok();
    }

    [HttpPut("{Id}")]
    public ActionResult Put(int Id, [FromBody] PathsDto path)
    {
        Destination destination = context.Destination.Find(Id);

        if (destination == null)
            return NotFound("Object does not exists");

        destination.Id_Config = path.Id_Config;
        destination.DestPath = path.Path;

        context.SaveChanges();

        return Ok();
    }
}